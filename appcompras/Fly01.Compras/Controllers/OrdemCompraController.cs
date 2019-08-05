using Fly01.Compras.ViewModel;
using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasOrcamentoPedido)]
    public class OrdemCompraController : BaseController<OrdemCompraVM>
    {
        public OrdemCompraController()
        {
            ExpandProperties = "grupoTributarioPadrao($select=id,descricao,tipoTributacaoICMS),estadoPlacaVeiculo,condicaoParcelamento,formaPagamento,categoria";
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var queryString = base.GetQueryStringDefaultGridLoad();
            queryString.Add("$select", "id,numero,tipoOrdemCompra,data,status,total,observacao");
            queryString.AddParam("$orderby", "data,numero");
            return queryString;
        }

        public override Func<OrdemCompraVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                numero = x.Numero.ToString(),
                tipoOrdemCompra = x.TipoOrdemCompra,
                tipoOrdemCompraDescription = EnumHelper.GetDescription(typeof(TipoOrdemCompra), x.TipoOrdemCompra),
                tipoOrdemCompraCssClass = EnumHelper.GetCSS(typeof(TipoOrdemCompra), x.TipoOrdemCompra),
                tipoOrdemCompraValue = EnumHelper.GetValue(typeof(TipoOrdemCompra), x.TipoOrdemCompra),
                data = x.Data.ToString("dd/MM/yyyy"),
                status = x.Status,
                total = x.Total.ToString("C", AppDefaults.CultureInfoDefault),
                observacao = string.IsNullOrEmpty(x.Observacao) ? "" : x.Observacao.Substring(0, x.Observacao.Length <= 20 ? x.Observacao.Length : 20),
                statusDescription = EnumHelper.GetDescription(typeof(StatusOrdemCompra), x.Status),
                statusCssClass = EnumHelper.GetCSS(typeof(StatusOrdemCompra), x.Status),
                statusValue = EnumHelper.GetValue(typeof(StatusOrdemCompra), x.Status)
            };
        }

        public override ContentResult List()
            => Content(JsonConvert.SerializeObject(OrdemCompraJson(Url, Request.Url.Scheme), JsonSerializerSetting.Front), "application/json");

        public ContentResult ListOrdemCompra()
            => Content(JsonConvert.SerializeObject(OrdemCompraJson(Url, Request.Url.Scheme, gridLoad: "GridLoadNoFilter"), JsonSerializerSetting.Front), "application/json");

        public List<HtmlUIButton> GetListButtonsOnHeaderCustom(string buttonLabel, string buttonOnClick)
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo orçamento", OnClickFn = "fnNovoOrcamento", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo pedido", OnClickFn = "fnNovoPedido", Position = HtmlUIButtonPosition.Main });
                target.Add(new HtmlUIButton { Id = "filterGrid", Label = buttonLabel, OnClickFn = buttonOnClick, Position = HtmlUIButtonPosition.In });
            }

            return target;
        }

        protected ContentUI OrdemCompraJson(UrlHelper url, string scheme, string gridLoad = "GridLoad")
        {
            ConfiguracaoPersonalizacaoVM personalizacao = null;
            try
            {
                personalizacao = RestHelper.ExecuteGetRequest<ResultBase<ConfiguracaoPersonalizacaoVM>>("ConfiguracaoPersonalizacao", queryString: null)?.Data?.FirstOrDefault();
            }
            catch (Exception)
            {

            }
            var emiteNotaFiscal = personalizacao != null ? personalizacao.EmiteNotaFiscal : true;

            var buttonLabel = "Mostrar todas as compras";
            var buttonOnClick = "fnRemoveFilter";

            if (gridLoad == "GridLoadNoFilter")
            {
                buttonLabel = "Mostrar compras do mês atual";
                buttonOnClick = "fnAddFilter";
            }

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = url.Action("Index", "OrdemCompra") },
                Header = new HtmlUIHeader
                {
                    Title = "Orçamento / Pedido",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeaderCustom(buttonLabel, buttonOnClick))
                },
                UrlFunctions = url.Action("Functions", "OrdemCompra") + "?fns="
            };

            var cfgForm = new FormUI
            {
                Id = "fly01frm",
                UrlFunctions = url.Action("Functions", "OrdemCompra") + "?fns=",
                ReadyFn = gridLoad == "GridLoad" ? "" : "fnChangeInput",
                Elements = new List<BaseUI>()
                {
                    new InputHiddenUI()
                    {
                        Id = "dataFinal",
                        Name = "dataFinal"
                    },
                    new InputHiddenUI()
                    {
                        Id = "dataInicial",
                        Name = "dataInicial"
                    }
                }
            };

            if (gridLoad == "GridLoad")
            {
                cfgForm.Elements.Add(new PeriodPickerUI()
                {
                    Label = "Selecione o período",
                    Id = "mesPicker",
                    Name = "mesPicker",
                    Class = "col s12 m6 offset-m3 l4 offset-l4",
                    DomEvents = new List<DomEventUI>()
                    {
                        new DomEventUI()
                        {
                            DomEvent = "change",
                            Function = "fnUpdateDataFinal"
                        }
                    }
                });
                cfgForm.ReadyFn = "fnUpdateDataFinal";
            }

            cfg.Content.Add(cfgForm);
            var config = new DataTableUI
            {
                Id = "fly01dt",
                UrlGridLoad = url.Action(gridLoad, "OrdemCompra"),
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() {Id = "dataInicial", Required = (gridLoad == "GridLoad") },
                    new DataTableUIParameter() {Id = "dataFinal", Required = (gridLoad == "GridLoad") }
                },
                UrlFunctions = url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" }
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnVisualizarPedido", Label = "Visualizar", ShowIf = "(row.tipoOrdemCompra == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnVisualizarOrcamento", Label = "Visualizar", ShowIf = "(row.tipoOrdemCompra == 'Orcamento')" },
                new DataTableUIAction { OnClickFn = "fnEditarPedido", Label = "Editar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnEditarOrcamento", Label = "Editar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Orcamento')" },
                new DataTableUIAction { OnClickFn = "fnFinalizarPedido", Label = "Finalizar pedido", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnFinalizarFaturarPedido", Label = "Finalizar e faturar", ShowIf = $"({emiteNotaFiscal.ToString().ToLower()} && row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnExcluirPedido", Label = "Excluir", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnExcluirOrcamento", Label = "Excluir", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Orcamento')" },
                new DataTableUIAction { OnClickFn = "fnGerarPedidos", Label = "Gerar pedidos", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Orcamento')" },
                new DataTableUIAction { OnClickFn = "fnImprimirPedido", Label = "Imprimir", ShowIf = "(row.tipoOrdemCompra == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnImprimirOrcamento", Label = "Imprimir", ShowIf = "(row.tipoOrdemCompra == 'Orcamento')" },
                new DataTableUIAction { OnClickFn = "fnEnviarEmailPedido", Label = "Enviar por e-mail", ShowIf = "(row.tipoOrdemCompra == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnClonarPedido", Label = "Clonar Pedido", ShowIf = "(row.status == 'Finalizado' && row.tipoOrdemCompra == 'Pedido')" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Número", Priority = 1, Type = "numbers" });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 2,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusOrdemCompra))),
                RenderFn = "fnRenderEnum(full.statusCssClass, full.statusDescription)"
            });

            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoOrdemCompra",
                DisplayName = "Tipo",
                Priority = 3,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoOrdemCompra))),
                RenderFn = "fnRenderEnum(full.tipoOrdemCompraCssClass, full.tipoOrdemCompraDescription)"
            });

            config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 4, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "total", DisplayName = "Total", Priority = 5, Type = "currency" });

            cfg.Content.Add(config);

            return cfg;
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
                filters = new Dictionary<string, string>();

            if (Request.QueryString["dataFinal"] != "")
                filters.Add("data le ", Request.QueryString["dataFinal"]);
            if (Request.QueryString["dataInicial"] != "")
                filters.Add(" and data ge ", Request.QueryString["dataInicial"]);

            return base.GridLoad(filters);
        }

        public JsonResult GridLoadNoFilter()
        {
            return GridLoad();
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }
    }
}
