using Fly01.Core;
using System.Linq;
using Fly01.Core.Api.BL;
using Fly01.Core.Notifications;
using Fly01.Faturamento.Domain.Entities;

namespace Fly01.Faturamento.BL
{
    public class GrupoProdutoBL : PlataformaBaseBL<GrupoProduto>
    {
        public GrupoProdutoBL(AppDataContextBase context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(GrupoProduto entity)
        {
            entity.Fail(All.Any(x => x.Descricao.ToUpper() == entity.Descricao.ToUpper() && x.Id != entity.Id), DescricaoEmBranco);

            base.ValidaModel(entity);
        }

        public static Error DescricaoEmBranco = new Error("Descrição já utilizada anteriormente.", "descricao");
    }
}