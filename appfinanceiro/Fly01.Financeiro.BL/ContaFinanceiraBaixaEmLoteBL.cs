using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;

namespace Fly01.Financeiro.BL
{
    public class ContaFinanceiraBaixaEmLoteBL : PlataformaBaseBL<ContaFinanceiraBaixaEmLote>
    {
        private ContaFinanceiraBaixaBL contaFinanceiraBaixaBL;

        public ContaFinanceiraBaixaEmLoteBL(AppDataContext context, ContaFinanceiraBaixaBL contaFinanceiraBaixaBL) : base(context)
        {
            this.contaFinanceiraBaixaBL = contaFinanceiraBaixaBL;
        }
        
        public override void Insert(ContaFinanceiraBaixaEmLote entity)
        {
            throw new BusinessException("Não é possível inserir baixas múltiplas.");
        }

        public override void Update(ContaFinanceiraBaixaEmLote entity)
        {
            throw new BusinessException("Não é possível alterar baixas múltiplas.");
        }

        public override void Delete(ContaFinanceiraBaixaEmLote entity)
        {
            throw new BusinessException("Não é possível deletar baixas múltiplas.");
        }        
    }
}