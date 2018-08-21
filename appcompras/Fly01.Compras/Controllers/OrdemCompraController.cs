using Fly01.Compras.ViewModel;
using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
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
            //ExpandProperties = "fornecedor,grupoTributarioPadrao($select=id,descricao,tipoTributacaoICMS),transportadora($select=id,nome),estadoPlacaVeiculo,condicaoParcelamento,formaPagamento,categoria";
            ExpandProperties = "grupoTributarioPadrao($select=id,descricao,tipoTributacaoICMS),estadoPlacaVeiculo,condicaoParcelamento,formaPagamento,categoria";
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
                total = x.Total?.ToString("C", AppDefaults.CultureInfoDefault),
                observacao = string.IsNullOrEmpty(x.Observacao) ? "" : x.Observacao.Substring(0, x.Observacao.Length <= 20 ? x.Observacao.Length : 20),
                statusDescription = EnumHelper.GetDescription(typeof(StatusOrdemCompra), x.Status),
                statusCssClass = EnumHelper.GetCSS(typeof(StatusOrdemCompra), x.Status),
                statusValue = EnumHelper.GetValue(typeof(StatusOrdemCompra), x.Status),
                geraNotaFiscal = x.GeraNotaFiscal
            };
        }

        public override ContentResult List() 
            => Content(JsonConvert.SerializeObject(OrdemCompraJson(Url, Request.Url.Scheme), JsonSerializerSetting.Front), "application/json");

        public ContentResult ListOrdemCompra() 
            => Content(JsonConvert.SerializeObject(OrdemCompraJson(Url, Request.Url.Scheme, gridLoad: "GridLoadNoFilter"), JsonSerializerSetting.Front), "application/json");

        public List<HtmlUIButton> GetListButtonsOnHeaderCustom(string buttonLabel, string buttonOnClick)
        {
            var target = new List<HtmlUIButton>();

            if(UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo orçamento", OnClickFn = "fnNovoOrcamento", Position = HtmlUIButtonPosition.Out});
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo pedido", OnClickFn = "fnNovoPedido", Position = HtmlUIButtonPosition.Main });
                target.Add(new HtmlUIButton { Id = "filterGrid", Label = buttonLabel, OnClickFn = buttonOnClick, Position = HtmlUIButtonPosition.In });
            }

            return target;
        }

        public override List<DataTableUIAction> GetActionsInGrid(List<DataTableUIAction> customWriteActions)
        {
            if (UserCanWrite)
                return customWriteActions;

            return new List<DataTableUIAction>
            {
                new DataTableUIAction { OnClickFn = "fnVisualizar", Label = "Visualizar" },
                new DataTableUIAction { OnClickFn = "fnEditarPedido", Label = "Editar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnEditarOrcamento", Label = "Editar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Orcamento')" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "(row.status == 'Aberto')" },
                new DataTableUIAction { OnClickFn = "fnFinalizarPedido", Label = "Finalizar pedido", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido' && row.geraNotaFiscal == false)" },
                new DataTableUIAction { OnClickFn = "fnFinalizarFaturarPedido", Label = "Finalizar e faturar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" },
            };
        }

        protected ContentUI OrdemCompraJson(UrlHelper url, string scheme, bool withSidebarUrl = false, string gridLoad = "GridLoad")
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
                History = new ContentUIHistory { Default = url.Action("Index", "OrdemCompra") },
                Header = new HtmlUIHeader
                {
                    Title = "Orçamento / Pedido",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeaderCustom(buttonLabel, buttonOnClick))
                },
                UrlFunctions = url.Action("Functions", "OrdemCompra") + "?fns="
            };

            if (withSidebarUrl)
                cfg.SidebarUrl = url.Action("Sidebar", "OrdemCompra", null, scheme);

            if (gridLoad == "GridLoad")
            {
                var cfgForm = new FormUI
                {
                    ReadyFn = "fnUpdateDataFinal",
                    UrlFunctions = url.Action("Functions", "OrdemCompra") + "?fns=",
                    Elements = new List<BaseUI>()
                    {
                        new PeriodPickerUI()
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
                UrlFunctions = url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" }
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnVisualizarPedido", Label = "Visualizar", ShowIf = "(row.tipoOrdemCompra == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnEditarPedido", Label = "Editar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnEditarOrcamento", Label = "Editar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Orcamento')" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "(row.status == 'Aberto')" },
                new DataTableUIAction { OnClickFn = "fnFinalizarPedido", Label = "Finalizar pedido", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido' && row.geraNotaFiscal == false)" },
                new DataTableUIAction { OnClickFn = "fnFinalizarFaturarPedido", Label = "Finalizar e faturar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" },

                //new DataTableUIAction { OnClickFn = "fnVisualizarPedido", Label = "Visualizar", ShowIf = "(row.tipoOrdemCompra == 'Pedido')" },
                //new DataTableUIAction { OnClickFn = "fnVisualizarOrcamento", Label = "Visualizar", ShowIf = "(row.tipoOrdemCompra == 'Orcamento')" },
                //new DataTableUIAction { OnClickFn = "fnEditarPedido", Label = "Editar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" },
                //new DataTableUIAction { OnClickFn = "fnEditarOrcamento", Label = "Editar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Orcamento')" },
                //new DataTableUIAction { OnClickFn = "fnExcluirPedido", Label = "Excluir", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" },
                //new DataTableUIAction { OnClickFn = "fnExcluirOrcamento", Label = "Excluir", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Orcamento')" },
                new DataTableUIAction { OnClickFn = "fnGerarPedidos", Label = "Gerar pedidos", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Orcamento')" },
                //new DataTableUIAction { OnClickFn = "fnFinalizarPedido", Label = "Finalizar pedido", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemCompra == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnImprimirPedido", Label = "Imprimir", ShowIf = "(row.tipoOrdemCompra == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnImprimirOrcamento", Label = "Imprimir", ShowIf = "(row.tipoOrdemCompra == 'Orcamento')" }
            }));

            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoOrdemCompra",
                DisplayName = "Tipo",
                Priority = 1,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoOrdemCompra))),
                RenderFn = "fnRenderEnum(full.tipoOrdemCompraCssClass, full.tipoOrdemCompraDescription)"
            });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 5,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusOrdemCompra))),
                RenderFn = "fnRenderEnum(full.statusCssClass, full.statusDescription)"
            });

            config.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Número", Priority = 2, Type = "numbers" });
            config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 4, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "total", DisplayName = "Total", Priority = 3, Type = "currency" });

            config.Columns.Add(new DataTableUIColumn { DataField = "observacao", DisplayName = "Observação", Priority = 6 });

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

        //public ContentResult Visualizar()
        //{
        //    ModalUIForm config = new ModalUIForm()
        //    {
        //        Title = "Visualizar",
        //        UrlFunctions = @Url.Action("Functions", "OrdemCompra") + "?fns=",
        //        CancelAction = new ModalUIAction() { Label = "Cancelar" },
        //        Action = new FormUIAction
        //        {
        //            Create = @Url.Action("Create"),
        //            Edit = @Url.Action("Edit"),
        //            Get = @Url.Action("Json") + "/",
        //            List = @Url.Action("List")
        //        },
        //        ReadyFn = "fnFormReadyVisualizarOrdemCompra",
        //        Id = "fly01mdlfrmVisualizarPedido"
        //    };

        //    config.Elements.Add(new InputHiddenUI { Id = "id" });
        //    config.Elements.Add(new InputNumbersUI { Id = "numero", Class = "col s12 m6 l2", Label = "Número", Disabled = true });
        //    config.Elements.Add(new SelectUI
        //    {
        //        Id = "tipoCompra",
        //        Class = "col s12 m6 l4",
        //        Label = "Tipo Compra",
        //        Value = "Normal",
        //        Disabled = true,
        //        Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoVenda)).
        //        ToList().FindAll(x => "Normal,Devolucao,Complementar".Contains(x.Value)))
        //    });
        //    config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m6 l2", Label = "Data", Disabled = true });
        //    config.Elements.Add(new InputCheckboxUI { Id = "nFeRefComplementarIsDevolucao", Class = "col s12 m6 l4", Label = "NF Referenciada é de Devolução", Disabled = true });


        //    config.Elements.Add(new InputNumbersUI { Id = "chaveNFeReferenciada", Class = "col s12 m6", Label = "Chave SEFAZ Nota Fiscal Referenciada", Disabled = true });
        //    config.Elements.Add(new AutoCompleteUI
        //    {
        //        Id = "fornecedorId",
        //        Class = "col s12 m6",
        //        Label = "Fornecedor",
        //        Disabled = true,
        //        DataUrl = Url.Action("Fornecedor", "AutoComplete"),
        //        LabelId = "fornecedorNome"
        //    });
        //    config.Elements.Add(new AutoCompleteUI
        //    {
        //        Id = "grupoTributarioPadraoId",
        //        Class = "col s12 m6",
        //        Label = "Grupo Tributário Padrão",
        //        Disabled = true,
        //        DataUrl = Url.Action("GrupoTributario", "AutoComplete"),
        //        LabelId = "grupoTributarioPadraoDescricao"
        //    });
        //    config.Elements.Add(new TextAreaUI
        //    {
        //        Id = "observacao",
        //        Class = "col s12",
        //        Label = "Observação",
        //        MaxLength = 200,
        //        Disabled = true
        //    });

        //    config.Elements.Add(new AutoCompleteUI
        //    {
        //        Id = "formaPagamentoId",
        //        Class = "col s12 m6",
        //        Label = "Forma Pagamento",
        //        Disabled = true,
        //        DataUrl = Url.Action("FormaPagamento", "AutoComplete"),
        //        LabelId = "formaPagamentoDescricao"
        //    });
        //    config.Elements.Add(new AutoCompleteUI
        //    {
        //        Id = "condicaoParcelamentoId",
        //        Class = "col s12 m6",
        //        Label = "Condição Parcelamento",
        //        Disabled = true,
        //        DataUrl = Url.Action("CondicaoParcelamento", "AutoComplete"),
        //        LabelId = "condicaoParcelamentoDescricao"
        //    });
        //    config.Elements.Add(new AutoCompleteUI
        //    {
        //        Id = "categoriaId",
        //        Class = "col s12 m6",
        //        Label = "Categoria",
        //        Disabled = true,
        //        DataUrl = @Url.Action("Categoria", "AutoComplete"),
        //        LabelId = "categoriaDescricao",
        //    });
        //    config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m6", Label = "Data Vencimento", Disabled = true });

        //    config.Elements.Add(new SelectUI
        //    {
        //        Id = "tipoFrete",
        //        Class = "col s12 m6",
        //        Label = "Tipo Frete",
        //        Value = "SemFrete",
        //        Disabled = true,
        //        Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoFrete))),
        //    });
        //    config.Elements.Add(new AutoCompleteUI
        //    {
        //        Id = "transportadoraId",
        //        Class = "col s12 m6",
        //        Label = "Transportadora",
        //        Disabled = true,
        //        DataUrl = Url.Action("Transportadora", "AutoComplete"),
        //        LabelId = "transportadoraNome"
        //    });
        //    config.Elements.Add(new InputCustommaskUI
        //    {
        //        Id = "placaVeiculo",
        //        Class = "col s12 m4",
        //        Label = "Placa Veículo",
        //        Disabled = true,
        //        Data = new { inputmask = "'mask':'AAA-9999', 'showMaskOnHover': false, 'autoUnmask':true" }
        //    });
        //    config.Elements.Add(new AutoCompleteUI
        //    {
        //        Id = "estadoPlacaVeiculoId",
        //        Class = "col s12 m4",
        //        Label = "UF Placa Veículo",
        //        Disabled = true,
        //        DataUrl = Url.Action("Estado", "AutoComplete"),
        //        LabelId = "estadoPlacaVeiculoNome"
        //    });
        //    config.Elements.Add(new InputCurrencyUI { Id = "valorFrete", Class = "col s12 m4", Label = "Valor Frete", Disabled = true });
        //    config.Elements.Add(new InputFloatUI { Id = "pesoBruto", Class = "col s12 m4", Label = "Peso Bruto", Digits = 3, Disabled = true });
        //    config.Elements.Add(new InputTextUI { Id = "marca", Class = "col s12 m4", Label = "Marca", Disabled = true, MaxLength = 60 });
        //    config.Elements.Add(new InputFloatUI { Id = "pesoLiquido", Class = "col s12 m4", Label = "Peso Líquido", Digits = 3, Disabled = true });
        //    config.Elements.Add(new InputNumbersUI { Id = "quantidadeVolumes", Class = "col s12 m4", Label = "Quantidade Volumes", Disabled = true });
        //    config.Elements.Add(new InputTextUI { Id = "tipoEspecie", Class = "col s12 m4", Label = "Tipo Espécie", Disabled = true, MaxLength = 60 });
        //    config.Elements.Add(new InputTextUI { Id = "numeracaoVolumesTrans", Class = "col s12 m4", Label = "Numeração", Disabled = true, MaxLength = 60 });

        //    config.Elements.Add(new InputTextUI { Id = "naturezaOperacao", Class = "col s12", Label = "Natureza de Operação", Disabled = true });

        //    config.Elements.Add(new LabelSetUI { Id = "labelSetTotais", Class = "col s12", Label = "Totais" });
        //    config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m6", Label = "Total produtos", Readonly = true });
        //    config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m6", Label = "Frete a pagar", Readonly = true });
        //    config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutos", Class = "col s12 m6", Label = "Total impostos produtos incidentes", Readonly = true });
        //    config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutosNaoAgrega", Class = "col s12 m6", Label = "Total de impostos não incidentes", Readonly = true });
        //    config.Elements.Add(new InputCurrencyUI { Id = "totalOrdemCompra", Class = "col s12", Label = "Total (produtos + impostos + frete)", Readonly = true });
        //    config.Elements.Add(new InputCheckboxUI { Id = "movimentaEstoque", Class = "col s12 m4", Label = "Movimenta estoque", Disabled = true });
        //    config.Elements.Add(new InputCheckboxUI { Id = "geraNotaFiscal", Class = "col s12 m4", Label = "Faturar", Disabled = true });
        //    config.Elements.Add(new InputCheckboxUI { Id = "geraFinanceiro", Class = "col s12 m4", Label = "Gera financeiro", Disabled = true });

        //    config.Elements.Add(new LabelSetUI { Id = "labelSetProdutos", Class = "col s12", Label = "Produtos" });
        //    config.Elements.Add(new TableUI
        //    {
        //        Id = "PedidoItemDataTable",
        //        Class = "col s12",
        //        Disabled = true,
        //        Options = new List<OptionUI>
        //        {
        //            new OptionUI { Label = "Produto", Value = "0"},
        //            new OptionUI { Label = "GrupoTributário", Value = "0"},
        //            new OptionUI { Label = "Quant.", Value = "1"},
        //            new OptionUI { Label = "Valor",Value = "2"},
        //            new OptionUI { Label = "Desconto",Value = "3"},
        //            new OptionUI { Label = "Total",Value = "4"},
        //        }
        //    });
        //    return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        //}
    }
}
