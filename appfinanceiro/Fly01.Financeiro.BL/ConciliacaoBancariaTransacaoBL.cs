using Fly01.Core.BL;
using System.Linq;
using System;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.BL
{
    public class ConciliacaoBancariaTransacaoBL : EmpresaBaseBL<ConciliacaoBancariaTransacao>
    {
        protected ConciliacaoBancariaItemContaFinanceiraBL conciliacaoBancariaItemContaFinanceiraBL { get; set; }
        protected ContaPagarBL contaPagarBL { get; set; }
        protected ContaReceberBL contaReceberBL { get; set; }
        protected CondicaoParcelamentoBL condicaoParcelamentoBL { get; set; }
        protected ConciliacaoBancariaItemBL conciliacaoBancariaItemBL { get; set; }
        protected ConciliacaoBancariaBL conciliacaoBancariaBL { get; set; }

        public ConciliacaoBancariaTransacaoBL(AppDataContext Context, ConciliacaoBancariaItemContaFinanceiraBL ConciliacaoBancariaItemContaFinanceiraBL, ContaPagarBL ContaPagarBL, ContaReceberBL ContaReceberBL, CondicaoParcelamentoBL CondicaoParcelamentoBL, ConciliacaoBancariaItemBL ConciliacaoBancariaItemBL, ConciliacaoBancariaBL ConciliacaoBancariaBL) : base(Context)
        {
            conciliacaoBancariaItemContaFinanceiraBL = ConciliacaoBancariaItemContaFinanceiraBL;
            contaPagarBL = ContaPagarBL;
            contaReceberBL = ContaReceberBL;
            condicaoParcelamentoBL = CondicaoParcelamentoBL;
            conciliacaoBancariaItemBL = ConciliacaoBancariaItemBL;
            conciliacaoBancariaBL = ConciliacaoBancariaBL;
        }

        public override void Insert(ConciliacaoBancariaTransacao entity)
        {
            try
            {
                entity.EmpresaId = EmpresaId;
                entity.DataInclusao = DateTime.Now;
                entity.DataAlteracao = null;
                entity.DataExclusao = null;
                entity.UsuarioInclusao = AppUser;
                entity.UsuarioAlteracao = null;
                entity.UsuarioExclusao = null;
                entity.Ativo = true;
                entity.Id = Guid.NewGuid();

                CondicaoParcelamento condicaoAVista = condicaoParcelamentoBL.All.Where(x => x.Id == entity.CondicaoParcelamentoId && (x.QtdParcelas == 1 || x.CondicoesParcelamento.Equals("0"))).FirstOrDefault();
                if (condicaoAVista == null)
                    throw new BusinessException("Condição de parcelamento inválida para a transação. Somente a vista.");

                ContaFinanceira contaFinanceira = (entity.TipoContaFinanceira == "ContaPagar") 
                    ? (ContaFinanceira) new ContaPagar()
                    : (ContaFinanceira) new ContaReceber();

                var conciliacaoBancariaId = conciliacaoBancariaItemBL.All.Where(x => x.Id == entity.ConciliacaoBancariaItemId).FirstOrDefault().ConciliacaoBancariaId;

                contaFinanceira.Id = Guid.NewGuid();
                contaFinanceira.ValorPrevisto = entity.ValorPrevisto;
                contaFinanceira.CategoriaId = entity.CategoriaId;
                contaFinanceira.FormaPagamentoId = entity.FormaPagamentoId;
                contaFinanceira.CondicaoParcelamentoId = entity.CondicaoParcelamentoId;
                contaFinanceira.PessoaId = entity.PessoaId;
                contaFinanceira.DataEmissao = DateTime.Now;
                contaFinanceira.DataVencimento = entity.DataVencimento;
                contaFinanceira.Descricao = entity.Descricao;
                contaFinanceira.ContaBancariaId = conciliacaoBancariaBL.All.FirstOrDefault(x => x.Id == conciliacaoBancariaId).ContaBancariaId;
                //a baixa não atualiza a conta, pois ela está referenciada pela navigation
                //a conta não está salva ainda
                contaFinanceira.StatusContaBancaria = StatusContaBancaria.Pago;
                contaFinanceira.ValorPago = entity.ValorPrevisto;

                if (entity.TipoContaFinanceira == "ContaPagar")
                    contaPagarBL.Insert((ContaPagar)contaFinanceira);
                else
                    contaReceberBL.Insert((ContaReceber)contaFinanceira);

                contaFinanceira.ValorPago = null;

                ConciliacaoBancariaItemContaFinanceira CBItemContaFinanceira = new ConciliacaoBancariaItemContaFinanceira()
                {
                    ConciliacaoBancariaItemId = entity.ConciliacaoBancariaItemId,
                    ValorConciliado = entity.ValorConciliado,
                    ContaFinanceira = contaFinanceira //passado via navigation para as demais validações que a esperam
                };
                conciliacaoBancariaItemContaFinanceiraBL.Insert(CBItemContaFinanceira);
                               
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public override void Update(ConciliacaoBancariaTransacao entity)
        {
            throw new BusinessException("Não é possível atualizar este tipo de registro");
        }

        public override void Delete(ConciliacaoBancariaTransacao entity)
        {
            throw new BusinessException("Não é possível deletar este tipo de registro");
        }
    }
}