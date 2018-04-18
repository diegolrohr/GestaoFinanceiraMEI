﻿using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System;
using System.Linq;
using Fly01.Core.Helpers;
using Fly01.Core.Entities.Domains.Enum;
using System.Data.Entity;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.BL
{
    public class ContaPagarBL : PlataformaBaseBL<ContaPagar>
    {
        private CondicaoParcelamentoBL condicaoParcelamentoBL;

        public ContaPagarBL(AppDataContext context, CondicaoParcelamentoBL condicaoParcelamentoBL) : base(context)
        {
            MustConsumeMessageServiceBus = true;

            this.condicaoParcelamentoBL = condicaoParcelamentoBL;
        }

        public virtual IQueryable<ContaPagar> Everything => repository.All.Where(x => x.PlataformaId == PlataformaUrl);

        public override void ValidaModel(ContaPagar entity)
        {
            entity.Fail(entity.Numero < 1, new Error("Número da conta inválido", "numero"));
            entity.Fail(Everything.Any(x => x.Numero == entity.Numero && x.Id != entity.Id), new Error("Número da conta duplicado", "numero"));

            base.ValidaModel(entity);
        }

        public override void Insert(ContaPagar entity)
        {
            const int limiteSemanal = 208;
            const int limiteMensal = 48;
            const int limiteAnual = 4;

            entity.PlataformaId = PlataformaUrl;
            entity.UsuarioInclusao = AppUser;
            
            entity.Fail(entity.Repetir && entity.TipoPeriodicidade == TipoPeriodicidade.Nenhuma, TipoPeriodicidadeInvalida);
            entity.Fail(entity.Repetir && !entity.NumeroRepeticoes.HasValue, NumeroRepeticoesInvalido);

            entity.Fail((entity.Repetir && entity.TipoPeriodicidade != TipoPeriodicidade.Nenhuma && entity.NumeroRepeticoes.HasValue) &&
                ((entity.TipoPeriodicidade == TipoPeriodicidade.Semanal && !(entity.NumeroRepeticoes.Value > 0 && entity.NumeroRepeticoes.Value <= limiteSemanal)) ||
                (entity.TipoPeriodicidade == TipoPeriodicidade.Mensal && !(entity.NumeroRepeticoes.Value > 0 && entity.NumeroRepeticoes.Value <= limiteMensal)) ||
                (entity.TipoPeriodicidade == TipoPeriodicidade.Anual && !(entity.NumeroRepeticoes.Value > 0 && entity.NumeroRepeticoes.Value <= limiteAnual)))
            , RepeticoesInvalidas);

            //na nova Transação já é informado o id e o status
            if (entity.Id == default(Guid))
                entity.StatusContaBancaria = StatusContaBancaria.EmAberto;

            var max = Everything.Any(x => x.Id != entity.Id) ? Everything.Max(x => x.Numero) : 0;

            max = (max == 1 && !Everything.Any(x => x.Id != entity.Id && x.Ativo && x.Numero == 1)) ? 0 : max;

            var condicoesParcelamento = condicaoParcelamentoBL.GetPrestacoes(entity.CondicaoParcelamentoId, entity.DataVencimento, entity.ValorPrevisto);
            Guid contaFinanceiraPrincipal = entity.Id == default(Guid) ? Guid.NewGuid() : entity.Id;
            for (int iParcela = 0; iParcela < condicoesParcelamento.Count(); iParcela++)
            {
                var parcela = condicoesParcelamento[iParcela];

                var itemContaPagar = new ContaPagar();
                entity.CopyProperties<ContaPagar>(itemContaPagar);

                // CopyProperties não copia as notificações
                itemContaPagar.Notification.Errors.AddRange(entity.Notification.Errors);

                itemContaPagar.DataVencimento = parcela.DataVencimento;
                itemContaPagar.DescricaoParcela = parcela.DescricaoParcela;
                itemContaPagar.ValorPrevisto = parcela.Valor;

                itemContaPagar.Id = iParcela == default(int) ? contaFinanceiraPrincipal : Guid.NewGuid();

                itemContaPagar.Numero = ++max;

                base.Insert(itemContaPagar);

                if (entity.Repetir && entity.TipoPeriodicidade != TipoPeriodicidade.Nenhuma)
                {
                    for (int iRepeticao = 1; iRepeticao <= entity.NumeroRepeticoes; iRepeticao++)
                    {
                        var itemContaPagarRepeticao = new ContaPagar();

                        itemContaPagar.CopyProperties<ContaPagar>(itemContaPagarRepeticao);

                        // CopyProperties não copia as notificações
                        itemContaPagarRepeticao.Notification.Errors.AddRange(itemContaPagar.Notification.Errors);

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

                        itemContaPagarRepeticao.Numero = ++max;

                        base.Insert(itemContaPagarRepeticao);
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

            entity.Numero = contaPagarDb.Numero;

            base.Update(entity);
        }

        public static Error RepeticoesInvalidas = new Error("Número de repetições inválido. Somente até 48 Meses (4 Anos / 208 Semanas).");
        public static Error AlteracaoCondicaoParcelamento = new Error("Não é permitido alterar a condição de parcelamento.");
        public static Error AlteracaoConfiguracaoRecorrencia = new Error("Não é permitido alterar as configurações de recorrência.");

        public static Error TipoPeriodicidadeInvalida = new Error("Periodicidade inválida", "tipoPeriodicidade");
        public static Error NumeroRepeticoesInvalido = new Error("Número de repetições inválido", "numeroRepeticoes");
    }
}
