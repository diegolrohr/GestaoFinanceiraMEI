using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;
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
    [OperationRole(ResourceKey = ResourceHashConst.OrdemServicoConfiguracoesParametros)]
    public class ParametroOrdemServicoController : BaseController<ParametroOrdemServicoVM>
    {
        public ParametroOrdemServicoController() : base()
        {
            ExpandProperties = "responsavelPadrao($select=id,nome,email)";
        }

        public override ContentResult List()
            => Form();

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public override ContentResult Form() => base.Form();

        public ParametroOrdemServicoVM GetParametro()
        {
            var queryParams = new Dictionary<string, string>
            {
                {  "$expand", ExpandProperties }
            };
            var response = RestHelper.ExecuteGetRequest<ResultBase<ParametroOrdemServicoVM>>(ResourceName, queryParams);

            if (response == null || response.Data == null)
                return null;

            return response.Data.FirstOrDefault();
        }

        /// <summary>
        /// TODO: Checar o motivo da entidade ParametroOrdemServico não retornar a propriedade de navegação do responsavelPadrao
        /// </summary>
        /// <returns></returns>

        public JsonResult CarregaParametro()
        {
            var parametro = GetParametro();

            if (parametro == null)
                return Json(new
                {
                    diasPrazoEntrega = 7,
                    responsavelNome = "",
                    responsavelPadraoId = (Guid?)null
                }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                diasPrazoEntrega = parametro.DiasPrazoEntrega,
                responsavelPadraoNome = ResponsavelPadraoNome(parametro.ResponsavelPadraoId),
                responsavelPadraoId = parametro.ResponsavelPadraoId
            }, JsonRequestBehavior.AllowGet);
        }

        public string ResponsavelPadraoNome(Guid? idResponsvel)
        {
            if (idResponsvel == null || idResponsvel.Value == Guid.Empty) return null;

            var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
            var queryString = AppDefaults.GetQueryStringDefault();

            queryString.AddParam("$filter", $"id eq {idResponsvel.GetValueOrDefault().ToString()}");
            queryString.AddParam("$select", "nome");

            var filterObjects = from item in RestHelper.ExecuteGetRequest<ResultBase<PessoaVM>>(resourceName, queryString).Data
                                select item.Nome;

            return filterObjects.FirstOrDefault();
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
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = { "fnFormReady" },
                SidebarUrl = Url.Action("Sidebar", "Home")
            };

            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                },
                ReadyFn = "fnFormReady"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputNumbersUI { Id = "diasPrazoEntrega", Class = "col s12 m3", Label = "Previsão de Entrega (dias)", Required = true });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "responsavelPadraoId",
                Class = "col s12 m3",
                Label = "Responsável padrão",
                Required = false,
                DataUrl = Url.Action("Vendedor", "AutoComplete"),
                LabelId = "responsavelPadraoNome",
                LabelName = "responsavelPadraoNome"
            }, ResourceHashConst.OrdemServicoCadastrosResponsaveis));

            #region Helpers 
            config.Helpers.Add(new TooltipUI
            {
                Id = "diasPrazoEntrega",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Número de dias sugeridos para a previsão de entrega"
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "responsavelPadraoId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Pessoa que será responsável pela ordem de serviço, por padrão"
                }
            });
            #endregion

            cfg.Content.Add(config);

            return cfg;
        }


        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" });

            return target;
        }

        public override Func<ParametroOrdemServicoVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }
    }
}