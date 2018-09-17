using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Compras.BL
{
    public class SerieNotaFiscalBL : PlataformaBaseBL<SerieNotaFiscal>
    {
        protected NotaFiscalInutilizadaBL NotaFiscalInutilizadaBL { get; set; }

        public SerieNotaFiscalBL(AppDataContextBase context, NotaFiscalInutilizadaBL notaFiscalInutilizadaBL) : base(context)
        {
            MustConsumeMessageServiceBus = true;
            NotaFiscalInutilizadaBL = notaFiscalInutilizadaBL;
        }

        public override void Insert(SerieNotaFiscal entity)
        {
            base.Insert(entity);
        }

        public override void Update(SerieNotaFiscal entity)
        {
            base.Update(entity);
        }

        public override void ValidaModel(SerieNotaFiscal entity)
        {
            entity.Serie.PadLeft(3, '0');
            entity.Fail(All.Any(x => x.Id != entity.Id && x.Serie.ToUpper() == entity.Serie.ToUpper() && (x.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas || entity.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas || x.TipoOperacaoSerieNotaFiscal == entity.TipoOperacaoSerieNotaFiscal)), SerieRepetida);            
            entity.Fail(NotaFiscalInutilizadaBL.All.Any(x => x.Serie.ToUpper() == entity.Serie.ToUpper() && x.NumNotaFiscal == entity.NumNotaFiscal), NumNotaFiscalInutilizada);

            base.ValidaModel(entity);
        }

        public static Error SerieRepetida = new Error("Série de Nota Fiscal já existe.", "serie");
        public static Error NumNotaFiscalInutilizada = new Error("Nota Fiscal Inutilizada, não pode ser a próxima.", "numNotaFiscal");        
    }
}