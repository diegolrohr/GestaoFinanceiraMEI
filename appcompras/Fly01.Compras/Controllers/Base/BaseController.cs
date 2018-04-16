using Fly01.Core;
using Fly01.uiJS.Classes;
using Fly01.Compras.Entities.ViewModel;
using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.uiJS.Defaults;
using Fly01.Core.Config;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Presentation;
using Fly01.Core.Rest;

namespace Fly01.Compras.Controllers.Base
{
    public abstract class BaseController<T> : WebBaseController<T> where T : DomainBaseVM
    {
        protected BaseController()
        {
            ResourceName = AppDefaults.GetResourceName(typeof(T));
            APIEnumResourceName = "Fly01.Compras.Domain.Enums.";
            AppViewModelResourceName = "Fly01.Compras.Entities.ViewModel.";
            AppEntitiesResourceName = "Fly01.Compras.Entities";
        }

        public EmpresaVM GetDadosEmpresa()
        {
            return RestHelper.ExecuteGetRequest<EmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{SessionManager.Current.UserData.PlatformUrl}");
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
                        new LabelsetUI()
                        {
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