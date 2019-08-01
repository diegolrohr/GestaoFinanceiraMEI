using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System.Linq;
using System;

namespace Fly01.Financeiro.BL
{
    public class RollbackFinanceiroCompraVendaBL : PlataformaBaseBL<RollbackFinanceiroCompraVenda>
    {
        protected ContaReceberBL ContaReceberBL;
        protected ContaPagarBL ContaPagarBL;
        
        public RollbackFinanceiroCompraVendaBL(AppDataContext context, ContaReceberBL contaReceberBL, ContaPagarBL contaPagarBL) : base(context)
        {
            ContaReceberBL = contaReceberBL;
            ContaPagarBL = contaPagarBL;
            MustConsumeMessageServiceBus = true;
        }

        public override void Insert(RollbackFinanceiroCompraVenda entity)
        {
            var contasPagar =
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
                x.ContaFinanceiraParcelaPaiId == (entity.ContaFinanceiraParcelaPaiIdServicos.HasValue ? entity.ContaFinanceiraParcelaPaiIdServicos.Value : default(Guid))
            )).Union(
            ContaPagarBL.All.Where(x =>
                x.Observacao.Contains($"venda nº {entity.NumeroPedido}") && x.PessoaId == (entity.TransportadoraId.HasValue ? entity.TransportadoraId.Value : default(Guid))
            ));

            foreach (var contaPagar in contasPagar)
            {
                ContaPagarBL.Delete(contaPagar);
            }

            var contasReceber =
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

            foreach (var contaReceber in contasReceber)
            {
                ContaReceberBL.Delete(contaReceber);
            }
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