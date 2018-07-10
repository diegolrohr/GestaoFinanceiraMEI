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

                var mensagens = All.AsNoTracking().Where(x => x.NotaFiscalId == entity.NotaFiscalId && x.Numero == max).FirstOrDefault();

                if (!string.IsNullOrEmpty(entity.MensagemCorrecao))
                    entity.MensagemCorrecao += " " + mensagens.MensagemCorrecao;
                else
                    entity.MensagemCorrecao = mensagens.MensagemCorrecao;
            }

            entity.Numero = ++max;

            entity.Fail(string.IsNullOrEmpty(entity.MensagemCorrecao), new Error("Informe a mensagem de correção", "mensagemCorrecao"));
            entity.Fail(!string.IsNullOrEmpty(entity.MensagemCorrecao) && entity.MensagemCorrecao.Length > 1000,
                new Error ("SEFAZ permite até 1000 caracteres por carta de correção. A soma da mensagem atual com as anteriores excedeu 1000 caracteres. A soma possui: " + entity.MensagemCorrecao.Length.ToString() +" caracteres."));
            entity.Fail(entity.Numero > 20, new Error("Você pode gerar no máximo 20 cartas de correções para uma nota."));

            base.ValidaModel(entity);
        }
    }
}
