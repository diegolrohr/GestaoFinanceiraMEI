using Fly01.Core.Presentation;
using Fly01.Financeiro.ViewModel;
using System;
using Fly01.uiJS.Classes;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using System.Collections.Generic;
using Newtonsoft.Json;
using Fly01.Core.Defaults;

namespace Fly01.Financeiro.Controllers
{
    public class StoneController : BaseController<StoneVM>
    {
        public override Func<StoneVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
            => FormModal();


        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        public ContentResult FormModal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Login STONE",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Acessar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                Id = "fly01mdlfrmOrdemVendaProduto",
                //ReadyFn = "fnFormReadyOrdemVendaProduto",
                //Functions = new List<string>() { "fnChangeTotalProduto" }
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputTextUI { Id = "email", Class = "col s6 m3 l4", Label = "Email Stone" });

            config.Elements.Add(new InputTextUI { Id = "senha", Class = "col s6 m3 l4", Label = "Senha Stone" });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }
}