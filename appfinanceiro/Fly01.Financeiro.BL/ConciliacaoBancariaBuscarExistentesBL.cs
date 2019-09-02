using Fly01.Core.BL;
using System;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;

namespace Fly01.Financeiro.BL
{
    public class ConciliacaoBancariaBuscarExistentesBL : EmpresaBaseBL<ConciliacaoBancariaItem>
    {
        protected ConciliacaoBancariaItemContaFinanceiraBL conciliacaoBancariaItemContaFinanceiraBL { get; set; }

        public ConciliacaoBancariaBuscarExistentesBL(AppDataContext Context, ConciliacaoBancariaItemContaFinanceiraBL ConciliacaoBancariaItemContaFinanceiraBL) : base(Context)
        {
            conciliacaoBancariaItemContaFinanceiraBL = ConciliacaoBancariaItemContaFinanceiraBL;            
        }
                
        public override void Insert(ConciliacaoBancariaItem entity)
        {
            try
            {
                conciliacaoBancariaItemContaFinanceiraBL.SalvarConciliacaoBuscarExistentes(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public override void Update(ConciliacaoBancariaItem entity)
        {
            throw new BusinessException("Não é possível atualizar este tipo de registro");
        }

        public override void Delete(ConciliacaoBancariaItem entity)
        {
            throw new BusinessException("Não é possível deletar este tipo de registro");
        }
    }
}