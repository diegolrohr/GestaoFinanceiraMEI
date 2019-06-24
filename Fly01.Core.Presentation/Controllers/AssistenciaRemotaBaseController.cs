﻿using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class AssistenciaRemotaBaseController : BaseController<DomainBaseVM>
    {

        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Assistência Remota",
                },
                UrlFunctions = Url.Action("Functions", "AssistenciaRemota", null, Request.Url.Scheme) + "?fns=",
                SidebarUrl = @Url.Action("Sidebar", "Home"),
            };

            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {

                },
                ReadyFn = "fnAssistenciaRemotaFormReady",
                UrlFunctions = Url.Action("Functions", "AssistenciaRemota", null, Request.Url.Scheme) + "?fns="
            };

            cfg.Content.Add(new CardUI()
            {
                Class = "col s12",
                Color = "blue",
                Id = "instaladorTeamViewer",
                Title = "Instalador do TeamViewer v.13",
                Action = new LinkUI()
                {
                    Label = "Clique Aqui para Baixar o Instalador"
                }
            });

            cfg.Content.Add(config);
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override Func<DomainBaseVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }
    }
}