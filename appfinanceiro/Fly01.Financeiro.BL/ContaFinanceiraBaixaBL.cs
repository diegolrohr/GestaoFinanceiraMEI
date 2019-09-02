using System;
using System.Linq;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.BL
{
    public class ContaFinanceiraBaixaBL : EmpresaBaseBL<ContaFinanceiraBaixa>
    {
        private ContaFinanceiraBL contaFinanceiraBL;
        private ContaBancariaBL contaBancariaBL;
        private SaldoHistoricoBL saldoHistoricoBL;
        private MovimentacaoBL movimentacaoBL;
        private BancoBL bancoBL;

        public ContaFinanceiraBaixaBL(AppDataContext context, ContaFinanceiraBL contaFinanceiraBL, ContaBancariaBL contaBancariaBL, SaldoHistoricoBL saldoHistoricoBL, MovimentacaoBL movimentacaoBL, BancoBL bancoBL)
            : base(context)
        {
            this.contaFinanceiraBL = contaFinanceiraBL;
            this.contaBancariaBL = contaBancariaBL;
            this.saldoHistoricoBL = saldoHistoricoBL;
            this.movimentacaoBL = movimentacaoBL;
            this.bancoBL = bancoBL;
        }

        public override void Insert(ContaFinanceiraBaixa entity)
        {
            var contaFinanceira = entity.ContaFinanceiraId != default(Guid) ? contaFinanceiraBL.All.FirstOrDefault(x => x.Id == entity.ContaFinanceiraId) : entity.ContaFinanceira;
            var valorPagoConta = contaFinanceira.ValorPago.GetValueOrDefault(0);

            entity.ContaFinanceira = null;
            contaFinanceira.ValorPrevisto = Math.Round(contaFinanceira.ValorPrevisto, 2);

            if (contaFinanceira == null) throw new BusinessException("Conta inválida.");

            entity.ContaFinanceiraId = contaFinanceira.Id;

            entity.Fail(!contaBancariaBL.All.Any(x => x.Id == entity.ContaBancariaId), ContaInvalida);
            entity.Fail(Math.Round(entity.Valor, 2) > Math.Round(contaFinanceira.ValorPrevisto, 2), ValorPagoInvalido);
            entity.Fail(Math.Round((valorPagoConta + entity.Valor), 2) > Math.Round(contaFinanceira.ValorPrevisto, 2), SomaValoresInvalida);

            base.Insert(entity);

            valorPagoConta += entity.Valor;

            contaFinanceira.ValorPago = valorPagoConta;
            if (Math.Round(contaFinanceira.ValorPago.GetValueOrDefault(0), 2) < Math.Round(contaFinanceira.ValorPrevisto, 2))
                contaFinanceira.StatusContaBancaria = StatusContaBancaria.BaixadoParcialmente;
            else
                contaFinanceira.StatusContaBancaria = StatusContaBancaria.Pago;

            saldoHistoricoBL.AtualizaSaldoHistorico(entity.Data, entity.Valor, entity.ContaBancariaId, contaFinanceira.TipoContaFinanceira);

            movimentacaoBL.CriaMovimentacao(entity.Data, entity.Valor, entity.ContaBancariaId, contaFinanceira.TipoContaFinanceira, entity.ContaFinanceiraId);
        }

        public override void Update(ContaFinanceiraBaixa entity)
        {
            throw new BusinessException("Não é possível alterar uma baixa.");
        }

        public void DeleteWithoutRecalc(ContaFinanceiraBaixa entity)
        {
            base.Delete(entity);
        }

        public override void Delete(ContaFinanceiraBaixa entity)
        {
            ContaFinanceira contaFinanceira = contaFinanceiraBL.All.FirstOrDefault(x => x.Id == entity.ContaFinanceiraId);
            var valorPagoConta = contaFinanceira.ValorPago.HasValue ? (double)contaFinanceira.ValorPago : default(double);

            var valorBaixa = (entity.Valor * -1);
            valorPagoConta += valorBaixa;

            //Atualiza Conta Financeira
            contaFinanceira.ValorPago = valorPagoConta;
            if (contaFinanceira.ValorPago > default(double))
                contaFinanceira.StatusContaBancaria = StatusContaBancaria.BaixadoParcialmente;
            else
                contaFinanceira.StatusContaBancaria = StatusContaBancaria.EmAberto;

            //Atualiza Saldo Histórico
            saldoHistoricoBL.AtualizaSaldoHistorico(entity.Data, valorBaixa, entity.ContaBancariaId, contaFinanceira.TipoContaFinanceira);

            string descricao = "Estorno* " + contaFinanceira.Descricao;
            //Atualiza movimentações
            movimentacaoBL.CriaMovimentacao(DateTime.Now, valorBaixa, entity.ContaBancariaId, contaFinanceira.TipoContaFinanceira, entity.ContaFinanceiraId, descricao);

            base.Delete(entity);
        }

        public void GeraContaFinanceiraBaixa(ContaFinanceira contaFinanceira)
        {
            if (contaFinanceira.Id == default(Guid)) throw new BusinessException("Conta Financeira inválida.");

            if (contaFinanceira.ContaBancariaId != default(Guid))
                contaFinanceira.ContaBancaria = contaBancariaBL.Find(contaFinanceira.ContaBancariaId);
            else
            {
                var bancoOutros = bancoBL.All.FirstOrDefault(x => x.Codigo == "999");

                contaFinanceira.ContaBancariaId = contaBancariaBL.All.FirstOrDefault(x => x.BancoId == bancoOutros.Id).Id;
            }

            if (contaFinanceira.ContaBancaria == null && contaFinanceira.ContaBancariaId == null) throw new BusinessException("Conta bancária inválida.");

            saldoHistoricoBL.AtualizaSaldoHistorico(contaFinanceira.DataVencimento, contaFinanceira.ValorPrevisto, contaFinanceira.ContaBancariaId, contaFinanceira.TipoContaFinanceira);

            movimentacaoBL.CriaMovimentacao(contaFinanceira.DataVencimento, contaFinanceira.ValorPrevisto, contaFinanceira.ContaBancariaId, contaFinanceira.TipoContaFinanceira, contaFinanceira.Id);

            base.Insert(new ContaFinanceiraBaixa()
            {
                Data = contaFinanceira.DataVencimento,
                ContaFinanceiraId = contaFinanceira.Id,
                ContaBancariaId = contaFinanceira.ContaBancariaId,
                Valor = contaFinanceira.ValorPago.HasValue ? contaFinanceira.ValorPago.Value : contaFinanceira.ValorPrevisto,
                Observacao = contaFinanceira.Descricao
            });
        }

        public static Error ContaInvalida = new Error("Conta Bancária inválida.");
        public static Error ValorPagoInvalido = new Error("Valor pago não pode ser superior ao valor da conta.");
        public static Error SomaValoresInvalida = new Error("Somatório dos valores não pode ser superior ao valor da conta.");
    }
}