using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;
using System.Data.Entity;

namespace Fly01.Faturamento.BL
{
    public class SerieNotaFiscalInutilizadaBL : PlataformaBaseBL<SerieNotaFiscal>
    {
        //protected NotaFiscalBL NotaFiscalBL { get; set; }

        public SerieNotaFiscalInutilizadaBL(AppDataContextBase context) : base(context)
        {
            //NotaFiscalBL = notaFiscalBL;
        }

        public override void Insert(SerieNotaFiscal entity)
        {
            entity.StatusSerieNotaFiscal = StatusSerieNotaFiscal.Inutilizada;//Inutilizada

            base.Insert(entity);
        }

        public override void ValidaModel(SerieNotaFiscal entity)
        {
            entity.Fail(All.Any(x => 
                x.Id != entity.Id &&
                x.Serie.ToUpper() == entity.Serie.ToUpper() &&
                x.NumNotaFiscal == entity.NumNotaFiscal &&
                (x.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas || entity.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas || x.TipoOperacaoSerieNotaFiscal == entity.TipoOperacaoSerieNotaFiscal) &&
                x.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Inutilizada
            ), NumNotaFiscalRepetida);

            base.ValidaModel(entity);
        }

        public static Error NumNotaFiscalRepetida = new Error("Nota Fiscal já inutilizada.", "numNotaFiscal");
    }
}