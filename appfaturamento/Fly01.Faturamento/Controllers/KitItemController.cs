using Fly01.uiJS.Classes;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core;

namespace Fly01.Faturamento.Controllers
{
    public class KitItemController : BaseController<KitItemVM>
    {
        public KitItemController()
        {
            ExpandProperties = "produto($select=descricao),servico($select=descricao)";
        }

        public override Func<KitItemVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                produtoServicoDescricao = x.ProdutoServicoDescricao,
                quantidade = x.Quantidade.ToString("N", AppDefaults.CultureInfoDefault),
                tipoItem = x.TipoItem,
                tipoItemDescription = EnumHelper.GetDescription(typeof(TipoItem), x.TipoItem),
                tipoItemCssClass = EnumHelper.GetCSS(typeof(TipoOrdemVenda), x.TipoItem),
                tipoItemValue = EnumHelper.GetValue(typeof(TipoOrdemVenda), x.TipoItem),
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