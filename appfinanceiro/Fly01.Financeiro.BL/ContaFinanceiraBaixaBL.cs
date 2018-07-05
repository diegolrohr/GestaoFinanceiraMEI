using System;
using System.Linq;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.BL
{
    public class ContaFinanceiraBaixaBL : PlataformaBaseBL<ContaFinanceiraBaixa>
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

            if (contaFinanceira == null)
                throw new BusinessException("Conta inválida.");

            entity.ContaFinanceiraId = contaFinanceira.Id;

            entity.Fail(!contaBancariaBL.All.Any(x => x.Id == entity.ContaBancariaId), ContaInvalida);
            entity.Fail(entity.Valor > contaFinanceira.ValorPrevisto, ValorPagoInvalido);
            entity.Fail(valorPagoConta + entity.Valor > contaFinanceira.ValorPrevisto, SomaValoresInvalida);

            base.Insert(entity);

            valorPagoConta += entity.Valor;

            //Atualiza Conta Financeira
            contaFinanceira.ValorPago = valorPagoConta;
            if (contaFinanceira.ValorPago < contaFinanceira.ValorPrevisto)
                contaFinanceira.StatusContaBancaria = StatusContaBancaria.BaixadoParcialmente;
            else
                contaFinanceira.StatusContaBancaria = StatusContaBancaria.Pago;
            
            //Atualiza Saldo Histórico
            saldoHistoricoBL.AtualizaSaldoHistorico(entity.Data, entity.Valor, entity.ContaBancariaId, contaFinanceira.TipoContaFinanceira);

            //Atualiza movimentações
            movimentacaoBL.CriaMovimentacao(entity.Data, entity.Valor, entity.ContaBancariaId, contaFinanceira.TipoContaFinanceira, entity.ContaFinanceiraId);
        }

        public override void Update(ContaFinanceiraBaixa entity)
        {
            throw new BusinessException("Não é possível alterar uma baixa.");
        }

        public override void Delete(ContaFinanceiraBaixa entity)
        {
            ContaFinanceira contaFinanceira = contaFinanceiraBL.All.FirstOrDefault(x => x.Id == entity.ContaFinanceiraId);
            var valorPagoConta = contaFinanceira.ValorPago.HasValue ? (double)contaFinanceira.ValorPago : default(double);

            var valorBaixa = (entity.Valor * -1);
            valorPagoConta += valorBaixa;            

            //Atualiza Conta Financeira
            contaFinanceira.ValorPago = valorPagoConta;
            if(contaFinanceira.ValorPago > default(double))
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

                if (bancoOutros == null) throw new BusinessException("Nenhuma conta bancária encontrada.");

                contaFinanceira.ContaBancaria.BancoId = contaBancariaBL.All.FirstOrDefault(x => x.BancoId == bancoOutros.Id && x.RegistroFixo == true).Id;
            }

            if (contaFinanceira.ContaBancaria == null) throw new BusinessException("Conta bancária inválida.");

            saldoHistoricoBL.AtualizaSaldoHistorico(contaFinanceira.DataVencimento, contaFinanceira.ValorPrevisto, contaFinanceira.ContaBancariaId, contaFinanceira.TipoContaFinanceira);

            movimentacaoBL.CriaMovimentacao(contaFinanceira.DataVencimento, contaFinanceira.ValorPrevisto, contaFinanceira.ContaBancariaId, contaFinanceira.TipoContaFinanceira, contaFinanceira.Id);

            base.Insert(new ContaFinanceiraBaixa()
            {
                Data = contaFinanceira.DataVencimento,
                ContaFinanceiraId = contaFinanceira.Id,
                ContaBancariaId = contaFinanceira.ContaBancariaId,
                Valor = contaFinanceira.ValorPrevisto,
                Observacao = contaFinanceira.Descricao
            });
        }

        public static Error ContaInvalida = new Error("Conta Bancária inválida.");
        public static Error ValorPagoInvalido = new Error("Valor pago não pode ser superior ao valor da conta.");
        public static Error SomaValoresInvalida = new Error("Somatório dos valores não pode ser superior ao valor da conta.");
    }
}