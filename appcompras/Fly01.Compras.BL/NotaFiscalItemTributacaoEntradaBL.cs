using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Compras.DAL;
using Fly01.Core.Notifications;
using System.Linq;

namespace Fly01.Compras.BL
{
    public class NotaFiscalItemTributacaoEntradaBL : PlataformaBaseBL<NotaFiscalItemTributacaoEntrada>
    {
        public NotaFiscalItemTributacaoEntradaBL(AppDataContext context) : base(context)
        {
        }

        public override void ValidaModel(NotaFiscalItemTributacaoEntrada entity)
        {
            var jaExiste = All.Any(x => x.NotaFiscalItemEntradaId == entity.NotaFiscalItemEntradaId && x.Id != entity.Id);
            entity.Fail(jaExiste, new Error("Já existe um registro de tributação para este item da nota fiscal"));

            base.ValidaModel(entity);
        }
    }
}