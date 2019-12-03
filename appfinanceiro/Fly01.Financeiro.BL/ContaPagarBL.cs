using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Financeiro.API.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Fly01.Financeiro.BL
{
    public class ContaPagarBL : EmpresaBaseBL<ContaPagar>
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
            entity.EmpresaId = EmpresaId;
            entity.UsuarioInclusao = AppUser;

            var repetir = RepeticaoValida(entity);

            entity.ValorPrevisto = Math.Round(entity.ValorPrevisto, 2);
            entity.ValorPago = entity.ValorPago.HasValue ? Math.Round(entity.ValorPago.Value, 2) : entity.ValorPago;

            //na nova Transação e quando status nao definido
            if (entity.StatusContaBancaria == default(StatusContaBancaria))
                entity.StatusContaBancaria = StatusContaBancaria.EmAberto;

            var condicoesParcelamento = condicaoParcelamentoBL.GetPrestacoes(entity.CondicaoParcelamentoId, entity.DataVencimento, entity.ValorPrevisto);
            var contaFinanceiraPrincipal = entity.Id == default(Guid) ? Guid.NewGuid() : entity.Id;

            GravaParcelamentoRepeticoes(entity, repetir, condicoesParcelamento, contaFinanceiraPrincipal);
        }

        private void GravaParcelamentoRepeticoes(ContaPagar entity, bool repetir, List<CondicaoParcelamentoParcela> condicoesParcelamento, Guid contaFinanceiraPrincipal)
        {
            var numero = default(int);
            if (All.Any())
            {
                numero = All.Max(x => x.Numero);
            }

            for (int iParcela = 0; iParcela < condicoesParcelamento.Count; iParcela++)
            {
                numero += 1;
                var parcela = condicoesParcelamento[iParcela];
                var itemContaPagar = new ContaPagar();
                entity.CopyProperties<ContaPagar>(itemContaPagar);

                GravaParcelamento(entity, repetir, contaFinanceiraPrincipal, iParcela, parcela, itemContaPagar, numero);

                if (repetir)
                {
                    GravaRepeticoes(entity, contaFinanceiraPrincipal, itemContaPagar);
                }
            }
        }

        private void GravaParcelamento(ContaPagar entity, bool repetir, Guid contaFinanceiraPrincipal, int iParcela, CondicaoParcelamentoParcela parcela, ContaPagar itemContaPagar, int numero)
        {
            itemContaPagar.Notification.Errors.AddRange(entity.Notification.Errors);
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

            itemContaPagar.Numero = numero;

            base.Insert(itemContaPagar);

            if (entity.StatusContaBancaria == StatusContaBancaria.Pago || entity.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                contaFinanceiraBaixaBL.GeraContaFinanceiraBaixa(itemContaPagar);
        }

        private void GravaRepeticoes(ContaPagar entity, Guid contaFinanceiraPrincipal, ContaPagar itemContaPagar)
        {
            var numero = default(int);
            if (All.Any())
            {
                numero = All.Max(x => x.Numero);
            }

            for (int iRepeticao = 1; iRepeticao <= entity.NumeroRepeticoes; iRepeticao++)
            {
                numero += 1;
                var itemContaPagarRepeticao = new ContaPagar();
                itemContaPagar.CopyProperties<ContaPagar>(itemContaPagarRepeticao);
                itemContaPagarRepeticao.ContaFinanceiraParcelaPaiId = null;
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
                itemContaPagarRepeticao.DataEmissao = itemContaPagarRepeticao.DataVencimento;
                itemContaPagarRepeticao.Numero = numero;

                base.Insert(itemContaPagarRepeticao);

                if (entity.StatusContaBancaria == StatusContaBancaria.Pago || entity.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                    contaFinanceiraBaixaBL.GeraContaFinanceiraBaixa(itemContaPagarRepeticao);
            }
        }

        public List<ContaFinanceiraPorStatusVM> GetSaldoStatus(DateTime dataFinal, DateTime dataInicial)
        {
            List<ContaFinanceiraPorStatusVM> listaResult = new List<ContaFinanceiraPorStatusVM>();
            int QtdTotal = All.AsNoTracking().Where(x => x.DataVencimento >= dataInicial && x.DataVencimento <= dataFinal).Count();

            var result = All.AsNoTracking().Where(x => x.DataVencimento >= dataInicial && x.DataVencimento <= dataFinal)
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

            double? resultDiferenca = result.Where(x => x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente).Select(x => x.valorTotal - x.valorPago).FirstOrDefault() ?? 0;
            double? valorPago = result.Where(x => x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente).Select(x => x.valorPago).FirstOrDefault() ?? 0;

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

        public static Error RepeticoesInvalidas = new Error("Número de repetições inválido. Somente até 48 Meses (4 Anos / 208 Semanas).");
        public static Error AlteracaoCondicaoParcelamento = new Error("Não é permitido alterar a condição de parcelamento.");
        public static Error AlteracaoConfiguracaoRecorrencia = new Error("Não é permitido alterar as configurações de recorrência.");
        public static Error TipoPeriodicidadeInvalida = new Error("Periodicidade inválida", "tipoPeriodicidade");
        public static Error NumeroRepeticoesInvalido = new Error("Número de repetições inválido", "numeroRepeticoes");
    }
}