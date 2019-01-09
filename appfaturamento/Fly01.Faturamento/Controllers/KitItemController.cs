using Fly01.uiJS.Classes;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Faturamento.Controllers
{
    public class KitItemController : BaseController<KitItemVM>
    {
        public KitItemController()
        {
            ExpandProperties = "produto($select=descricao,codigoProduto,valorCusto,saldoProduto,unidadeMedidaId),produto($expand=unidadeMedida)";
        }

        public override Func<KitItemVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                produtoId = x.ProdutoId,
            };
        }

        public virtual JsonResult GridLoadKitItem(string id)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "kitId eq", string.IsNullOrEmpty(id) ? new Guid().ToString() : id }
            };
            return GridLoad(filters);
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }
    }
}