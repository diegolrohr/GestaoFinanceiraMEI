using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class KitItemBaseController<T> : BaseController<T> where T : KitItemVM
    {
        public KitItemBaseController()
        {
            ExpandProperties = "produto($select=descricao,codigoProduto),servico($select=descricao,codigoServico)";
        }

        public override Func<T, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                produtoServicoCodigo = x.ProdutoServicoCodigo,
                produtoServicoDescricao = x.ProdutoServicoDescricao,
                quantidade = x.Quantidade.ToString("N", AppDefaults.CultureInfoDefault),
                tipoItem = x.TipoItem,
                tipoItemDescription = EnumHelper.GetDescription(typeof(TipoItem), x.TipoItem),
                tipoItemCssClass = EnumHelper.GetCSS(typeof(TipoItem), x.TipoItem),
                tipoItemValue = EnumHelper.GetValue(typeof(TipoItem), x.TipoItem),
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