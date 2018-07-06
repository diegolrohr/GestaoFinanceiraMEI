using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using System;
using System.Linq;

namespace Fly01.Faturamento.BL
{
    public class NotaFiscalCartaCorrecaoBL : PlataformaBaseBL<NotaFiscalCartaCorrecao>
    {

        protected NotaFiscalBL NotaFiscalBL { get; set; }
        public NotaFiscalCartaCorrecaoBL(AppDataContextBase context, NotaFiscalBL notaFiscalBL) : base(context)
        {
            NotaFiscalBL = notaFiscalBL;
        }
        public override void ValidaModel(NotaFiscalCartaCorrecao entity)
        {
            entity.Data = DateTime.Now;

            base.ValidaModel(entity);
        }
    }
}
