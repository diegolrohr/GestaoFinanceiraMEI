using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using Fly01.Core.Notifications;

namespace Fly01.Faturamento.BL
{
    public class NotaFiscalInutilizadaBL : PlataformaBaseBL<NotaFiscalInutilizada>
    {
        public NotaFiscalInutilizadaBL(AppDataContextBase context) : base(context)
        {
        }

        public override void ValidaModel(NotaFiscalInutilizada entity)
        {
            entity.Fail(All.Any(x => x.Id != entity.Id && x.Serie.ToUpper() == entity.Serie.ToUpper() && x.NumNotaFiscal == entity.NumNotaFiscal), new Error("Nota Fiscal já inutilizada"));

            base.ValidaModel(entity);
        }
    }
}