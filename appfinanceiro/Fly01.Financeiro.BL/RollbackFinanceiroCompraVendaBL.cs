using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System.Linq;
using System;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.BL
{
    public class RollbackFinanceiroCompraVendaBL : PlataformaBaseBL<RollbackFinanceiroCompraVenda>
    {
        protected ContaReceberBL ContaReceberBL;
        protected ContaPagarBL ContaPagarBL;
        protected RenegociacaoContaFinanceiraOrigemBL RenegociacaoContaFinanceiraOrigemBL;
        protected ContaFinanceiraRenegociacaoBL ContaFinanceiraRenegociacaoBL;
        protected RenegociacaoContaFinanceiraRenegociadaBL RenegociacaoContaFinanceiraRenegociadaBL;

        public RollbackFinanceiroCompraVendaBL(AppDataContext context, ContaReceberBL contaReceberBL, ContaPagarBL contaPagarBL, RenegociacaoContaFinanceiraOrigemBL renegociacaoContaFinanceiraOrigemBL, ContaFinanceiraRenegociacaoBL contaFinanceiraRenegociacaoBL) : base(context)
        {
            ContaReceberBL = contaReceberBL;
            ContaPagarBL = contaPagarBL;
            RenegociacaoContaFinanceiraOrigemBL = renegociacaoContaFinanceiraOrigemBL;
            ContaFinanceiraRenegociacaoBL = contaFinanceiraRenegociacaoBL;
            MustConsumeMessageServiceBus = true;
        }

        public override void Insert(RollbackFinanceiroCompraVenda entity)
        {
            foreach (var contaPagar in GetContasPagar(entity))
            {
                ContaPagarBL.Delete(contaPagar);
            }            

            foreach (var contaReceber in GetContasReceber(entity))
            {                
                ContaReceberBL.Delete(contaReceber);
            }
        }

        public bool VerificaContasRenegociadas(RollbackFinanceiroCompraVenda entity)
        {
            var contasPagar = GetContasPagar(entity);
            var contasReceber = GetContasReceber(entity);

            return (contasPagar != null && contasPagar.Any(x => x.StatusContaBancaria == StatusContaBancaria.Renegociado)) ||
            (contasReceber != null && contasReceber.Any(x => x.StatusContaBancaria == StatusContaBancaria.Renegociado));
        }

        public IQueryable<ContaPagar> GetContasPagar(RollbackFinanceiroCompraVenda entity)
        {
            var observacaoPedido = $"venda nº {entity.NumeroPedido}";
            return
            ContaPagarBL.All.Where(x =>
                x.Id == (entity.ContaFinanceiraParcelaPaiIdProdutos.HasValue ? entity.ContaFinanceiraParcelaPaiIdProdutos.Value : default(Guid))
            ).Union(
            ContaPagarBL.All.Where(x =>
                x.ContaFinanceiraParcelaPaiId == (entity.ContaFinanceiraParcelaPaiIdProdutos.HasValue ? entity.ContaFinanceiraParcelaPaiIdProdutos.Value : default(Guid))
            )).Union(
            ContaPagarBL.All.Where(x =>
                x.Id == (entity.ContaFinanceiraParcelaPaiIdServicos.HasValue ? entity.ContaFinanceiraParcelaPaiIdServicos.Value : default(Guid))
            )).Union(
            ContaPagarBL.All.Where(x =>
                x.ContaFinanceiraParcelaPaiId == (entity.ContaFinanceiraParcelaPaiIdServicos.HasValue ? entity.ContaFinanceiraParcelaPaiIdServicos.Value : default(Guid)
            )).Union(
            ContaPagarBL.All.Where(x =>
                x.Observacao.Contains(observacaoPedido) && x.PessoaId == (entity.TransportadoraId.HasValue ? entity.TransportadoraId.Value : default(Guid))
            )));
        }

        public IQueryable<ContaReceber> GetContasReceber(RollbackFinanceiroCompraVenda entity)
        {
            return
            ContaReceberBL.All.Where(x =>
                x.Id == (entity.ContaFinanceiraParcelaPaiIdProdutos.HasValue ? entity.ContaFinanceiraParcelaPaiIdProdutos.Value : default(Guid))
            ).Union(
            ContaReceberBL.All.Where(x =>
                x.ContaFinanceiraParcelaPaiId == (entity.ContaFinanceiraParcelaPaiIdProdutos.HasValue ? entity.ContaFinanceiraParcelaPaiIdProdutos.Value : default(Guid))
            )).Union(
            ContaReceberBL.All.Where(x =>
                x.Id == (entity.ContaFinanceiraParcelaPaiIdServicos.HasValue ? entity.ContaFinanceiraParcelaPaiIdServicos.Value : default(Guid))
            )).Union(
            ContaReceberBL.All.Where(x =>
                x.ContaFinanceiraParcelaPaiId == (entity.ContaFinanceiraParcelaPaiIdServicos.HasValue ? entity.ContaFinanceiraParcelaPaiIdServicos.Value : default(Guid))
            ));
        }

        public override void Update(RollbackFinanceiroCompraVenda entity)
        {
            throw new BusinessException("Não é possível alterar um movimento de baixa");
        }

        public override void Delete(RollbackFinanceiroCompraVenda entityToDelete)
        {
            throw new BusinessException("Não é possível excluir um movimento de baixa");
        }

    }
}