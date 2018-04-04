using Fly01.Financeiro.API.Models.DAL;
using Fly01.Financeiro.Domain.Entities;
using Fly01.Core.Api.BL;
using Fly01.Core.ValueObjects;

namespace Fly01.Financeiro.BL
{
    public class TransferenciaBL : PlataformaBaseBL<Transferencia>
    {
        protected MovimentacaoBL movimentacaoBL { get; set; }

        public TransferenciaBL(AppDataContext context, MovimentacaoBL movBl) : base(context)
        {
            this.movimentacaoBL = movBl;
        }

        public override void Insert(Transferencia entity)
        {
            movimentacaoBL.NovaTransferencia(entity);
        }

        public override void Update(Transferencia entity)
        {
            throw new BusinessException("Não é possivel realizar a atualização de movimentação.");
        }

        public override void Delete(Transferencia entityToDelete)
        {
            throw new BusinessException("Não é possivel realizar a deleção de movimentação.");
        }
    }
}
