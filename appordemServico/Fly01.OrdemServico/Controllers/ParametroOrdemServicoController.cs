using Fly01.Core;
using Fly01.Core.Presentation;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.OrdemServico.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.OrdemServico.Controllers
{
    public class ParametroOrdemServicoController : BaseController<ParametroOrdemServicoVM>
    {

        public override ContentResult List()
            => Form();

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public override ContentResult Form() => base.Form();

        public ParametroOrdemServicoVM GetParametro()
        {
            var response = RestHelper.ExecuteGetRequest<ResultBase<ParametroOrdemServicoVM>>(ResourceName);

            if (response == null || response.Data == null)
                return null;

            return response.Data.FirstOrDefault();
        }

        public JsonResult CarregaParametro()
        {
            var parametro = GetParametro();

            if (parametro == null)
                return Json(new
                {
                    diasPrazoEntrega = 0
                }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                diasPrazoEntrega = parametro.DiasPrazoEntrega
            }, JsonRequestBehavior.AllowGet);
        }

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Parâmetros",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="

            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                }
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputNumbersUI { Id = "diasPrazoEntrega", Class = "col s12 m3", Label = "Previsão de Entrega (dias)", Required = true });

            #region Helpers 
            config.Helpers.Add(new TooltipUI
            {
                Id = "diasPrazoEntrega",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Número de dias sugeridos para a previsão de entrega"
                }
            });
            #endregion

            cfg.Content.Add(config);

            return cfg;
        }


        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            //if (UserCanWrite)
            target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" });

            return target;
        }

        public override Func<ParametroOrdemServicoVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }
    }
}