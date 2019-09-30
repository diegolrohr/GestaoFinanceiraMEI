using Fly01.Financeiro.ViewModel;
using Fly01.Financeiro.Models.Reports;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core;
using Fly01.Core.Config;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Financeiro.Controllers
{
    [AllowAnonymous]
    public class DemonstrativoResultadoExercicioController : BaseController<DemonstrativoResultadoExercicioVM>
    {
        [HttpGet]
        public ActionResult Imprimir(DateTime dataInicial, DateTime dataFinal, bool somaRealizados = true, bool somaPrevistos = false)
        {
            var queryString = new Dictionary<string, string>
            {
                { "dataInicial", dataInicial.ToString("yyyy-MM-dd")},
                { "dataFinal", dataFinal.ToString("yyyy-MM-dd")},
                { "somaRealizados", somaRealizados.ToString()},
                { "somaPrevistos", somaPrevistos.ToString()},
            };

            var movimentacao = RestHelper.ExecuteGetRequest<ResultBase<MovimentacaoPorCategoriaVM>>("MovimentacaoPorCategoria", queryString);
            var total = movimentacao.Data.Where(x => x.CategoriaPaiId == null).Sum(x => x.Soma);
            var totalReceitas = movimentacao.Data.Where(x => x.TipoCarteira == "1" && x.CategoriaPaiId == null).Sum(x => x.Soma);
            var totalDespesas = movimentacao.Data.Where(x => x.TipoCarteira == "2" && x.CategoriaPaiId == null).Sum(x => x.Soma);
            var reportViewer = new WebReportViewer<MovimentacaoPorCategoriaVM>(ReportDre.Instance);
            var parameters = new ReportParameter[]
            {
                new ReportParameter("TotalReceitas", totalReceitas.ToString("C", AppDefaults.CultureInfoDefault)),
                new ReportParameter("TotalDespesas", totalDespesas.ToString("C", AppDefaults.CultureInfoDefault)),
                new ReportParameter("ReportTotal", total.ToString("C", AppDefaults.CultureInfoDefault))
            };

            return File(reportViewer.Print(movimentacao.Data, "", $"Intervalo: {dataInicial:dd/MM/yyyy} a {dataFinal:dd/MM/yyyy}", parameters), "application/pdf");
        }

        public override List<HtmlUIButton> GetListButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

                target.Add(new HtmlUIButton { Id = "save", Label = "Atualizar", OnClickFn = "fnAtualizar" });
                target.Add(new HtmlUIButton { Id = "print", Label = "Imprimir", OnClickFn = "fnImprimirDRE" });

            return target;
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "DRE",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions", "DemonstrativoResultadoExercicio", null, Request.Url.Scheme) + "?fns="
            };

            var dataInicialFiltroDefault = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var dataFinalFiltroDefault = DateTime.Now;

            cfg.Content.Add(new FormUI
            {
                Id = "fly01frm",
                UrlFunctions = Url.Action("Functions", "DemonstrativoResultadoExercicio", null, Request.Url.Scheme) + "?fns=",
                Class = "col s12",
                Elements = new List<BaseUI>
                {
                    new InputDateUI { Id = "dataInicial", Class = "col s6", Label = "Data Inicial", Value = dataInicialFiltroDefault.ToString("dd/MM/yyyy"),
                        DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "change", Function = "fnAtualizar" } }
                    },
                    new InputDateUI { Id = "dataFinal", Class = "col s6", Label = "Data Final",
                        Value = dataFinalFiltroDefault.ToString("dd/MM/yyyy"),
                        DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "change", Function = "fnAtualizar" } }
                    }
                },
                ReadyFn = "fnFormReady"
            });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m4",
                Color = "totvs-blue",
                Id = "fly01cardReceitas",
                Title = "Receitas",
                Placeholder = "R$ 0,00",
                Action = new LinkUI
                {
                    Label = "Contas a receber",
                    OnClick = @Url.Action("List", "ContaReceber")
                }
            });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m4",
                Color = "red",
                Id = "fly01cardDespesas",
                Title = "Despesas",
                Placeholder = "R$ 0,00",
                Action = new LinkUI
                {
                    Label = "Contas a pagar",
                    OnClick = @Url.Action("List", "ContaPagar")
                }
            });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12 m4",
                Color = "green",
                Id = "fly01cardTotal",
                Title = "Total",
                Placeholder = "R$ 0,00",
                Action = new LinkUI
                {
                    Label = "",
                    OnClick = ""
                }
            });

            cfg.Content.Add(new DataTableUI
            {
                Id = "fly01dtreceitas",
                RowGroup = "tipoCarteira",
                Class = "col s12",
                UrlGridLoad = Url.Action("LoadReceitasPorCategoria", "ReceitaPorCategoria"),
                UrlFunctions = Url.Action("Functions", "DemonstrativoResultadoExercicio", null, Request.Url?.Scheme) + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "dataInicial" },
                    new DataTableUIParameter { Id = "dataFinal" }
                },
                Columns = new List<DataTableUIColumn>
                {
                    new DataTableUIColumn
                    {
                        DataField = "categoria",
                        DisplayName = "",
                        Priority = 1,
                        RenderFn = "fnRenderGroup",
                        Searchable = false,
                        Orderable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "soma",
                        DisplayName = "",
                        Priority = 3,
                        RenderFn = "fnRenderSoma",
                        Orderable = false,
                        Searchable = false
                    }
                },
                Options = new DataTableUIConfig { PageLength = 30, WithoutRowMenu = true }
            });

            cfg.Content.Add(new DataTableUI
            {
                Id = "fly01dtdespesas",
                RowGroup = "tipoCarteira",
                Class = "col s12",
                UrlGridLoad = Url.Action("LoadDespesasPorCategoria", "DespesaPorCategoria"),
                UrlFunctions = Url.Action("Functions", "DemonstrativoResultadoExercicio", null, Request.Url?.Scheme) + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "dataInicial" },
                    new DataTableUIParameter { Id = "dataFinal" }
                },
                Columns = new List<DataTableUIColumn>
                {
                    new DataTableUIColumn
                    {
                        DataField = "categoria",
                        DisplayName = "",
                        Priority = 1,
                        RenderFn = "fnRenderGroup",
                        Searchable = false,
                        Orderable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "soma",
                        DisplayName = "",
                        Priority = 3,
                        RenderFn = "fnRenderSoma",
                        Orderable = false,
                        Searchable = false
                    }
                },
                Options = new DataTableUIConfig { PageLength = 30, WithoutRowMenu = true }
            });

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public JsonResult Total(DateTime dataInicial, DateTime dataFinal)
        {
            var queryString = new Dictionary<string, string>
            {
                { "dataInicial", dataInicial.ToString("yyyy-MM-dd")},
                { "dataFinal", dataFinal.ToString("yyyy-MM-dd")},
                { "$select", "soma" }
            };

            var total = RestHelper
                .ExecuteGetRequest<ResultBase<DespesaPorCategoriaVM>>
                    ("MovimentacaoPorCategoria", queryString)
                .Data
                .Where(x => x.CategoriaPaiId == null)
                .Sum(x => x.Soma)
                .ToString("C", AppDefaults.CultureInfoDefault);

            return Json(new { total }, JsonRequestBehavior.AllowGet);
        }

        protected override ContentUI FormJson() { throw new NotImplementedException(); }

        public override Func<DemonstrativoResultadoExercicioVM, object> GetDisplayData() { throw new NotImplementedException(); }
    }
}