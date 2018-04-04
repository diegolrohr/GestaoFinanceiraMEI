﻿using Fly01.Core;
using System.Web.Mvc;
using Fly01.Core.VM;
using Fly01.Core.Controllers.Web;
using Fly01.Core.Config;
using System.Collections.Generic;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Controllers.Base
{
    public abstract class BaseController<T> : WebBaseController<T> where T : DomainBaseVM
    {
        protected BaseController()
        {
            ResourceName = AppDefaults.GetResourceName(typeof(T));
            APIEnumResourceName = "Fly01.Financeiro.Domain.Enums.";
            AppViewModelResourceName = "Fly01.Financeiro.Entities.ViewModel.";
            AppEntitiesResourceName = "Fly01.Financeiro.Entities";
        }

        public EmpresaVM GetDadosEmpresa()
        {
            var urlGateway = AppDefaults.UrlGateway
                .Replace("financeiro/", string.Empty)
                .Replace("faturamento/", string.Empty)
                .Replace("estoque/", string.Empty)
                .Replace("compras/", string.Empty);

            return RestHelper.ExecuteGetRequest<EmpresaVM>(urlGateway, $"Empresa/{SessionManager.Current.UserData.PlatformUrl}");
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