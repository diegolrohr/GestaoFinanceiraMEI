using Fly01.Compras.Domain.Entities;
using Fly01.Core.BL;
using System.Linq;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.BL
{
    public class FormaPagamentoBL : PlataformaBaseBL<FormaPagamento>
    {
        private static Error FormaDePagamentoJaCadastradaErr = new Error("Descrição já utilizada anteriormente.", "descricao");

        public FormaPagamentoBL(AppDataContextBase context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(FormaPagamento entity)
        {
            entity.Fail(All.Any(e => e.Ativo && e.Id != entity.Id && (e.Descricao.ToUpper() == entity.Descricao.ToUpper() && e.TipoFormaPagamento == entity.TipoFormaPagamento)), FormaDePagamentoJaCadastradaErr);

            base.ValidaModel(entity);
        }
    }
}
