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

        private void DeleteRenegociadasRecursivo(Guid contaFinanceiraId)
        {
            var renegociacaoId = RenegociacaoContaFinanceiraOrigemBL.All.Where(x => x.ContaFinanceiraId == contaFinanceiraId)?.FirstOrDefault()?.ContaFinanceiraRenegociacaoId;
            var renegociacao = ContaFinanceiraRenegociacaoBL.All.Where(x => x.Id == renegociacaoId)?.FirstOrDefault();

            var recordsIds = (from c in RenegociacaoContaFinanceiraRenegociadaBL.AllIncluding(x => x.ContaFinanceira)
                              where c.ContaFinanceiraRenegociacaoId == renegociacaoId
                              select new
                              {
                                  c.ContaFinanceiraId,
                                  c.ContaFinanceira.TipoContaFinanceira
                              }).ToList();

            //Recursividade de uma possivel renegociação de renegociação...
            foreach (var item in recordsIds)
            {
                DeleteRenegociadasRecursivo(item.ContaFinanceiraId);
                if(item.TipoContaFinanceira == TipoContaFinanceira.ContaPagar)
                {
                    ContaPagarBL.Delete(item.ContaFinanceiraId);
                }
                else
                {
                    ContaReceberBL.Delete(item.ContaFinanceiraId);
                }
            }
        }

        public override void Insert(RollbackFinanceiroCompraVenda entity)
        {
            var observacaoPedido = $"venda nº {entity.NumeroPedido}";
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
                x.ContaFinanceiraParcelaPaiId == (entity.ContaFinanceiraParcelaPaiIdServicos.HasValue ? entity.ContaFinanceiraParcelaPaiIdServicos.Value : default(Guid)
            )).Union(
            ContaPagarBL.All.Where(x =>
                x.Observacao.Contains(observacaoPedido) && x.PessoaId == (entity.TransportadoraId.HasValue ? entity.TransportadoraId.Value : default(Guid))
            )));

            foreach (var contaPagar in contasPagar)
            {
                DeleteRenegociadasRecursivo(contaPagar.Id);
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
                DeleteRenegociadasRecursivo(contaReceber.Id);
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