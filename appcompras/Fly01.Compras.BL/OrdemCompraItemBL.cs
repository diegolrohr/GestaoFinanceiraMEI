using Fly01.Compras.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;

namespace Fly01.Compras.BL
{
    public class OrdemCompraItemBL : PlataformaBaseBL<OrdemCompraItem>
    {
        public OrdemCompraItemBL(AppDataContext context) : base(context)
        {
        }

        public override void Insert(OrdemCompraItem entity)
        {
            entity.Fail(true, new Error("Não é possível inserir, somente em orçamento ou pedido"));
        }

        public override void Update(OrdemCompraItem entity)
        {
            entity.Fail(true, new Error("Não é possível atualizar, somente em orçamento ou pedido"));
        }

        public override void Delete(OrdemCompraItem entity)
        {
            entity.Fail(true, new Error("Não é possível deletar, somente em orçamento ou pedido"));
        }
    }
}