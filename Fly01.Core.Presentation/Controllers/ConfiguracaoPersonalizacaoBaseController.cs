using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class ConfiguracaoPersonalizacaoBaseController<T> : BaseController<T> where T : ConfiguracaoPersonalizacaoVM
    {
        public override Func<T, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public override ActionResult Index()
        {
            return View("Index");
        }

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Personalizar Sistema",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton {Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit"}
                    }
                },
                UrlFunctions = Url.Action("Functions", "ConfiguracaoPersonalizacao", null, Request.Url.Scheme) + "?fns=",
            };

            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = Url.Action("Edit"),
                    Edit = Url.Action("Edit"),
                    Get = Url.Action("Json") + "/",
                    List = Url.Action("Form")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions", "ConfiguracaoPersonalizacao", null, Request.Url.Scheme) + "?fns=",
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputCheckboxUI { Id = "emiteNotaFiscal", Class = "col s12 m6 l3", Label = "Emite NotaFiscal" });
            config.Elements.Add(new InputCheckboxUI { Id = "exibirStepProdutosVendas", Class = "col s12 m6 l3", Label = "exibirStepProdutosVendas" });
            config.Elements.Add(new InputCheckboxUI { Id = "exibirStepProdutosCompras", Class = "col s12 m6 l3", Label = "exibirStepProdutosCompras" });
            config.Elements.Add(new InputCheckboxUI { Id = "exibirStepServicosVendas", Class = "col s12 m6 l3", Label = "exibirStepServicosVendas" });
            config.Elements.Add(new InputCheckboxUI { Id = "exibirStepServicosCompras", Class = "col s12 m6 l3", Label = "exibirStepServicosCompras" });
            config.Elements.Add(new InputCheckboxUI { Id = "exibirStepTransportadoraVendas", Class = "col s12 m6 l3", Label = "exibirStepTransportadoraVendas" });
            config.Elements.Add(new InputCheckboxUI { Id = "exibirStepTransportadoraCompras", Class = "col s12 m6 l3", Label = "exibirStepTransportadoraCompras" });

            cfg.Content.Add(config);

            return cfg;
        }

        [HttpPost]
        public override JsonResult Edit(T entityVM)
        {
            if(entityVM?.Id != null && entityVM?.Id != default(Guid))
            {
                return base.Edit(entityVM);
            }
            else
            {
                return base.Create(entityVM);
            }
        }

        public override ContentResult Json(Guid id)
        {
            try
            {
                var entity = RestHelper.ExecuteGetRequest<ResultBase<ConfiguracaoPersonalizacaoVM>>("ConfiguracaoPersonalizacao", queryString: null)?.Data?.FirstOrDefault();

                entity = (entity != null && entity.Id != default(Guid))
                    ? entity
                    : new ConfiguracaoPersonalizacaoVM()
                    {
                        EmiteNotaFiscal  = true,
                        ExibirStepProdutosCompras = true,
                        ExibirStepProdutosVendas = true,
                        ExibirStepServicosCompras = true,
                        ExibirStepServicosVendas = true,
                        ExibirStepTransportadoraCompras = true,
                        ExibirStepTransportadoraVendas = true,
                        Id = default(Guid)
                    };
                return Content(JsonConvert.SerializeObject(entity, JsonSerializerSetting.Front), "application/json");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return Content(JsonConvert.SerializeObject(JsonResponseStatus.GetFailure(error.Message).Data), "application/json");
            }
        }
    }
}