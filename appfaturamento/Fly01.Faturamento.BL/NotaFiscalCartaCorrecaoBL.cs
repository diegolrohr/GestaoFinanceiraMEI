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

            entity.Fail(string.IsNullOrEmpty(entity.MensagemCorrecao), new Error("Informe a mensagem de correção", "mensagemCorrecao"));

            var max = 0;
            var cceValidasAnterioes = All.AsNoTracking().Where(x => x.NotaFiscalId == entity.NotaFiscalId && x.Id != entity.Id && x.Status == StatusCartaCorrecao.RegistradoEVinculado);

            if (cceValidasAnterioes != null && cceValidasAnterioes.Any())
            {
                max = cceValidasAnterioes.Max(x => x.Numero);

                var ultimaMensagem = cceValidasAnterioes.Where(x => x.Numero == max).FirstOrDefault().MensagemCorrecao;

                if (!string.IsNullOrEmpty(entity.MensagemCorrecao))
                    entity.MensagemCorrecao += " " + ultimaMensagem;
                else
                    entity.MensagemCorrecao = ultimaMensagem;
            }

            entity.Fail(!string.IsNullOrEmpty(entity.MensagemCorrecao) && entity.MensagemCorrecao.Length > 1000,
                new Error ("SEFAZ permite até 1000 caracteres por carta de correção. A soma da mensagem atual com as anteriores excedeu 1000 caracteres. A soma possui: " + entity.MensagemCorrecao.Length.ToString() +" caracteres."));
            entity.Fail(cceValidasAnterioes != null && cceValidasAnterioes.Count() >= 20, new Error("Você pode ter no máximo 20 cartas de correções registradas por nota fiscal."));

            base.ValidaModel(entity);
        }

        public override void Delete(NotaFiscalCartaCorrecao entityToDelete)
        {
            var status = entityToDelete.Status;
            entityToDelete.Fail(status == StatusCartaCorrecao.Transmitida || status == StatusCartaCorrecao.RegistradoEVinculado, new Error("Não é possível deletar Carta de Correção com status diferente de Transmitida ou Não Autorizada, Não Transmitida ou Falha na Transmissão", "status"));
            if (entityToDelete.IsValid())
            {
                base.Delete(entityToDelete);
            }
            else
            {
                throw new BusinessException(entityToDelete.Notification.Get());
            }
        }
    }
}
