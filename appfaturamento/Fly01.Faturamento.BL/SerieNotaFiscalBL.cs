using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Faturamento.BL
{
    public class SerieNotaFiscalBL : PlataformaBaseBL<SerieNotaFiscal>
    {
        //protected NotaFiscalBL NotaFiscalBL { get; set; }

        public SerieNotaFiscalBL(AppDataContextBase context) : base(context)
        {
            //NotaFiscalBL = notaFiscalBL;
        }

        public override void Insert(SerieNotaFiscal entity)
        {
            entity.StatusSerieNotaFiscal = StatusSerieNotaFiscal.Habilitada;//Inutilizada
            base.Insert(entity);
        }

        public override void Update(SerieNotaFiscal entity)
        {
            entity.StatusSerieNotaFiscal = StatusSerieNotaFiscal.Habilitada;//Inutilizada
            base.Update(entity);
        }

        public override void ValidaModel(SerieNotaFiscal entity)
        {
            entity.Fail(All.Any(x => x.Id != entity.Id && x.Serie.ToUpper() == entity.Serie.ToUpper() && (x.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas || entity.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas || x.TipoOperacaoSerieNotaFiscal == entity.TipoOperacaoSerieNotaFiscal) && x.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Habilitada), SerieRepetida);
            //entity.Fail(All.Any(x => x.Id != entity.Id && x.Serie.ToUpper() == entity.Serie.ToUpper() && entity.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas && x.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Habilitada), SerieRepetida);
            //entity.Fail(All.Any(x => x.Id != entity.Id && x.Serie.ToUpper() == entity.Serie.ToUpper() && x.NumNotaFiscal == entity.NumNotaFiscal && (entity.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas || x.TipoOperacaoSerieNotaFiscal == entity.TipoOperacaoSerieNotaFiscal) && entity.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Habilitada), SerieRepetida);
            entity.Fail(All.Any(x => x.Id != entity.Id && x.Serie.ToUpper() == entity.Serie.ToUpper() && x.NumNotaFiscal == entity.NumNotaFiscal && x.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Inutilizada), NumNotaFiscalInutilizada);
            //entity.Fail(NotaFiscalBL.All.Any(x => x.SerieNotaFiscalId == entity.Id) && All.Any(y => y.Id == entity.Id && y.Serie.ToUpper() != entity.Serie.ToUpper()), SerieEmitida);


            //Testar Edicao, se já há uma nota emitida com a Serie, não permitir

            base.ValidaModel(entity);
        }

        public static Error SerieRepetida = new Error("Série de Nota Fiscal já existe.", "serie");
        public static Error NumNotaFiscalInutilizada = new Error("Nota Fiscal Inutilizada, não pode ser a próxima.", "numNotaFiscal");
        //public static Error SerieEmitida = new Error("Já há emissão de Nota Fiscal com essa Série, não pode ser editada.", "serie");
    }
}