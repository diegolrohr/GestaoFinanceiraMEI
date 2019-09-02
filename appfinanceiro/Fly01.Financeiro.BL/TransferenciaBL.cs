using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.BL
{
    public class TransferenciaBL : EmpresaBaseBL<TransferenciaFinanceira>
    {
        protected MovimentacaoBL movimentacaoBL { get; set; }

        public TransferenciaBL(AppDataContext context, MovimentacaoBL movBl) : base(context)
        {
            movimentacaoBL = movBl;
        }

        public override void Insert(TransferenciaFinanceira entity) => movimentacaoBL.NovaTransferencia(entity);

        public override void Update(TransferenciaFinanceira entity)
        {
            throw new BusinessException("Não é possivel realizar a atualização de movimentação.");
        }

        public override void Delete(TransferenciaFinanceira entityToDelete)
        {
            throw new BusinessException("Não é possivel realizar a deleção de movimentação.");
        }
    }
}