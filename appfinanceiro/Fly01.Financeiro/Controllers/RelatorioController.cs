using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Financeiro.Controllers
{
    [AllowAnonymous]
    public class RelatorioController : BaseController<EmpresaBaseVM>
    {
        public override Func<EmpresaBaseVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {   
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Relatórios ",
                },
                UrlFunctions = Url.Action("Functions", "Relatorio", null, Request.Url.Scheme) + "?fns=",
                Functions = new List<string> { "fnFormReady", "__format", "fnGetSaldos"},
                SidebarUrl = Url.Action("Sidebar", "Home"), 
            };

           
            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m4",
                Color = "WhitClass",
                Id = "fly01cardDre",
                Title = "DRE",
                Placeholder = "",
                Action = new LinkUI
                {
                    Label = "Relatório DRE",
                    OnClick = @Url.Action("List", "DemonstrativoResultadoExercicio")
                }
            });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m4",
                Color = "WhitClass",
                Id = "fly01cardCPCentroCusto",
                Title = "Contas a pagar",
                Placeholder = "",
                Action = new LinkUI
                {
                    Label = "Relatório Contas Pagar",
                    OnClick = @Url.Action("List", "RelatorioContaPagar")
                }
            });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m4",
                Color = "WhitClass",
                Id = "fly01cardCRCentroCusto",
                Title = "Contas a receber",
                Placeholder = "",
                Action = new LinkUI
                {
                    Label = "Relatório Contas Receber",
                    OnClick = @Url.Action("List", "RelatorioContaReceber")
                }
            });

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }
    }
}