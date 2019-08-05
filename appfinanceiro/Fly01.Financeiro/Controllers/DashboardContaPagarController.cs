using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroDashboardContasPagar)]
    public class DashboardContaPagarController : DashboardContaFinanceiraController
    {
        public DashboardContaPagarController(): base("ContaPagar")
        {
            
        }
        protected override ContentUI DashboardJson(UrlHelper url, string scheme)
        {
            var dJson = base.DashboardJson(url, scheme);

            // Grid
            dJson.Content.Add(new DivUI
            {
                Id = "divLabel",
                Elements = new List<BaseUI> {
                    new LabelSetUI {
                        Id = "titleLabel",
                        Class = "col s12",
                        Label = "Contas a Pagar - Dia"
                    }
                }
            });
            dJson.Content.Add(new DataTableUI
            {
                Id = "dtDashCF",
                UrlGridLoad = url.Action("DashboardGridLoad"),
                Parameters = new List<DataTableUIParameter>
                    {
                        new DataTableUIParameter { Id = "dataInicial", Required = true }
                    },
                Options = new DataTableUIConfig()
                {
                    PageLength = 10,
                    WithoutRowMenu = true
                },
                UrlFunctions = url.Action("Functions") + " ?fns=",
                Columns = new List<DataTableUIColumn> {
                    new DataTableUIColumn
                    {
                        DataField = "descricao",
                        DisplayName = "Descrição",
                        Priority = 1,
                        Orderable = false,
                        Searchable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "vencimento",
                        DisplayName = "Vencimento",
                        Priority = 2,
                        Orderable = false,
                        Searchable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "status",
                        DisplayName = "Status",
                        Priority = 3,
                        Orderable = false,
                        Searchable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "valor",
                        DisplayName = "Valor",
                        Priority = 4,
                        Type = "currency",
                        Orderable = false,
                        Searchable = false
                    }
                }
            });
            return dJson;
        }

        public JsonResult DashboardGridLoad(DateTime? dataInicial = null)
        {
            try
            {
                var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);

                var pageNo = param.Start > 0 ? (param.Start / 10) + 1 : 1;

                Dictionary<string, string> queryString = new Dictionary<string, string>
                    {
                        { "filtro", dataInicial.HasValue? dataInicial.Value.ToString("yyyy-MM-dd"):DateTime.Now.ToString("yyyy-MM-dd") },
                        { "pageNo", pageNo.ToString() },
                        { "pageSize", "10" }
                    };

                var response = RestHelper.ExecuteGetRequest<PagedResult<ContasPagarDoDiaVM>>("dashboardcontapagardia", queryString);

                return Json(new
                {
                    totalRecords = response.Paging.TotalRecordCount,
                    recordsFiltered = response.Paging.TotalRecordCount,
                    data = response.Data.Select(x => new
                    {
                        vencimento = x.Vencimento.ToString("dd/MM/yyyy"),
                        descricao = x.Descricao,
                        valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                        status = x.Status
                    })
                }, JsonRequestBehavior.AllowGet);
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
