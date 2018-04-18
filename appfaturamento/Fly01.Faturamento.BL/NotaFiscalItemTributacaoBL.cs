using Fly01.Core.BL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Faturamento.DAL;
using Fly01.Core.Notifications;
using System.Linq;

namespace Fly01.Faturamento.BL
{
    public class NotaFiscalItemTributacaoBL : PlataformaBaseBL<NotaFiscalItemTributacao>
    {
        public NotaFiscalItemTributacaoBL(AppDataContext context) : base(context)
        {
        }

        public override void ValidaModel(NotaFiscalItemTributacao entity)
        {
            var jaExiste = All.Any(x => x.NotaFiscalItemId == entity.NotaFiscalItemId && x.Id != entity.Id);
            entity.Fail(jaExiste, new Error("Já existe um registro de tributação para este item da nota fiscal"));

            base.ValidaModel(entity);
        }
    }
}