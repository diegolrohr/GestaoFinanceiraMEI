using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Financeiro.API.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Fly01.Financeiro.BL
{
    public class ContaReceberBL : PlataformaBaseBL<ContaReceber>
    {
        private CondicaoParcelamentoBL condicaoParcelamentoBL;
        private ContaFinanceiraBaixaBL contaFinanceiraBaixaBL;
        private PessoaBL pessoaBL;

        public ContaReceberBL(AppDataContext context, CondicaoParcelamentoBL condicaoParcelamentoBL, ContaFinanceiraBaixaBL contaFinanceiraBaixaBL, PessoaBL pessoaBL) : base(context)
        {
            this.condicaoParcelamentoBL = condicaoParcelamentoBL;
            this.contaFinanceiraBaixaBL = contaFinanceiraBaixaBL;
            this.pessoaBL = pessoaBL;
            MustConsumeMessageServiceBus = true;
        }

        public override void Insert(ContaReceber entity)
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
                entity.PessoaId = pessoaBL.BuscaPessoaNome(entity.NomePessoa, true, false);

            if (!string.IsNullOrEmpty(entity.DescricaoParcela))
            {
                //post bemacash ignorando condicao parcelamento
                if (entity.Id == default(Guid)) entity.Id = Guid.NewGuid();

                rpc = new RpcClient();
                numero = int.Parse(rpc.Call($"plataformaid={entity.PlataformaId},tipocontafinanceira={(int)TipoContaFinanceira.ContaReceber}"));
                //numero = All.Max(x => x.Numero) + 1;
                entity.Numero = numero;

                base.Insert(entity);

                if (entity.StatusContaBancaria == StatusContaBancaria.Pago || entity.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                    contaFinanceiraBaixaBL.GeraContaFinanceiraBaixa(entity);

            }
            else
            {
                var condicoesParcelamento = condicaoParcelamentoBL.GetPrestacoes(entity.CondicaoParcelamentoId, entity.DataVencimento, entity.ValorPrevisto);
                var contaFinanceiraPrincipal = entity.Id == default(Guid) ? Guid.NewGuid() : entity.Id;

                GravaParcelamentoRepeticoes(entity, repetir, condicoesParcelamento, contaFinanceiraPrincipal);
            }
        }

        private void GravaParcelamentoRepeticoes(ContaReceber entity, bool repetir, List<CondicaoParcelamentoParcela> condicoesParcelamento, Guid contaFinanceiraPrincipal)
        {
            //numero = All.Max(x => x.Numero) + 1;
            rpc = new RpcClient();
            var numero = default(int);
            numero = int.Parse(rpc.Call($"plataformaid={entity.PlataformaId},tipocontafinanceira={(int)TipoContaFinanceira.ContaReceber},add={condicoesParcelamento.Count}"));
            numero -= condicoesParcelamento.Count;

            for (int iParcela = 0; iParcela < condicoesParcelamento.Count; iParcela++)
            {
                numero += 1;
                var parcela = condicoesParcelamento[iParcela];
                var itemContaReceber = new ContaReceber();
                entity.CopyProperties<ContaReceber>(itemContaReceber);

                GravaParcelamento(entity, repetir, contaFinanceiraPrincipal, iParcela, parcela, itemContaReceber, numero);

                if (repetir)
                {
                    GravaRepeticoes(entity, contaFinanceiraPrincipal, itemContaReceber);
                }
            }
        }

        private void GravaParcelamento(ContaReceber entity, bool repetir, Guid contaFinanceiraPrincipal, int iParcela, CondicaoParcelamentoParcela parcela, ContaReceber itemContaReceber, int numero)
        {
            itemContaReceber.Notification.Errors.AddRange(entity.Notification.Errors);
            itemContaReceber.DataVencimento = parcela.DataVencimento;
            itemContaReceber.DescricaoParcela = parcela.DescricaoParcela;
            itemContaReceber.ValorPrevisto = parcela.Valor;
            itemContaReceber.ValorPago = entity.StatusContaBancaria == StatusContaBancaria.Pago ? parcela.Valor : entity.ValorPago;

            if (iParcela == default(int))
                itemContaReceber.Id = contaFinanceiraPrincipal;
            else
            {
                itemContaReceber.Id = Guid.NewGuid();
                itemContaReceber.ContaFinanceiraParcelaPaiId = contaFinanceiraPrincipal;

                if (repetir)
                    itemContaReceber.ContaFinanceiraRepeticaoPaiId = contaFinanceiraPrincipal;
            }

            itemContaReceber.Numero = numero;

            base.Insert(itemContaReceber);

            if (entity.StatusContaBancaria == StatusContaBancaria.Pago || entity.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                contaFinanceiraBaixaBL.GeraContaFinanceiraBaixa(itemContaReceber);
        }

        private void GravaRepeticoes(ContaReceber entity, Guid contaFinanceiraPrincipal, ContaReceber itemContaReceber)
        {
            rpc = new RpcClient();
            var numero = default(int);
            numero = int.Parse(rpc.Call($"plataformaid={entity.PlataformaId},tipocontafinanceira={(int)TipoContaFinanceira.ContaReceber},add={entity.NumeroRepeticoes ?? 1}"));
            numero -= entity.NumeroRepeticoes ?? numero;

            for (int iRepeticao = 1; iRepeticao <= entity.NumeroRepeticoes; iRepeticao++)
            {
                numero += 1;
                var itemContaReceberRepeticao = new ContaReceber();
                itemContaReceber.CopyProperties<ContaReceber>(itemContaReceberRepeticao);
                itemContaReceberRepeticao.ContaFinanceiraParcelaPaiId = null;
                itemContaReceberRepeticao.Notification.Errors.AddRange(itemContaReceber.Notification.Errors);
                itemContaReceberRepeticao.Id = default(Guid);
                itemContaReceberRepeticao.ContaFinanceiraRepeticaoPaiId = contaFinanceiraPrincipal;

                switch (entity.TipoPeriodicidade)
                {
                    case TipoPeriodicidade.Semanal:
                        itemContaReceberRepeticao.DataVencimento = itemContaReceberRepeticao.DataVencimento.AddDays(iRepeticao * 7);
                        break;
                    case TipoPeriodicidade.Mensal:
                        itemContaReceberRepeticao.DataVencimento = itemContaReceberRepeticao.DataVencimento.AddMonths(iRepeticao);
                        break;
                    case TipoPeriodicidade.Anual:
                        itemContaReceberRepeticao.DataVencimento = itemContaReceberRepeticao.DataVencimento.AddYears(iRepeticao);
                        break;
                }

                itemContaReceberRepeticao.Numero = numero;

                base.Insert(itemContaReceberRepeticao);

                if (entity.StatusContaBancaria == StatusContaBancaria.Pago || entity.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                    contaFinanceiraBaixaBL.GeraContaFinanceiraBaixa(itemContaReceberRepeticao);
            }
        }

        public override void Update(ContaReceber entity)
        {
            var contaReceberDb = All.AsNoTracking().FirstOrDefault(x => x.Id == entity.Id);

            entity.Fail(contaReceberDb.CondicaoParcelamentoId != entity.CondicaoParcelamentoId, AlteracaoCondicaoParcelamento);
            entity.Fail((contaReceberDb.Repetir != entity.Repetir) || (contaReceberDb.TipoPeriodicidade != entity.TipoPeriodicidade) ||
                (contaReceberDb.NumeroRepeticoes != entity.NumeroRepeticoes), AlteracaoConfiguracaoRecorrencia);

            base.Update(entity);
        }

        public override void Delete(ContaReceber entityToDelete)
        {
            contaFinanceiraBaixaBL.All.Where(x => x.ContaFinanceiraId == entityToDelete.Id).OrderBy(x => x.DataInclusao).ToList()
                .ForEach(itemBaixa => { contaFinanceiraBaixaBL.Delete(itemBaixa); });

            base.Delete(entityToDelete);
        }

        public List<ContaReceber> GetParcelas(Guid contaFinanceiraParcelaPaiId)
        {
            var parcelas = new List<ContaReceber>();


            if (All.Any(x => x.Id == contaFinanceiraParcelaPaiId))
                parcelas.Add(All.Where(x => x.Id == contaFinanceiraParcelaPaiId).AsNoTracking().FirstOrDefault());
            if (All.Any(x => x.ContaFinanceiraParcelaPaiId == contaFinanceiraParcelaPaiId))
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

        public List<ContaFinanceiraPorStatusVM> GetSaldoStatus(DateTime dataFinal, DateTime dataInicial)
        {
            List<ContaFinanceiraPorStatusVM> listaResult = new List<ContaFinanceiraPorStatusVM>();
            int QtdTotal = All.AsNoTracking().Where(x => x.DataEmissao >= dataInicial && x.DataEmissao <= dataFinal).Count();

            var result = All.AsNoTracking().Where(x => x.DataEmissao >= dataInicial && x.DataEmissao <= dataFinal)
                                            .Select(x => new { x.ValorPrevisto, x.StatusContaBancaria, x.ValorPago })
                                            .GroupBy(x => new { x.StatusContaBancaria })
                                            .Select(x => new
                                            {
                                                x.Key.StatusContaBancaria,
                                                Quantidade = x.Count(),
                                                valorTotal = x.Sum(y => y.ValorPrevisto),
                                                valorPago = x.Sum(y => y.ValorPago)
                                            })
                                            .ToList();

            double? resultDiferenca = result.Where(x => x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente).Select(x => x.valorTotal - x.valorPago)?.FirstOrDefault()?? 0;
            double? valorPago = result.Where(x => x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente).Select(x => x.valorPago)?.FirstOrDefault()?? 0;

            result.ForEach(x =>
            {
                listaResult.Add(new ContaFinanceiraPorStatusVM
                {
                    Status = EnumHelper.GetValue(typeof(StatusContaBancaria), x.StatusContaBancaria.ToString()),
                    Quantidade = x.Quantidade,
                    QuantidadeTotal = QtdTotal,
                    Valortotal = (x.StatusContaBancaria.ToString() == "EmAberto")
                         ? x.valorTotal + (double)resultDiferenca
                         : (x.StatusContaBancaria.ToString() == "Pago")
                            ? x.valorTotal + (double)valorPago
                            : x.valorTotal
                });
            });

            if (!result.Where(x => x.StatusContaBancaria == StatusContaBancaria.EmAberto).Any() && result != null)
            {
                listaResult.Add(new ContaFinanceiraPorStatusVM()
                {
                    Status = "Em aberto",
                    Valortotal = (double)resultDiferenca
                });
            }

            return listaResult;
        }

        public static Error RepeticoesInvalidas = new Error("Número de repetições inválido. Somente até 48 Meses (4 Anos / 208 Semanas).");
        public static Error AlteracaoCondicaoParcelamento = new Error("Não é permitido alterar a condição de parcelamento.");
        public static Error AlteracaoConfiguracaoRecorrencia = new Error("Não é permitido alterar as configurações de recorrência.");
        public static Error TipoPeriodicidadeInvalida = new Error("Periodicidade inválida", "tipoPeriodicidade");
        public static Error NumeroRepeticoesInvalido = new Error("Número de repetições inválido", "numeroRepeticoes");
    }
}