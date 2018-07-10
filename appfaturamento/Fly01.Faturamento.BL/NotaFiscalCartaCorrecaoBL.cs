using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using System;
using System.Data.Entity;
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
            entity.Status = StatusCartaCorrecao.Transmitida;

            var max = 0;
            if (All.Any(x => x.NotaFiscalId == entity.NotaFiscalId))
            {
                max = All.Max(x => x.Numero);
                var correcao = "";
                var mensagens = All.AsNoTracking().Where(x => x.NotaFiscalId == entity.NotaFiscalId).ToList();

                foreach (var item in mensagens)
                {
                    correcao += " " + item.MensagemCorrecao;
                }
                entity.MensagemCorrecao += " " + correcao;
            }

            entity.Numero = ++max;

            entity.Fail(entity.Numero > 20, new Error("Você pode gerar no máximo 20 cartas de correções para uma nota."));

            base.ValidaModel(entity);
        }
    }
}
