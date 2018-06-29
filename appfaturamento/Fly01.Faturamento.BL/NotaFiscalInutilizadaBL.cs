using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;
using System.Data.Entity;

namespace Fly01.Faturamento.BL
{
    public class NotaFiscalInutilizadaBL : PlataformaBaseBL<NotaFiscalInutilizada>
    {
        //protected NotaFiscalBL NotaFiscalBL { get; set; }

        public NotaFiscalInutilizadaBL(AppDataContextBase context) : base(context)
        {
            //NotaFiscalBL = notaFiscalBL;
        }

        public override void ValidaModel(NotaFiscalInutilizada entity)
        {
            //entity.Fail(All.Any(x => 
            //    x.Id != entity.Id &&
            //    x.Serie.ToUpper() == entity.Serie.ToUpper() &&
            //    x.NumNotaFiscal == entity.NumNotaFiscal &&
            //    (x.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas || entity.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas || x.TipoOperacaoSerieNotaFiscal == entity.TipoOperacaoSerieNotaFiscal) &&
            //    x.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Inutilizada
            //), NumNotaFiscalRepetida);

            base.ValidaModel(entity);
        }        
    }
}