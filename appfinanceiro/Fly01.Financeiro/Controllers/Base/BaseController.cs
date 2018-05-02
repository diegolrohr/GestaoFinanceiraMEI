using Fly01.Core;
using System.Web.Mvc;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Config;
using System.Collections.Generic;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using Fly01.Core.Presentation;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;

namespace Fly01.Financeiro.Controllers.Base
{
    public abstract class BaseController<T> : WebBaseController<T> where T : DomainBaseVM
    {
        protected BaseController()
        {
            ResourceName = AppDefaults.GetResourceName(typeof(T));
            AppViewModelResourceName = "Fly01.Financeiro.ViewModel.";
            AppEntitiesResourceName = "Fly01.Financeiro";
        }

        public ManagerEmpresaVM GetDadosEmpresa()
        {
            return ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);
        }

        public ContentResult EmConstrucao(string history)
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory() { Default = history },
                Header = new HtmlUIHeader()
                {
                    Title = "Opção indisponível",
                    Buttons = new List<HtmlUIButton>()
                },
                UrlFunctions = ""
            };


            cfg.Content.Add(new FormUI()
            {
                Elements = new List<BaseUI>()
                    {
                        new LabelsetUI {
                            Class = "col s12",
                            Id = "underconstruction",
                            Name = "underconstruction",
                            Label = "O recurso está em desenvolvimento."
                        }
                    },
                Class = "col s12",

            });

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }
    }
}