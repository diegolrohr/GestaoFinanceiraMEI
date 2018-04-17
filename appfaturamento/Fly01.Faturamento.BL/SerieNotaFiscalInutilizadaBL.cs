using System.Linq;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Faturamento.Domain.Enums;
using Fly01.Core.Entities.Domains.Enum;

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
            entity.Fail(All.Any(x => x.Serie.ToUpper() == entity.Serie.ToUpper() && (x.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas || entity.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas || x.TipoOperacaoSerieNotaFiscal == entity.TipoOperacaoSerieNotaFiscal) && x.NumNotaFiscal == entity.NumNotaFiscal && x.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Inutilizada), NumNotaFiscalRepetida);
            //entity.Fail(NotaFiscalBL.All.Any(x => x.NumNotaFiscal == entity.NumNotaFiscal), NumNotaFiscalEmitida);

            base.ValidaModel(entity);
        }

        public static Error NumNotaFiscalRepetida = new Error("Nota Fiscal já inutilizada.", "numNotaFiscal");
        //public static Error NumNotaFiscalEmitida = new Error("Nota Fiscal já emitida, não é possível inutilizar.", "numNotaFiscal");
    }
}