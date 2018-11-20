﻿using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using Fly01.Financeiro.API.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Fly01.Financeiro.BL
{
    public class ContaPagarBL : PlataformaBaseBL<ContaPagar>
    {
        private CondicaoParcelamentoBL condicaoParcelamentoBL;
        private ContaFinanceiraBaixaBL contaFinanceiraBaixaBL;
        private PessoaBL pessoaBL;

        public ContaPagarBL(AppDataContext context, CondicaoParcelamentoBL condicaoParcelamentoBL, ContaFinanceiraBaixaBL contaFinanceiraBaixaBL, PessoaBL pessoaBL) : base(context)
        {
            this.condicaoParcelamentoBL = condicaoParcelamentoBL;
            this.contaFinanceiraBaixaBL = contaFinanceiraBaixaBL;
            this.pessoaBL = pessoaBL;

            MustConsumeMessageServiceBus = true;
        }

        public override void Insert(ContaPagar entity)
        {
            var numero = default(int);

            entity.PlataformaId = PlataformaUrl;
            entity.UsuarioInclusao = AppUser;

            var repetir = RepeticaoValida(entity);

            entity.ValorPrevisto = Math.Round(entity.ValorPrevisto, 2);
            entity.ValorPago = entity.ValorPago.HasValue ? Math.Round(entity.ValorPago.Value, 2) : entity.ValorPago;

            //na nova Transação e quando status nao definido
            if (entity.StatusContaBancaria == default(StatusContaBancaria))
                entity.StatusContaBancaria = StatusContaBancaria.EmAberto;

            //Se Cliente não informado, busca pelo nome ou Insere
            if (!GuidHelper.IsValidGuid(entity.PessoaId) && !string.IsNullOrEmpty(entity.NomePessoa))
                entity.PessoaId = pessoaBL.BuscaPessoaNome(entity.NomePessoa, false, true);

            if (!string.IsNullOrEmpty(entity.DescricaoParcela))
            {
                //post bemacash ignorando condicao parcelamento
                if (entity.Id == default(Guid)) entity.Id = Guid.NewGuid();

                rpc = new RpcClient();
                numero = int.Parse(rpc.Call($"plataformaid={entity.PlataformaId},tipocontafinanceira={(int)TipoContaFinanceira.ContaPagar}"));

                entity.Numero = numero;

                base.Insert(entity);

                if (entity.StatusContaBancaria == StatusContaBancaria.Pago || entity.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                    contaFinanceiraBaixaBL.GeraContaFinanceiraBaixa(entity);
            }
            else
            {
                var condicoesParcelamento = condicaoParcelamentoBL.GetPrestacoes(entity.CondicaoParcelamentoId, entity.DataVencimento, entity.ValorPrevisto);
                var contaFinanceiraPrincipal = entity.Id == default(Guid) ? Guid.NewGuid() : entity.Id;

                for (int iParcela = 0; iParcela < condicoesParcelamento.Count; iParcela++)
                {
                    var parcela = condicoesParcelamento[iParcela];
                    var itemContaPagar = new ContaPagar();
                    entity.CopyProperties<ContaPagar>(itemContaPagar);
                    itemContaPagar.Notification.Errors.AddRange(entity.Notification.Errors); // CopyProperties não copia as notificações
                    itemContaPagar.DataVencimento = parcela.DataVencimento;
                    itemContaPagar.DescricaoParcela = parcela.DescricaoParcela;
                    itemContaPagar.ValorPrevisto = parcela.Valor;
                    itemContaPagar.ValorPago = entity.StatusContaBancaria == StatusContaBancaria.Pago ? parcela.Valor : entity.ValorPago;

                    if (iParcela == default(int))
                        itemContaPagar.Id = contaFinanceiraPrincipal;
                    else
                    {
                        itemContaPagar.Id = Guid.NewGuid();
                        itemContaPagar.ContaFinanceiraParcelaPaiId = contaFinanceiraPrincipal;

                        if (repetir)
                            itemContaPagar.ContaFinanceiraRepeticaoPaiId = contaFinanceiraPrincipal;
                    }

                    rpc = new RpcClient();
                    numero = int.Parse(rpc.Call($"plataformaid={entity.PlataformaId},tipocontafinanceira={(int)TipoContaFinanceira.ContaPagar}"));

                    itemContaPagar.Numero = numero;

                    base.Insert(itemContaPagar);

                    if (entity.StatusContaBancaria == StatusContaBancaria.Pago || entity.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                        contaFinanceiraBaixaBL.GeraContaFinanceiraBaixa(itemContaPagar);

                    if (repetir)
                    {
                        for (int iRepeticao = 1; iRepeticao <= entity.NumeroRepeticoes; iRepeticao++)
                        {
                            var itemContaPagarRepeticao = new ContaPagar();
                            itemContaPagar.CopyProperties<ContaPagar>(itemContaPagarRepeticao);
                            itemContaPagarRepeticao.ContaFinanceiraParcelaPaiId = null;
                            itemContaPagarRepeticao.Notification.Errors.AddRange(itemContaPagar.Notification.Errors); // CopyProperties não copia as notificações
                            itemContaPagarRepeticao.Id = default(Guid);
                            itemContaPagarRepeticao.ContaFinanceiraRepeticaoPaiId = contaFinanceiraPrincipal;                            

                            switch (entity.TipoPeriodicidade)
                            {
                                case TipoPeriodicidade.Semanal:
                                    itemContaPagarRepeticao.DataVencimento = itemContaPagarRepeticao.DataVencimento.AddDays(iRepeticao * 7);
                                    break;
                                case TipoPeriodicidade.Mensal:
                                    itemContaPagarRepeticao.DataVencimento = itemContaPagarRepeticao.DataVencimento.AddMonths(iRepeticao);
                                    break;
                                case TipoPeriodicidade.Anual:
                                    itemContaPagarRepeticao.DataVencimento = itemContaPagarRepeticao.DataVencimento.AddYears(iRepeticao);
                                    break;
                            }

                            rpc = new RpcClient();
                            numero = int.Parse(rpc.Call($"plataformaid={entity.PlataformaId},tipocontafinanceira={(int)TipoContaFinanceira.ContaPagar}"));

                            itemContaPagarRepeticao.Numero = numero;

                            base.Insert(itemContaPagarRepeticao);

                            if (entity.StatusContaBancaria == StatusContaBancaria.Pago || entity.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                                contaFinanceiraBaixaBL.GeraContaFinanceiraBaixa(itemContaPagarRepeticao);
                        }
                    }
                }
            }
        }

        public override void Update(ContaPagar entity)
        {
            var contaPagarDb = All.AsNoTracking().FirstOrDefault(x => x.Id == entity.Id);

            entity.Fail(contaPagarDb.CondicaoParcelamentoId != entity.CondicaoParcelamentoId, AlteracaoCondicaoParcelamento);
            entity.Fail((contaPagarDb.Repetir != entity.Repetir) || (contaPagarDb.TipoPeriodicidade != entity.TipoPeriodicidade) ||
                (contaPagarDb.NumeroRepeticoes != entity.NumeroRepeticoes), AlteracaoConfiguracaoRecorrencia);

            base.Update(entity);
        }

        public override void Delete(ContaPagar entityToDelete)
        {
            contaFinanceiraBaixaBL.All.Where(x => x.ContaFinanceiraId == entityToDelete.Id).OrderBy(x => x.DataInclusao).ToList()
                .ForEach(itemBaixa => { contaFinanceiraBaixaBL.Delete(itemBaixa); });

            base.Delete(entityToDelete);
        }

        public List<ContaPagar> GetParcelas(Guid contaFinanceiraParcelaPaiId)
        {
            var parcelas = new List<ContaPagar>();

            if(All.Any(x => x.Id == contaFinanceiraParcelaPaiId))
                parcelas.Add(All.Where(x => x.Id == contaFinanceiraParcelaPaiId).AsNoTracking().FirstOrDefault());
            if(All.Any(x => x.ContaFinanceiraParcelaPaiId == contaFinanceiraParcelaPaiId))
                parcelas.AddRange(All.Where(x => x.ContaFinanceiraParcelaPaiId == contaFinanceiraParcelaPaiId).AsNoTracking().OrderBy(x => x.DataInclusao));

            return parcelas;
        }

        private static bool RepeticaoValida(ContaFinanceira entity)
        {
            if (entity.Repetir)
            {
                const int limiteSemanal = 208;
                const int limiteMensal = 48;
                const int limiteAnual = 4;

                entity.Fail(entity.TipoPeriodicidade == TipoPeriodicidade.Nenhuma, TipoPeriodicidadeInvalida);
                entity.Fail(!entity.NumeroRepeticoes.HasValue, NumeroRepeticoesInvalido);

                entity.Fail((entity.TipoPeriodicidade != TipoPeriodicidade.Nenhuma && entity.NumeroRepeticoes.HasValue) &&
                            ((entity.TipoPeriodicidade == TipoPeriodicidade.Semanal && !(entity.NumeroRepeticoes.Value > 0 && entity.NumeroRepeticoes.Value <= limiteSemanal)) ||
                             (entity.TipoPeriodicidade == TipoPeriodicidade.Mensal && !(entity.NumeroRepeticoes.Value > 0 && entity.NumeroRepeticoes.Value <= limiteMensal)) ||
                             (entity.TipoPeriodicidade == TipoPeriodicidade.Anual && !(entity.NumeroRepeticoes.Value > 0 && entity.NumeroRepeticoes.Value <= limiteAnual)))
                    , RepeticoesInvalidas);

                return entity.Repetir && entity.IsValid();
            }

            return false;
        }

        public static Error RepeticoesInvalidas = new Error("Número de repetições inválido. Somente até 48 Meses (4 Anos / 208 Semanas).");
        public static Error AlteracaoCondicaoParcelamento = new Error("Não é permitido alterar a condição de parcelamento.");
        public static Error AlteracaoConfiguracaoRecorrencia = new Error("Não é permitido alterar as configurações de recorrência.");
        public static Error TipoPeriodicidadeInvalida = new Error("Periodicidade inválida", "tipoPeriodicidade");
        public static Error NumeroRepeticoesInvalido = new Error("Número de repetições inválido", "numeroRepeticoes");
    }
}