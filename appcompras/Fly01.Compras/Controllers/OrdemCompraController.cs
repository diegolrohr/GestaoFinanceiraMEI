using Fly01.Compras.Controllers.Base;
using Fly01.Compras.Entities.ViewModel;
using Fly01.Core;
using Fly01.Core.API;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
{
    public class OrdemCompraController : BaseController<OrdemCompraVM>
    {
        public override Func<OrdemCompraVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                numero = x.Numero.ToString(),
                tipoOrdemCompra = x.TipoOrdemCompra,
                tipoOrdemCompraDescription = EnumHelper.SubtitleDataAnotation("TipoOrdemCompra", x.TipoOrdemCompra).Description,
                tipoOrdemCompraCssClass = EnumHelper.SubtitleDataAnotation("TipoOrdemCompra", x.TipoOrdemCompra).CssClass,
                tipoOrdemCompraValue = EnumHelper.SubtitleDataAnotation("TipoOrdemCompra", x.TipoOrdemCompra).Value,
                data = x.Data.ToString("dd/MM/yyyy"),
                total = x.Total?.ToString("C", AppDefaults.CultureInfoDefault),
                observacao = string.IsNullOrEmpty(x.Observacao) ? "" : x.Observacao.Substring(0, x.Observacao.Length <= 20 ? x.Observacao.Length : 20),
                status = x.Status,
                statusDescription = EnumHelper.SubtitleDataAnotation("StatusOrdemCompra", x.Status).Description,
                statusCssClass = EnumHelper.SubtitleDataAnotation("StatusOrdemCompra", x.Status).CssClass,
                statusValue = EnumHelper.SubtitleDataAnotation("StatusOrdemCompra", x.Status).Value,
            };
        }

        public override ContentResult List()
        {
            return Content(JsonConvert.SerializeObject(OrdemCompraJson(Url, Request.Url.Scheme), JsonSerializerSetting.Front), "application/json");
        }

        public ContentResult ListOrdemCompra()
        {
            return Content(JsonConvert.SerializeObject(OrdemCompraJson(Url, Request.Url.Scheme, gridLoad: "GridLoadNoFilter"), JsonSerializerSetting.Front), "application/json");
        }

        protected internal static ContentUI OrdemCompraJson(UrlHelper url, string scheme, bool withSidebarUrl = false, string gridLoad = "GridLoad")
        {
            var buttonLabel = "Mostrar todas as compras";
            var buttonOnClick = "fnRemoveFilter";

            if (gridLoad == "GridLoadNoFilter")
            {
                buttonLabel = "Mostrar compras do mês atual";
                buttonOnClick = "fnAddFilter";
            }
            
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = url.Action("Index", "Home") },
                Header = new HtmlUIHeader
                {
                    Title = "Orçamento / Pedido",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo orçamento", OnClickFn = "fnNovoOrcamento" },
                        new HtmlUIButton { Id = "new", Label = "Novo pedido", OnClickFn = "fnNovoPedido" },
                        new HtmlUIButton { Id = "filterGrid", Label = buttonLabel, OnClickFn = buttonOnClick },
                    }
                },
                UrlFunctions = url.Action("Functions", "OrdemCompra") + "?fns="
            };

            if (withSidebarUrl)
                cfg.SidebarUrl = url.Action("Sidebar", "Home", null, scheme);

            if(gridLoad == "GridLoad")
            {
                var cfgForm = new FormUI
                {
                    ReadyFn = "fnUpdateDataFinal",
                    UrlFunctions = url.Action("Functions", "OrdemCompra") + "?fns=",
                    Elements = new List<BaseUI>()
                    {
                        new PeriodpickerUI()
                        {
                           Label= "Selecione o período",
                           Id= "mesPicker",
                           Name= "mesPicker",
                           Class= "col s12 m4 offset-m4",
                           DomEvents = new List<DomEventUI>()
                           {
                               new DomEventUI()
                               {
                                  DomEvent = "change",
                                  Function = "fnUpdateDataFinal"
                               }
                           }
                        },
                        new InputHiddenUI(){ Id= "dataFinal" },
                        new InputHiddenUI(){ Id= "dataInicial" }
                    }
                };

                cfg.Content.Add(cfgForm);
            }

            var config = new DataTableUI
            {
                UrlGridLoad = url.Action(gridLoad, "OrdemCompra"),
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() {Id = "dataInicial", Required = (gridLoad == "GridLoad") },
                    new DataTableUIParameter() {Id = "dataFinal", Required = (gridLoad == "GridLoad") }
                },
                UrlFunctions = url.Action("Functions", "OrdemCompra") + "?fns="
            };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnVisualizarPedido", Label = "Visualizar", ShowIf = "(row.status != 'Aberto' && row.tipoOrdemCompra == 'Pedido')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditarPedido", Label = "Editar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnVisualizarOrcamento", Label = "Visualizar", ShowIf = "(row.status != 'Aberto' && row.tipoOrdemCompra == 'Orcamento')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditarOrcamento", Label = "Editar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Orcamento')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluirPedido", Label = "Excluir", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluirOrcamento", Label = "Excluir", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Orcamento')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnGerarPedidos", Label = "Gerar pedidos", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Orcamento')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnFinalizarPedido", Label = "Finalizar pedido", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnImprimirPedido", Label = "Imprimir", ShowIf = "(row.tipoOrdemCompra == 'Pedido')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnImprimirOrcamento", Label = "Imprimir", ShowIf = "(row.tipoOrdemCompra == 'Orcamento')" });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoOrdemCompra",
                DisplayName = "Tipo",
                Priority = 1,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoOrdemCompra", true, false)),
                RenderFn = "function(data, type, full, meta) { return \"<span class=\\\"new badge \" + full.tipoOrdemCompraCssClass + \" left\\\" data-badge-caption=\\\" \\\">\" + full.tipoOrdemCompraDescription + \"</span>\" }"
            });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 5,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("StatusOrdemCompra", true, false)),
                RenderFn = "function(data, type, full, meta) { return \"<span class=\\\"new badge \" + full.statusCssClass + \" left\\\" data-badge-caption=\\\" \\\">\" + full.statusDescription + \"</span>\" }"
            });

            config.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Número", Priority = 2, Type = "numbers" });
            config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 4, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "total", DisplayName = "Total", Priority = 3, Type = "currency" });
            
            config.Columns.Add(new DataTableUIColumn { DataField = "observacao", DisplayName = "Observação", Priority = 6 });
                    
            cfg.Content.Add(config);
            
            return cfg;
        }

        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
                filters = new Dictionary<string, string>();

            filters.Add("data le ", Request.QueryString["dataFinal"]);
            filters.Add(" and data ge ", Request.QueryString["dataInicial"]);

            return base.GridLoad(filters);
        }

        public JsonResult GridLoadNoFilter()
        {
            return base.GridLoad();
        }
        
        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("$orderby", "data,numero");

            return customFilters;
        }
    }
}