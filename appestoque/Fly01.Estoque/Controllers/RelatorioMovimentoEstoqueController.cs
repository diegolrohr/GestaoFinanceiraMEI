using Fly01.Core;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Estoque.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Estoque.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.EstoqueEstoqueRelatorios)]
    public class RelatorioMovimentoEstoqueController : BaseController<RelatorioMovimentoEstoqueVM>
    {
        public override Func<RelatorioMovimentoEstoqueVM, object> GetDisplayData()
        {
            return x => new
            {
                data = x.Data.ToString("dd/MM/yyyy HH:mm:ss"),
                observacao = x.Observacao,
                quantidade = x.Quantidade.ToString("R", AppDefaults.CultureInfoDefault),
                saldoAntesMovimento = x.SaldoAntesMovimento.ToString("R", AppDefaults.CultureInfoDefault),
                tipoMovimentoDescricao = x.TipoMovimentoDescricao,
                produtoDescricao = x.ProdutoDescricao,
                inventarioDescricao = x.InventarioDescricao
            };
        }

        public override ContentResult List()
        {
            return Content(JsonConvert.SerializeObject(FormJson(), JsonSerializerSetting.Front), "application/json");
        }

        protected override ContentUI FormJson()
        {
            if (!UserCanRead)
            {
                return new ContentUIBase(Url.Action("Sidebar", "Home"));
            }

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Movimentações de Estoque",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
            };

            var config = new FormUI
            {
                Id = "fly01frmRelatorioMovimentoEstoque",
                Action = new FormUIAction
                {
                    //Create = @Url.Action("Create"),
                    //List = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputDateUI
            {
                Id = "dataInicial",
                Class = "col s12 m6 l3",
                Label = "Data Inicial",
                Value = DateTime.Now.ToString("dd/MM/yyyy"),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });
            config.Elements.Add(new InputDateUI
            {
                Id = "dataFinal",
                Class = "col s12 m6 l3",
                Label = "Data Final",
                Value = DateTime.Now.ToString("dd/MM/yyyy"),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "tipoMovimentoId",
                Class = "col s12 m6",
                Label = "Tipo Movimento",
                DataUrl = Url.Action("TiposMovimentos", "AutoComplete"),
                LabelId = "TipoMovimentoDescricao"
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "produtoId",
                Class = "col s12 m6",
                Label = "Produto",
                DataUrl = Url.Action("Produto", "AutoComplete"),
                LabelId = "produtoDescricao"
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "inventarioId",
                Class = "col s12 m6",
                Label = "Inventário",
                DataUrl = Url.Action("Inventario", "AutoComplete"),
                LabelId = "InventarioDescricao"
            });

            config.Elements.Add(new DivElementUI { Id = "movimentacoesEstoque", Class = "col s12 visible" });

            config.Helpers.Add(new TooltipUI
            {
                Id = "dataInicial",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Exibido o limite de até 2000 registros do resultado da busca. Se necessário aprimorar o resultado, informe todos os parâmetros possíveis."
                }
            });

            cfg.Content.Add(config);
            cfg.Content.Add(GetDtProdutosPendenciasCfg());
            return cfg;
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            target.Add(new HtmlUIButton { Id = "imprimirRelatorioMovimentoEstoque", Label = "Imprimir", OnClickFn = "fnImprimirRelatorioMovimentoEstoque" });

            return target;
        }

        public JsonResult GetMovimentacoesEstoque(DateTime? dataInicial, DateTime? dataFinal, Guid? produtoId, Guid? tipoMovimentoId, Guid? inventarioId)
        {
            try
            {
                //var length = 50;
                //var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);
                //var pageNo = param.Start > 0 ? (param.Start / length) + 1 : 1;

                var queryString = new Dictionary<string, string>
                {
                    { "dataInicial", dataInicial != null ? dataInicial.Value.ToString("yyyy-MM-dd") : null},
                    { "dataFinal", dataFinal != null ? dataFinal.Value.ToString("yyyy-MM-dd") : null},
                    { "produtoId", produtoId?.ToString()},
                    { "tipoMovimentoId", tipoMovimentoId?.ToString()},
                    { "inventarioId", inventarioId?.ToString()},
                    //{ "pageNo", pageNo.ToString() },
                    //{ "pageSize", length.ToString() }
                };

                var response = RestHelper.ExecuteGetRequest<ResultBase<RelatorioMovimentoEstoqueVM>>("movimentoestoque", queryString);
                if (response == null)
                    return Json(new { }, JsonRequestBehavior.AllowGet);

                return Json(new
                {
                    recordsTotal = response?.Data.Count(),
                    recordsFiltered = response?.Data.Count(),
                    data = response.Data.Select(GetDisplayData())
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }

        protected DataTableUI GetDtProdutosPendenciasCfg()
        {
            DataTableUI dtMovimentacoesEstoqueCfg = new DataTableUI
            {
                Parent = "movimentacoesEstoqueField",
                Id = "dtMovimentacoesEstoque",
                UrlGridLoad = Url.Action("GetMovimentacoesEstoque", "RelatorioMovimentoEstoque"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Options = new DataTableUIConfig
                {
                    //WithoutRowMenu = true,
                    PageLength = -1,
                    LengthChange = true
                },
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "dataInicial", Required = false },
                    new DataTableUIParameter { Id = "dataFinal", Required = false },
                    new DataTableUIParameter { Id = "produtoId", Required = false },
                    new DataTableUIParameter { Id = "tipoMovimentoId", Required = false },
                    new DataTableUIParameter { Id = "inventarioId", Required = false }
                }
            };
                        
            dtMovimentacoesEstoqueCfg.Columns.Add(new DataTableUIColumn() { Priority = 1, Searchable = false, Orderable = false, DataField = "produtoDescricao", DisplayName = "Produto" });
            dtMovimentacoesEstoqueCfg.Columns.Add(new DataTableUIColumn() { Priority = 2, Searchable = false, Orderable = false, DataField = "data", DisplayName = "Data" });
            dtMovimentacoesEstoqueCfg.Columns.Add(new DataTableUIColumn() { Priority = 3, Searchable = false, Orderable = false, DataField = "quantidade", DisplayName = "Quantidade" });
            dtMovimentacoesEstoqueCfg.Columns.Add(new DataTableUIColumn() { Priority = 4, Searchable = false, Orderable = false, DataField = "saldoAntesMovimento", DisplayName = "Saldo Anterior" });
            dtMovimentacoesEstoqueCfg.Columns.Add(new DataTableUIColumn() { Priority = 5, Searchable = false, Orderable = false, DataField = "tipoMovimentoDescricao", DisplayName = "Tipo Movimento" });
            dtMovimentacoesEstoqueCfg.Columns.Add(new DataTableUIColumn() { Priority = 6, Searchable = false, Orderable = false, DataField = "inventarioDescricao", DisplayName = "Inventário" });
            dtMovimentacoesEstoqueCfg.Columns.Add(new DataTableUIColumn() { Priority = 7, Searchable = false, Orderable = false, DataField = "observacao", DisplayName = "Observação" });

            return dtMovimentacoesEstoqueCfg;
        }
    }
}
