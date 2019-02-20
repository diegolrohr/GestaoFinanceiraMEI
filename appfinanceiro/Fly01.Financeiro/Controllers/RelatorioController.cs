using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Financeiro.Controllers
{
    // [OperationRole(ResourceKey = ResourceHashConst.FinanceiroFinanceiroRelatorioDRE)]
    public class RelatorioController : BaseController<DomainBaseVM>
    {
        public override Func<DomainBaseVM, object> GetDisplayData()
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
                UrlFunctions = Url.Action("Functions", "Home", null, Request.Url.Scheme) + "?fns=",
                Functions = new List<string> { "__format", "fnGetSaldos" },
                SidebarUrl = Url.Action("Sidebar", "Home")
            };

           
            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m4",
                Color = "totvs-blue",
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
                Color = "totvs-blue",
                Id = "fly01cardCPCentroCusto",
                Title = "Contas a pagar",
                Placeholder = "",
                Action = new LinkUI
                {
                    Label = "Relatório CP",
                    OnClick = @Url.Action("List", "RelatorioCentroCustoCP")
                }
            });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m4",
                Color = "totvs-blue",
                Id = "fly01cardCRCentroCusto",
                Title = "Contas a receber",
                Placeholder = "",
                Action = new LinkUI
                {
                    Label = "Relatório CR",
                    OnClick = @Url.Action("List", "RelatorioCentroCustoCR")
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