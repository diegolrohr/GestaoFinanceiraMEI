using Fly01.Core.BL;
using System.Linq;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.BL
{
    public class FormaPagamentoBL : PlataformaBaseBL<FormaPagamento>
    {
        public FormaPagamentoBL(AppDataContextBase context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(FormaPagamento entity)
        {
            entity.Fail(All.Any(x => x.Ativo && x.Id != entity.Id && x.Descricao.ToUpper() == entity.Descricao.ToUpper() && x.TipoFormaPagamento == entity.TipoFormaPagamento), 
                new Error("Descrição da forma de pagamento já utilizada anteriormente.", "descricao", All.FirstOrDefault(x => x.Id != entity.Id && x.Descricao.ToUpper() == entity.Descricao.ToUpper() && x.TipoFormaPagamento == entity.TipoFormaPagamento)?.Id.ToString()));

            base.ValidaModel(entity);
        }
    }
}
