using Fly01.Core.Presentation;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    [AllowAnonymous]
    public class DashboardContaReceberController : DashboardContaFinanceiraController
    {
        public DashboardContaReceberController() : base("ContaReceber")
        {

        }

        protected override ContentUI DashboardJson(UrlHelper url, string scheme)
        {
            var dJson = base.DashboardJson(url, scheme);

            dJson.Content.Add(new DivUI
            {
                Id = "divLabel",
                Elements = new List<BaseUI> {
                    new LabelSetUI {
                        Id = "titleLabel",
                        Class = "col s12",
                        Label = "Valores Recebidos"
                    }
                }
            });

            // CHART Dia/Dia
            dJson.Content.Add(new ChartUI
            {
                Id = "chartSaldoDiaDia",
                DrawType = ChartUIType.Bar,
                Options = new
                {                    
                    tooltips = new
                    {
                        mode = "label",
                        bodySpacing = 10,
                        cornerRadius = 0,
                        titleMarginBottom = 15
                    }
                },
                UrlData = @url.Action("LoadChartSaldoDia"),
                Class = "col s12",
                Parameters = new List<ChartUIParameter>
                {
                    new ChartUIParameter { Id = "dataInicial" }
                }
            });

            return dJson;
        }

        public JsonResult LoadChartSaldoDia(DateTime dataInicial)
        {
            var response = GetProjecaoSaldoDia(dataInicial);
            return Json(new
            {
                success = true,
                currency = true,
                labels = response.Select(x => x.Tipo).ToArray(),
                datasets = new object[]
                {
                    new
                    {
                        data = response.Select(x => Math.Round(x.Total, 2)).ToArray(),
                        backgroundColor = "rgba(250, 166, 52, 0.9)",                        
                        borderWidth = 1
                    }
                }
            }, JsonRequestBehavior.AllowGet);
        }

        private List<DashboardContaFinanceiraVM> GetProjecaoSaldoDia(DateTime dataInicial)
        {
            Dictionary<string, string> queryString = new Dictionary<string, string>
            {
                { "filtro", dataInicial.ToString("yyyy-MM-dd") }
            };

            var response = RestHelper.ExecuteGetRequest<List<DashboardContaFinanceiraVM>>("dashboardsaldodia", queryString);
            if (response == null)
                return new List<DashboardContaFinanceiraVM>();

            return response;
        }
    }
}