using System;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using Fly01.Core.Notifications;

namespace Fly01.Compras.BL
{
    public class NotaFiscalInutilizadaBL : PlataformaBaseBL<NotaFiscalInutilizada>
    {
        public NotaFiscalInutilizadaBL(AppDataContextBase context) : base(context)
        {
        }

        public IQueryable<NotaFiscalInutilizada> Everything => repository.All.Where(x => x.Ativo);

        public override void Insert(NotaFiscalInutilizada entity)
        {
            entity.Data = DateTime.Now;
            base.Insert(entity);
        }

        public override void ValidaModel(NotaFiscalInutilizada entity)
        {
            entity.Fail(All.Any(x => x.Id != entity.Id && x.Serie.ToUpper() == entity.Serie.ToUpper() && x.NumNotaFiscal == entity.NumNotaFiscal), new Error("Nota Fiscal já inutilizada"));

            base.ValidaModel(entity);
        }
    }
}