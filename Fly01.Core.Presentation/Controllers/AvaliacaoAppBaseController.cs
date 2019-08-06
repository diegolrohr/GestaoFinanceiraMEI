using Fly01.Core.Defaults;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class AvaliacaoAppBaseController<T> : BaseController<T> where T : DomainBaseVM
    {
        private string ResourceHashAvalicaoApp { get; set; }

        public AvaliacaoAppBaseController(string resourceHash)
        {
            ResourceHashAvalicaoApp = resourceHash;
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanPerformOperation(ResourceHashAvalicaoApp, EPermissionValue.Write))
                target.Add(new HtmlUIButton { Id = "save", Label = "Enviar", OnClickFn = "fnSalvarAvaliacaoApp", Type = "submit" });

            return target;
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
                    Title = "Avalie o Aplicativo",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "menu", Value = "AvaliaçãoAPP" });
            config.Elements.Add(new InputHiddenUI { Id = "aplicativo" });
            config.Elements.Add(new InputTextUI { Id = "titulo", Class = "col s12 m12 l6", Label = "Seu Nome", MaxLength = 45 });
            config.Elements.Add(new RatingUI { Id = "satisfacao", Class = "col s12 l3", Label = "Dê sua nota para o aplicativo" });
            config.Elements.Add(new TextAreaUI { Id = "descricao", Class = "col s12 m12 24", Label = "Descrição" });

            cfg.Content.Add(config);

            return cfg;
        }

        public override Func<T, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List() => Form();

        [HttpPost]
        public override JsonResult Create(T entityVM)
        {
            try
            {
                var postResponse = RestHelper.ExecutePostRequest(ResourceName, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                T postResult = JsonConvert.DeserializeObject<T>(postResponse);
                var response = new JsonResult
                {
                    Data = new { success = true, message = "Avaliação enviada com sucesso." }
                };
                return (response);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }
    }
}