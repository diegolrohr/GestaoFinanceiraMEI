using Fly01.Compras.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.Helpers;
using Fly01.Compras.Models.ViewModel;
using Fly01.Core.Config;
using Fly01.Compras.Models.Reports;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation;
using Fly01.uiJS.Classes.Helpers;
using System.Dynamic;
using Fly01.Core.ViewModels;
using Fly01.Core.Mensageria;
using System.IO;
using Fly01.Compras.Helpers;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasOrcamentoPedido)]
    public class PedidoController : BaseController<PedidoVM>
    {
        //OrcamentoVM e PedidoVM na mesma controller ordemCompra(gridLoad, form), direcionado para a controller via javaScript
        public PedidoController()
        {
            ExpandProperties = "condicaoParcelamento($select=descricao, qtdParcelas,condicoesParcelamento),formaPagamento($select=id,descricao),fornecedor($select=id,nome,email,endereco,numero,bairro,cep,complemento;$expand=cidade($select=nome),estado($select=sigla),pais($select=nome)),transportadora($select=id,nome),estadoPlacaVeiculo,categoria,centroCusto"; //expand = Cidade($select = id, nome)
        }

        [HttpPost]
        public JsonResult CreateTransportadora(PessoaVM entityVM)
        {
            try
            {
                var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));

                const string regexSomenteDigitos = @"[^\d]";
                entityVM.Transportadora = true;
                entityVM.CPFCNPJ = Regex.Replace(entityVM.CPFCNPJ ?? "", regexSomenteDigitos, "");
                entityVM.TipoDocumento = GetTipoDocumento(entityVM.CPFCNPJ ?? "");
                entityVM.Celular = Regex.Replace(entityVM.Celular ?? "", regexSomenteDigitos, "");
                entityVM.Telefone = Regex.Replace(entityVM.Telefone ?? "", regexSomenteDigitos, "");
                entityVM.CEP = Regex.Replace(entityVM.CEP ?? "", regexSomenteDigitos, "");

                var postResponse = RestHelper.ExecutePostRequest(resourceName, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                PessoaVM postResult = JsonConvert.DeserializeObject<PessoaVM>(postResponse);
                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, postResult.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        private string GetTipoDocumento(string documento)
        {
            if (documento.Length <= 11)
                return "F";
            if (documento.Length > 11)
                return "J";

            return null;
        }

        public override Func<PedidoVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        protected DataTableUI GetDtPedidoItensCfg()
        {
            DataTableUI dtPedidoItensCfg = new DataTableUI
            {
                Parent = "pedidoProdutosField",
                Id = "dtPedidoItens",
                UrlGridLoad = Url.Action("GetOrdemCompraProdutos", "PedidoItem"),
                UrlFunctions = Url.Action("Functions", "PedidoItem") + "?fns=",
                Callbacks = new DataTableUICallbacks()
                {
                    FooterCallback = "fnFooterCallbackPedidoItem"
                },
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                },
                Functions = new List<string>() { "fnFooterCallbackPedidoItem" }
            };

            dtPedidoItensCfg.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditarOrdemCompraItem", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluirOrdemCompraItem", Label = "Excluir" }
            }));

            dtPedidoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "produto_descricao", DisplayName = "Produto", Priority = 1, Searchable = false, Orderable = false });
            dtPedidoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "grupoTributario_descricao", DisplayName = "Grupo Tributário", Priority = 2, Searchable = false, Orderable = false });
            dtPedidoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantidade", DisplayName = "Quantidade", Priority = 3, Type = "float", Searchable = false, Orderable = false });
            dtPedidoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "valor", DisplayName = "Valor", Priority = 4, Type = "currency", Searchable = false, Orderable = false });
            dtPedidoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "desconto", DisplayName = "Desconto", Priority = 5, Type = "currency", Searchable = false, Orderable = false });
            dtPedidoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "total", DisplayName = "Total", Priority = 6, Type = "currency", Searchable = false, Orderable = false });

            return dtPedidoItensCfg;
        }

        protected override ContentUI FormJson()
            => FormPedidoJson();

        public ContentResult FormPedido(bool isEdit = false)
            => Content(JsonConvert.SerializeObject(FormPedidoJson(isEdit), JsonSerializerSetting.Front), "application/json");

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelarPedido" });
            }

            return target;
        }

        protected DataTableUI GetDtOrdemCompraItemCfg()
        {
            DataTableUI dtOrdemCompraItemCfg = new DataTableUI
            {
                Parent = "ordemCompraItemField",
                Id = "dtOrdemCompraItem",
                UrlGridLoad = Url.Action("GetOrdemCompraProdutos", "PedidoItem"),
                UrlFunctions = Url.Action("Functions", "Pedido") + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                }
            };

            dtOrdemCompraItemCfg.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditarOrdemCompraItem", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluirOrdemCompraItem", Label = "Excluir" }
            }));

            dtOrdemCompraItemCfg.Columns.Add(new DataTableUIColumn() { DataField = "produto_descricao", DisplayName = "Produto", Priority = 1, Searchable = false, Orderable = false });
            dtOrdemCompraItemCfg.Columns.Add(new DataTableUIColumn() { DataField = "grupoTributario_descricao", DisplayName = "Grupo Tributário", Priority = 2, Searchable = false, Orderable = false });
            dtOrdemCompraItemCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantidade", DisplayName = "Quantidade", Priority = 3, Type = "float", Searchable = false, Orderable = false });
            dtOrdemCompraItemCfg.Columns.Add(new DataTableUIColumn() { DataField = "valor", DisplayName = "Valor", Priority = 4, Type = "currency", Searchable = false, Orderable = false });
            dtOrdemCompraItemCfg.Columns.Add(new DataTableUIColumn() { DataField = "desconto", DisplayName = "Desconto", Priority = 5, Type = "currency", Searchable = false, Orderable = false });
            dtOrdemCompraItemCfg.Columns.Add(new DataTableUIColumn() { DataField = "total", DisplayName = "Total", Priority = 6, Type = "currency", Searchable = false, Orderable = false });

            return dtOrdemCompraItemCfg;
        }

        protected DataTableUI GetDtProdutosEstoqueNegativoCfg()
        {
            DataTableUI dtProdutosEstoqueNegativoCfg = new DataTableUI
            {
                Parent = "produtosEstoqueNegativoField",
                Id = "dtProdutosEstoqueNegativo",
                UrlGridLoad = Url.Action("VerificaEstoqueNegativo"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true },
                    new DataTableUIParameter { Id = "tipoCompra", Required = true },
                },
                Callbacks = new DataTableUICallbacks()
                {
                    FooterCallback = "fnFooterCallbackEstoqueNegativo"
                },
                Functions = new List<string>() { "fnFooterCallbackEstoqueNegativo" }
            };

            dtProdutosEstoqueNegativoCfg.Columns.Add(new DataTableUIColumn() { DataField = "produtoDescricao", DisplayName = "Produto", Priority = 1, Searchable = false, Orderable = false });
            dtProdutosEstoqueNegativoCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantPedido", DisplayName = "Quantidade Pedido", Priority = 2, Type = "float", Searchable = false, Orderable = false });
            dtProdutosEstoqueNegativoCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantEstoque", DisplayName = "Estoque Atual", Priority = 3, Type = "float", Searchable = false, Orderable = false });
            dtProdutosEstoqueNegativoCfg.Columns.Add(new DataTableUIColumn() { DataField = "saldoEstoque", DisplayName = "Saldo Estoque", Priority = 4, Type = "float", Searchable = false, Orderable = false });

            return dtProdutosEstoqueNegativoCfg;
        }

        public ContentUI FormPedidoJson(bool isEdit = false)
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
            var exibirTransportadora = personalizacao != null ? personalizacao.ExibirStepTransportadoraCompras : true;

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create"),
                    WithParams = Url.Action("Edit")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Pedido",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormWizardUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List", "OrdemCompra")
                },
                ReadyFn = "fnFormReadyPedido",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnChangeEstado", "fnChangeFinalidade" },
                Steps = new List<FormWizardUIStep>()
                {
                    new FormWizardUIStep()
                    {
                        Title = "Finalidade",
                        Id = "stepFinalidade",
                        Quantity = 2,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Cadastro",
                        Id = "stepCadastro",
                        Quantity = 13,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Produtos",
                        Id = "stepProdutos",
                        Quantity = 3,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Financeiro",
                        Id = "stepFinanceiro",
                        Quantity = 6,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Transporte",
                        Id = "stepTransporte",
                        Quantity = 11,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Finalizar",
                        Id = "stepFinalizar",
                        Quantity = 15,
                    }
                },
                Rule = isEdit ? "parallel" : "linear",
                ShowStepNumbers = true
            };

            #region step Finalidade
            if (!emiteNotaFiscal)
            {
                config?.Steps?.Remove(config?.Steps?.Find(x => x.Id == "stepFinalidade"));
            }
            else
            {
                config.Elements.Add(new ButtonGroupUI()
                {
                    Id = "fly01btngrpFinalidade",
                    Class = "col s12 m6 offset-m3",
                    OnClickFn = "fnChangeFinalidade",
                    Label = "Tipo do pedido",
                    Options = new List<ButtonGroupOptionUI>
                    {
                        new ButtonGroupOptionUI { Id = "btnNormal", Value = "Normal", Label = "Normal"},
                        new ButtonGroupOptionUI { Id = "btnDevolucao", Value = "Devolucao", Label = "Devolução"},
                    }
                });
                config.Elements.Add(new InputNumbersUI { Id = "chaveNFeReferenciada", Class = "col s12 m8 offset-m2", Label = "Chave SEFAZ Nota Fiscal Referenciada", MinLength = 44, MaxLength = 44 });
            }
            #endregion

            #region step Cadastro
            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoCompra", Value = "Normal" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoCarteira", Value = "Despesa" });
            config.Elements.Add(new InputHiddenUI { Id = "status", Value = "Aberto" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoOrdemCompra", Value = "Pedido" });
            config.Elements.Add(new InputHiddenUI { Id = "grupoTributarioPadraoTipoTributacaoICMS" });
            config.Elements.Add(new InputHiddenUI { Id = "cfopDescricao" });
            config.Elements.Add(new InputNumbersUI { Id = "numero", Class = "col s12 m2", Label = "Número", Disabled = true });

            config.Elements.Add(new InputHiddenUI { Id = "emiteNotaFiscal", Value = emiteNotaFiscal.ToString() });
            config.Elements.Add(new InputHiddenUI { Id = "exibeStepTransportadora", Value = exibirTransportadora.ToString() });

            if (!exibirTransportadora)
            {
                var stepCadastro = config?.Steps?.Find(x => x.Id == "stepCadastro");
                stepCadastro.Quantity += 2;
                config.Elements.Add(new InputHiddenUI { Id = "tipoFrete", Value = "SemFrete" });
                config.Elements.Add(new InputHiddenUI { Id = "valorFrete", Value = "0" });
            }

            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m3", Label = "Data", Required = true });
            if (emiteNotaFiscal)
            {
                var stepCadastro = config?.Steps?.Find(x => x.Id == "stepCadastro");
                stepCadastro.Quantity += 1;
                config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
                {
                    Id = "grupoTributarioPadraoId",
                    Class = "col s12 m7",
                    Label = "Grupo Tributário Padrão",
                    DataUrl = Url.Action("GrupoTributario", "AutoComplete"),
                    LabelId = "grupoTributarioPadraoDescricao",
                    DataUrlPostModal = Url.Action("FormModal", "GrupoTributario"),
                    DataPostField = "descricao",
                    DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeGrupoTribPadrao" } }
                }, ResourceHashConst.ComprasCadastrosGrupoTributario));
            }

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "fornecedorId",
                Class = "col s12",
                Label = "Fornecedor",
                Required = true,
                DataUrl = Url.Action("Fornecedor", "AutoComplete"),
                LabelId = "fornecedorNome",
                DataUrlPost = Url.Action("PostFornecedor")
            }, ResourceHashConst.ComprasCadastrosFornecedores));

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });
            #endregion

            #region step Produtos
            config.Elements.Add(new ButtonUI
            {
                Id = "btnAddPedidoItem",
                Class = "col s12 m3",
                Value = "Adicionar produto",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "click", Function = "fnModalPedidoItem" }
                }
            });
            config.Elements.Add(new ButtonUI
            {
                Id = "btnOrdemVendaServicoKit",
                Class = "col s12 m3",
                Label = "",
                Value = "Adicionar kit",
                DomEvents = new List<DomEventUI>
                    {
                        new DomEventUI { DomEvent = "click", Function = "fnModalPedidoKit" }
                    }
            });
            config.Elements.Add(new DivElementUI { Id = "pedidoProdutos", Class = "col s12 visible" });
            #endregion

            #region step Financeiro
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "geraFinanceiro",
                Class = "col s12 m6 l3",
                Label = "Gerar financeiro",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnValidaCamposGeraFinanceiro" }
                }
            });
            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m6 l3", Label = "Data Vencimento" });
            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 m6",
                Label = "Forma Pagamento",
                DataUrl = Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "FormaPagamento"),
                DataPostField = "descricao"
            }, ResourceHashConst.ComprasCadastrosFormaPagamento));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 m4",
                Label = "Condição Parcelamento",
                DataUrl = Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CondicaoParcelamento"),
                DataPostField = "descricao"
            }, ResourceHashConst.ComprasCadastrosCondicoesParcelamento));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m4",
                Label = "Categoria",
                PreFilter = "tipoCarteira",
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = @Url.Action("NovaCategoriaDespesa")
            }, ResourceHashConst.ComprasCadastrosCategoria));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "centroCustoId",
                Class = "col s12 m4",
                Label = "Centro de Custo",
                DataUrl = Url.Action("CentroCusto", "AutoComplete"),
                LabelId = "centroCustoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CentroCusto"),
                DataPostField = "descricao"
            }, ResourceHashConst.ComprasCadastrosCentroCustos));

            #endregion

            #region step Transporte
            if (!exibirTransportadora)
            {
                config?.Steps?.Remove(config?.Steps?.Find(x => x.Id == "stepTransporte"));
            }
            else
            {
                config.Elements.Add(new SelectUI
                {
                    Id = "tipoFrete",
                    Class = "col s12 m4",
                    Label = "Tipo Frete",
                    Required = true,
                    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoFrete))),
                    DomEvents = new List<DomEventUI>
                    {
                        new DomEventUI { DomEvent = "change", Function = "fnChangeFrete" }
                    }
                });
                config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
                {
                    Id = "transportadoraId",
                    Class = "col s12 m8",
                    Label = "Transportadora",
                    DataUrl = Url.Action("Transportadora", "AutoComplete"),
                    LabelId = "transportadoraNome",
                    DataUrlPost = Url.Action("PostTransportadora")
                }, ResourceHashConst.ComprasCadastrosTransportadora));

                config.Elements.Add(new InputCustommaskUI
                {
                    Id = "placaVeiculo",
                    Class = "col s12 m4",
                    Label = "Placa Veículo",
                    Data = new { inputmask = "'mask':'AAA[-9999]|[9A99]', 'showMaskOnHover': false, 'autoUnmask':true, 'greedy':true" }
                });
                config.Elements.Add(new AutoCompleteUI
                {
                    Id = "estadoPlacaVeiculoId",
                    Class = "col s12 m4",
                    Label = "UF Placa Veículo",
                    DataUrl = Url.Action("Estado", "AutoComplete"),
                    LabelId = "estadoPlacaVeiculoNome"
                });
                config.Elements.Add(new InputCurrencyUI
                {
                    Id = "valorFrete",
                    Class = "col s12 m4",
                    Label = "Valor Frete",
                    Value = "0",
                    DomEvents = new List<DomEventUI>
                    {
                        new DomEventUI { DomEvent = "change", Function = "fnChangeFrete" }
                    }
                });
                config.Elements.Add(new InputTextUI { Id = "marca", Class = "col s12 m4", Label = "Marca", MaxLength = 60 });
                config.Elements.Add(new InputFloatUI { Id = "pesoBruto", Class = "col s12 m4", Label = "Peso Bruto", Digits = 3, MaxLength = 8 });
                config.Elements.Add(new InputFloatUI { Id = "pesoLiquido", Class = "col s12 m4", Label = "Peso Líquido", Digits = 3, MaxLength = 8 });
                config.Elements.Add(new InputNumbersUI { Id = "quantidadeVolumes", Class = "col s12 m4", Label = "Quantidade Volumes", Value = "0" });
                config.Elements.Add(new InputTextUI { Id = "tipoEspecie", Class = "col s12 m4", Label = "Tipo Espécie", MaxLength = 60 });
                config.Elements.Add(new InputTextUI { Id = "numeracaoVolumesTrans", Class = "col s12 m4", Label = "Numeração", MaxLength = 60 });
            }
            #endregion

            #region step Finalizar
            var stepFinalizar = config?.Steps?.Find(x => x.Id == "stepFinalizar");
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m4", Label = "Total produtos", Readonly = true });
            if (emiteNotaFiscal)
            {
                stepFinalizar.Quantity += 2;
                config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutos", Class = "col s12 m4", Label = "Total de impostos incidentes", Readonly = true });
                config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutosNaoAgrega", Class = "col s12 m4", Label = "Total de impostos não incidentes", Readonly = true });
            }
            if (exibirTransportadora)
            {
                stepFinalizar.Quantity += 1;
                config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m4", Label = "Frete", Readonly = true });
            }
            config.Elements.Add(new InputCurrencyUI { Id = "totalOrdemCompra", Class = "col s12 m4", Label = "Total pedido", Readonly = true });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "movimentaEstoque",
                Class = "col s12 m4",
                Label = "Movimentar estoque",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI{DomEvent = "click", Function = "fnToggleMovimentaEstoque" }
                }
            });
            if (emiteNotaFiscal)
            {
                stepFinalizar.Quantity += 1;
                config.Elements.Add(new InputCheckboxUI
                {
                    Id = "geraNotaFiscal",
                    Class = "col s12 m4",
                    Label = "Faturar",
                    DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "click", Function = "fnClickGeraNotaFiscal" }
                }
                });
            }
            config.Elements.Add(new InputCheckboxUI { Id = "finalizarPedido", Class = "col s12 m4", Label = "Salvar e Finalizar" });
            config.Elements.Add(new InputTextUI { Id = "naturezaOperacao", Class = "col s12", Label = "Natureza de Operação", MaxLength = 60 });
            config.Elements.Add(new TextAreaUI { Id = "mensagemPadraoNota", Class = "col s12", Label = "Informações Adicionais", MaxLength = 4000 });
            config.Elements.Add(new DivElementUI { Id = "infoEstoqueNegativo", Class = "col s12 text-justify", Label = "Informação" });
            config.Elements.Add(new LabelSetUI { Id = "produtosEstoqueNegativoLabel", Class = "col s8", Label = "Produtos com estoque faltante" });
            config.Elements.Add(new InputCheckboxUI { Id = "ajusteEstoqueAutomatico", Class = "col s4", Label = "Ajustar negativo" });
            config.Elements.Add(new DivElementUI { Id = "produtosEstoqueNegativo", Class = "col s12 visible" });
            #endregion

            #region Helpers
            config.Helpers.Add(new TooltipUI
            {
                Id = "movimentaEstoque",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Movimentar Estoque, serão realizadas as movimentações de entrada da quantidade total dos produtos."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "finalizarPedido",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Salvar e Finalizar, serão efetivadas as opções marcadas (Gerar financeiro, Movimentar estoque). Não será mais possível editar ou excluir este pedido."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "transportadoraId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Informe a transportadora, quando configurar frete a ser pago por sua empresa (FOB/Destinatário)."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "geraFinanceiro",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Gerar Financeiro, serão criadas contas a Pagar ao fornecedor, e conta a Pagar a transportadora do valor de frete, se for configurado por conta da sua empresa."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "mensagemPadraoNota",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Ao transmitir a NF-e, além das informações aqui digitadas, será gerado automaticamente para o xml, as informações de IBPT e do aproveitamento de crédito de ICMS de acordo ao ARTIGO 23 DA LC 123 (Para CSOSN 101, 201 ou 900, conforme cadastro do Grupo Tributário em cada produto do pedido)."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "naturezaOperacao",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Faturar, informe a natureza de operação para a nota fiscal a ser emitida. Quando for um novo pedido, o sistema aplica a descrição do cfop configurado no grupo tributário do primeiro produto adicionado. Confirme e altere se necessário."
                }
            });
            #endregion

            cfg.Content.Add(config);
            cfg.Content.Add(GetDtPedidoItensCfg());
            cfg.Content.Add(GetDtProdutosEstoqueNegativoCfg());
            return cfg;
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public JsonResult FinalizarPedido(string id, bool faturar = false)
        {
            try
            {

                dynamic pedido = new ExpandoObject();
                pedido.status = "Finalizado";
                if (faturar)
                {
                    pedido.geraNotaFiscal = true;
                }

                var resourceNamePut = $"Pedido/{id}";
                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(pedido, JsonSerializerSetting.Edit));

                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Edit);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public ContentResult Visualizar()
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
            var exibirTransportadora = personalizacao != null ? personalizacao.ExibirStepTransportadoraCompras : true;            

            ModalUIForm config = new ModalUIForm()
            {
                Title = "Visualizar",
                UrlFunctions = @Url.Action("Functions", "Pedido") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                ReadyFn = "fnFormReadyVisualizarPedido",
                Id = "fly01mdlfrmVisualizarPedido",
                Functions = new List<string>() { "fnChangeCollapseExpand" }
            };

            config.Elements.Add(new ButtonGroupUI()
            {
                Id = "fly01btngrpExpandCollapse",
                Class = "col s12 m12",
                OnClickFn = "fnChangeCollapseExpand",
                Label = "Tipo do fator de conversão",
                Options = new List<ButtonGroupOptionUI>
                {
                    new ButtonGroupOptionUI { Id = "btnExpandAll", Value = "expandAll", Label = "Exibir todos"},
                    new ButtonGroupOptionUI { Id = "btnCollapseAll", Value = "collapseAll", Label = "Recolher todos"},
                }
            });

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "emiteNotaFiscal", Value = emiteNotaFiscal.ToString() });
            config.Elements.Add(new InputHiddenUI { Id = "exibeStepTransportadora", Value = exibirTransportadora.ToString() });

            #region Cadastro
            config.Elements.Add(new DivElementUI { Id = "collapseCadastro", Class = "col s12 visible" });

            config.Elements.Add(new InputNumbersUI { Id = "numero", Class = "col s12 m4", Label = "Número", Disabled = true });
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m4", Label = "Data", Disabled = true });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoCompra",
                Class = "col s12 m4",
                Label = "Tipo Compra",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCompraVenda))),
            });

            if (emiteNotaFiscal)
            {
                config.Elements.Add(new InputCheckboxUI { Id = "nFeRefComplementarIsDevolucao", Class = "col s12 m4", Label = "NF Referenciada é de Devolução", Disabled = true });
                config.Elements.Add(new InputNumbersUI { Id = "chaveNFeReferenciada", Class = "col s12 m4", Label = "Chave SEFAZ Nota Fiscal Referenciada", Disabled = true });
            }
            config.Elements.Add(new InputHiddenUI { Id = "tipoNfeComplementar" });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "fornecedorId",
                Class = "col s12",
                Label = "Fornecedor",
                Disabled = true,
                DataUrl = Url.Action("Fornecedor", "AutoComplete"),
                LabelId = "fornecedorNome"
            });
            config.Elements.Add(new TextAreaUI
            {
                Id = "observacao",
                Class = "col s12",
                Label = "Observação",
                MaxLength = 200,
                Disabled = true
            });
            #endregion

            #region Produtos
            config.Elements.Add(new DivElementUI { Id = "collapseProdutos", Class = "col s12 visible" });
            config.Elements.Add(new TableUI
            {
                Id = "PedidoItemDataTable",
                Class = "col s12",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Produto", Value = "0"},
                    new OptionUI { Label = "GrupoTributário", Value = "0"},
                    new OptionUI { Label = "Quant.", Value = "1"},
                    new OptionUI { Label = "Valor",Value = "2"},
                    new OptionUI { Label = "Desconto",Value = "3"},
                    new OptionUI { Label = "Total",Value = "4"},
                }
            });
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutosDt", Class = "col s12 m4 right", Label = "Total Produtos", Readonly = true });
            #endregion

            #region Financeiro
            config.Elements.Add(new DivElementUI { Id = "collapseFinanceiro", Class = "col s12 visible" });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 m6",
                Label = "Forma Pagamento",
                Disabled = true,
                DataUrl = Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 m6",
                Label = "Condição Parcelamento",
                Disabled = true,
                DataUrl = Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m6",
                Label = "Categoria",
                Disabled = true,
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao",
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "centroCustoId",
                Class = "col s12 m6",
                Label = "Centro de Custo",
                Disabled = true,
                DataUrl = @Url.Action("CentroCusto", "AutoComplete"),
                LabelId = "centroCustoDescricao",
            });
            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m6", Label = "Data Vencimento", Disabled = true });
            #endregion

            #region Transporte
            if (exibirTransportadora)
            {
                config.Elements.Add(new DivElementUI { Id = "collapseTransporte", Class = "col s12 visible" });
                config.Elements.Add(new SelectUI
                {
                    Id = "tipoFrete",
                    Class = "col s12 m6",
                    Label = "Tipo Frete",
                    Value = "SemFrete",
                    Disabled = true,
                    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoFrete))),
                });
                config.Elements.Add(new AutoCompleteUI
                {
                    Id = "transportadoraId",
                    Class = "col s12 m6",
                    Label = "Transportadora",
                    Disabled = true,
                    DataUrl = Url.Action("Transportadora", "AutoComplete"),
                    LabelId = "transportadoraNome"
                });
                config.Elements.Add(new InputCustommaskUI
                {
                    Id = "placaVeiculo",
                    Class = "col s12 m4",
                    Label = "Placa Veículo",
                    Disabled = true,
                    Data = new { inputmask = "'mask':'AAA[-9999]|[9A99]', 'showMaskOnHover': false, 'autoUnmask':true, 'greedy':true" }
                });
                config.Elements.Add(new AutoCompleteUI
                {
                    Id = "estadoPlacaVeiculoId",
                    Class = "col s12 m4",
                    Label = "UF Placa Veículo",
                    Disabled = true,
                    DataUrl = Url.Action("Estado", "AutoComplete"),
                    LabelId = "estadoPlacaVeiculoNome"
                });
                config.Elements.Add(new InputCurrencyUI { Id = "valorFrete", Class = "col s12 m4", Label = "Valor Frete", Disabled = true });
                config.Elements.Add(new InputFloatUI { Id = "pesoBruto", Class = "col s12 m4", Label = "Peso Bruto", Digits = 3, Disabled = true });
                config.Elements.Add(new InputTextUI { Id = "marca", Class = "col s12 m4", Label = "Marca", Disabled = true, MaxLength = 60 });
                config.Elements.Add(new InputFloatUI { Id = "pesoLiquido", Class = "col s12 m4", Label = "Peso Líquido", Digits = 3, Disabled = true });
                config.Elements.Add(new InputNumbersUI { Id = "quantidadeVolumes", Class = "col s12 m4", Label = "Quantidade Volumes", Disabled = true });
                config.Elements.Add(new InputTextUI { Id = "tipoEspecie", Class = "col s12 m4", Label = "Tipo Espécie", Disabled = true, MaxLength = 60 });
                config.Elements.Add(new InputTextUI { Id = "numeracaoVolumesTrans", Class = "col s12 m4", Label = "Numeração", Disabled = true, MaxLength = 60 });
            }
            else
            {
                config.Elements.Add(new InputHiddenUI() { Id = "tipoFrete" });
                config.Elements.Add(new InputHiddenUI() { Id = "valorFrete" });
            }
            #endregion

            #region Totais
            config.Elements.Add(new DivElementUI { Id = "collapseTotais", Class = "col s12 visible" });
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m4", Label = "Total produtos", Readonly = true });
            if (emiteNotaFiscal)
            {
                config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutos", Class = "col s12 m4", Label = "Total impostos produtos incidentes", Readonly = true });
                config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutosNaoAgrega", Class = "col s12 m4", Label = "Total de impostos não incidentes", Readonly = true });
            }

            if (exibirTransportadora)
            {
                config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m4", Label = "Frete a pagar", Readonly = true });
            }
            config.Elements.Add(new InputCurrencyUI { Id = "totalOrdemCompra", Class = "col s12 m4", Label = "Total", Readonly = true });

            if (emiteNotaFiscal)
            {
                config.Elements.Add(new InputTextUI { Id = "naturezaOperacao", Class = "col s12", Label = "Natureza de Operação", Disabled = true });
            }

            config.Elements.Add(new InputCheckboxUI { Id = "movimentaEstoque", Class = "col s12 m4", Label = "Movimenta estoque", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "geraNotaFiscal", Class = "col s12 m4", Label = "Faturar", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "geraFinanceiro", Class = "col s12 m4", Label = "Gera financeiro", Disabled = true });
            #endregion
            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [HttpPost]
        public override JsonResult Create(PedidoVM entityVM)
        {
            try
            {
                var postResponse = RestHelper.ExecutePostRequest(ResourceName, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                PedidoVM postResult = JsonConvert.DeserializeObject<PedidoVM>(postResponse);
                //return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, postResult.Id);
                var response = new JsonResult();
                response.Data = new { success = true, message = AppDefaults.EditSuccessMessage, id = postResult.Id.ToString(), numero = postResult.Numero.ToString() };
                return (response);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpPost]
        public override JsonResult Edit(PedidoVM entityVM)
        {
            return base.Edit(entityVM);
        }

        [HttpGet]
        public JsonResult TotalPedidoItens(string id)
        {
            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.AddParam("$filter", $"pedidoId eq {id} and ativo eq true");

                var total = RestHelper.ExecuteGetRequest<ResultBase<PedidoItemVM>>("PedidoItem", queryString).Data.Sum(x => x.Total);

                return Json(
                    new { totalPedidoItens = total, success = true },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public List<CondicaoParcelamentoParcelaVM> GetSimulacaoContas(PedidoVM pedido)
        {
            var dadosReferenciaSimulacao = new
            {
                valorReferencia = TotalOrdemCompraResponse(pedido.Id.ToString(), pedido.FornecedorId.ToString(), pedido.GeraNotaFiscal, pedido.TipoCompra, pedido.TipoFrete, pedido.ValorFrete)?.Total,
                dataReferencia = pedido?.DataVencimento,
                condicoesParcelamento = pedido?.CondicaoParcelamento?.CondicoesParcelamento,
                qtdParcelas = pedido.CondicaoParcelamento?.QtdParcelas
            };
            return RestHelper.ExecutePostRequest<ResponseSimulacaoVM>("condicaoparcelamentosimulacao", dadosReferenciaSimulacao)?.Items;
        }

        public virtual ActionResult ImprimirPedido(Guid id)
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
            var exibirTransportadora = personalizacao != null ? personalizacao.ExibirStepTransportadoraCompras : true;

            PedidoVM Pedido = Get(id);

            var simulacao = GetSimulacaoContas(Pedido);
            var parcelas = "";

            for (var i = 0; i < simulacao.Count; i++)
            {
                parcelas += $"{simulacao[i].DescricaoParcela} - Vencimento {simulacao[i].DataVencimento.ToString("dd/MM/yyyy")} - {simulacao[i].Valor.ToString("C", AppDefaults.CultureInfoDefault)}    ";
                if (i % 2 != 0 && i > 0 && i < (simulacao.Count - 1))
                {
                    parcelas += "\n";
                }
            }

            var produtos = GetProdutos(id);
            List<ImprimirPedidoVM> reportItems = new List<ImprimirPedidoVM>();

            if (!produtos.Any())
                AdicionarInformacoesPadrao(Pedido, reportItems, emiteNotaFiscal, exibirTransportadora, parcelas);
            else
                MontarItensParaPrint(Pedido, produtos, reportItems, emiteNotaFiscal, exibirTransportadora, parcelas);

            var reportViewer = new WebReportViewer<ImprimirPedidoVM>(ReportImprimirPedido.Instance);
            return File(reportViewer.Print(reportItems, SessionManager.Current.UserData.PlatformUrl), "application/pdf");
        }

        public List<PedidoItemVM> GetProdutos(Guid id)
        {
            var queryString = new Dictionary<string, string>();
            queryString.AddParam("$filter", $"pedidoId eq {id}");
            queryString.AddParam("$expand", "produto");

            return RestHelper.ExecuteGetRequest<ResultBase<PedidoItemVM>>("PedidoItem", queryString).Data;
        }

        [HttpGet]
        public JsonResult GetInformacoesComplementares()
        {
            try
            {
                var response = RestHelper.ExecuteGetRequest<ParametroTributarioVM>("parametrotributario");

                return Json(
                    new { success = true, infcomp = response?.MensagemPadraoNota },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public JsonResult VerificaEstoqueNegativo(string id, string tipoCompra)
        {
            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.AddParam("pedidoId", id);
                queryString.AddParam("tipoCompra", tipoCompra);

                var response = RestHelper.ExecuteGetRequest<List<PedidoEstoqueNegativoVM>>("PedidoEstoqueNegativo", queryString);

                return Json(new
                {
                    success = true,
                    recordsTotal = response.Count,
                    recordsFiltered = response.Count,
                    produtosEstoqueNegativo = response,
                    data = response.Select(GetDisplayDataPedidoEstoqueNegativo())
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        protected Func<PedidoEstoqueNegativoVM, object> GetDisplayDataPedidoEstoqueNegativo()
        {
            return x => new
            {
                produtoId = x.ProdutoId,
                quantEstoque = Math.Round(x.QuantEstoque, 2, MidpointRounding.AwayFromZero),
                quantPedido = Math.Round(x.QuantPedido, 2, MidpointRounding.AwayFromZero),
                saldoEstoque = Math.Round(x.SaldoEstoque, 2, MidpointRounding.AwayFromZero),
                produtoDescricao = x.ProdutoDescricao
            };
        }

        private TotalPedidoNotaFiscalVM TotalOrdemCompraResponse(string id, string fornecedorId, bool geraNotaFiscal, string tipoCompra, string tipoFrete, double? valorFrete = 0)
        {
            var resource = string.Format("CalculaTotalOrdemCompra?&ordemCompraId={0}&fornecedorId={1}&geraNotaFiscal={2}&tipoCompra={3}&tipoFrete={4}&valorFrete={5}&onList={6}", id, fornecedorId, geraNotaFiscal.ToString(), tipoCompra, tipoFrete, valorFrete.ToString().Replace(",", "."), false);
            var response = RestHelper.ExecuteGetRequest<TotalPedidoNotaFiscalVM>(resource, queryString: null);
            return response;
        }

        [HttpGet]
        public JsonResult TotalOrdemCompra(string id, string fornecedorId, bool geraNotaFiscal, string tipoCompra, string tipoFrete, double? valorFrete = 0)
        {
            try
            {
                var response = TotalOrdemCompraResponse(id, fornecedorId, geraNotaFiscal, tipoCompra, tipoFrete, valorFrete);
                return Json(
                    new { success = true, total = response },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public JsonResult EnviaEmailPedido(string id)
        {
            try
            {
                var empresa = GetDadosEmpresa();
                var pedido = Get(Guid.Parse(id));

                var ResponseError = ValidarDadosEmail(id, empresa, pedido);
                if (ResponseError != null) return ResponseError; // tem que arrumar uma solução para o retorno nulo

                MailSend(empresa, pedido);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        private JsonResult ValidarDadosEmail(string id, ManagerEmpresaVM empresa, PedidoVM pedido)
        {

            if (pedido.Fornecedor == null) return JsonResponseStatus.GetFailure("Nenhum Fornecedor foi encontrado.");
            if (string.IsNullOrEmpty(pedido.Fornecedor.Email)) return JsonResponseStatus.GetFailure("Não foi encontrado um email válido para este Fornecedor.");
            if (string.IsNullOrEmpty(empresa.Email)) return JsonResponseStatus.GetFailure("Você ainda não configurou um email válido para sua empresa.");

            return null;
        }

        private void MailSend(ManagerEmpresaVM empresa, PedidoVM pedido)
        {
            var anexo = File(GetPDFFile(pedido), "application/pdf");
            var mensagemPrincipal = $"Você está recebendo uma cópia de seu {pedido.TipoOrdemCompra}.".ToUpper();
            var tituloEmail = $"{empresa.NomeFantasia} {pedido.TipoOrdemCompra} - Nº {pedido.Numero}".ToUpper();
            var conteudoEmail = Mail.FormataMensagem(EmailFilesHelper.GetTemplate("Templates.OrdemCompra.html").Value, tituloEmail, mensagemPrincipal, empresa.Email);
            var arquivoAnexo = new FileStreamResult(new MemoryStream(anexo.FileContents), anexo.ContentType);

            Mail.Send(empresa.NomeFantasia, pedido.Fornecedor.Email, tituloEmail, conteudoEmail, arquivoAnexo.FileStream);
        }

        private byte[] GetPDFFile(PedidoVM pedido)
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
            var exibirTransportadora = personalizacao != null ? personalizacao.ExibirStepTransportadoraCompras : true;

            var produtos = GetProdutos(pedido.Id);
            List<ImprimirPedidoVM> reportItems = new List<ImprimirPedidoVM>();

            var simulacao = GetSimulacaoContas(pedido);
            var parcelas = "";

            for (var i = 0; i < simulacao.Count; i++)
            {
                parcelas += $"{simulacao[i].DescricaoParcela} - Vencimento {simulacao[i].DataVencimento.ToString("dd/MM/yyyy")} - {simulacao[i].Valor.ToString("C", AppDefaults.CultureInfoDefault)}    ";
                if (i % 2 != 0 && i > 0 && i < (simulacao.Count - 1))
                {
                    parcelas += "\n";
                }
            }

            if (!produtos.Any())
                AdicionarInformacoesPadrao(pedido, reportItems, emiteNotaFiscal, exibirTransportadora, parcelas);
            else
                MontarItensParaPrint(pedido, produtos, reportItems, emiteNotaFiscal, exibirTransportadora, parcelas);

            var reportViewer = new WebReportViewer<ImprimirPedidoVM>(ReportImprimirPedido.Instance);
            return reportViewer.Print(reportItems, SessionManager.Current.UserData.PlatformUrl);
        }

        private static void MontarItensParaPrint(PedidoVM Pedido, List<PedidoItemVM> produtos, List<ImprimirPedidoVM> reportItems, bool emiteNotaFiscal, bool exibirTransportadora, string parcelas)
        {
            foreach (PedidoItemVM produtospedido in produtos)
            {
                var resource = string.Format("CalculaTotalOrdemCompra?&ordemCompraId={0}&fornecedorId={1}&geraNotaFiscal={2}&tipoCompra={3}&tipoFrete={4}&valorFrete={5}&onList={6}", Pedido.Id, Pedido.FornecedorId, Pedido.GeraNotaFiscal.ToString(), Pedido.TipoCompra, Pedido.TipoFrete, Pedido.ValorFrete.ToString().Replace(",", "."), true);
                var response = RestHelper.ExecuteGetRequest<TotalPedidoNotaFiscalVM>(resource, queryString: null);
                reportItems.Add(new ImprimirPedidoVM
                {
                    //PEDIDO
                    ComplementoEndereco = Pedido.Fornecedor != null && Pedido.Fornecedor.Complemento != null ? Pedido.Fornecedor.Complemento.ToString() : string.Empty,
                    EnderecoFornecedor = Pedido.Fornecedor != null && Pedido.Fornecedor.Endereco != null ? Pedido.Fornecedor.Endereco.ToString() : string.Empty,
                    NumeroEndereco = Pedido.Fornecedor != null && Pedido.Fornecedor.Numero != null ? Pedido.Fornecedor.Numero.ToString() : string.Empty,
                    Bairro = Pedido.Fornecedor != null && Pedido.Fornecedor.Bairro != null ? Pedido.Fornecedor.Bairro.ToString() : string.Empty,
                    Cidade = Pedido.Fornecedor != null && Pedido.Fornecedor.Cidade != null ? Pedido.Fornecedor.Cidade.Nome.ToString() : string.Empty,
                    Estado = Pedido.Fornecedor != null && Pedido.Fornecedor.Estado != null ? Pedido.Fornecedor.Estado.Sigla.ToString() : string.Empty,
                    Pais = Pedido.Fornecedor != null && Pedido.Fornecedor.Pais != null ? Pedido.Fornecedor.Pais.Nome.ToString() : string.Empty,
                    CEP = Pedido.Fornecedor != null && Pedido.Fornecedor.CEP != null ? Pedido.Fornecedor.CEP.ToString() : string.Empty,
                    Fornecedor = Pedido.Fornecedor != null ? Pedido.Fornecedor.Nome.ToString() : string.Empty,
                    Categoria = Pedido.Categoria != null ? Pedido.Categoria.Descricao : string.Empty,
                    CondicaoParcelamento = Pedido.CondicaoParcelamento != null ? Pedido.CondicaoParcelamento.Descricao : string.Empty,
                    DataVencimento = Pedido.DataVencimento,
                    FormaPagamento = Pedido.FormaPagamento != null ? Pedido.FormaPagamento.Descricao : string.Empty,
                    Transportadora = Pedido.Transportadora != null ? Pedido.Transportadora.Nome : string.Empty,
                    Numero = Pedido.Numero,
                    Observacao = Pedido.Observacao,
                    PesoBruto = Pedido.PesoBruto != null ? Pedido.PesoBruto : 0,
                    PesoLiquido = Pedido.PesoLiquido != null ? Pedido.PesoLiquido : 0,
                    ValorFrete = (exibirTransportadora && Pedido.ValorFrete.HasValue) ? Pedido.ValorFrete : 0,
                    TipoFrete = EnumHelper.GetValue(typeof(TipoFrete),Pedido.TipoFrete),
                    QuantVolumes = Pedido.QuantidadeVolumes != null ? Pedido.QuantidadeVolumes : 0,
                    Data = Pedido.Data,
                    Finalidade = Pedido.TipoCompra,
                    PlacaVeiculo = Pedido.PlacaVeiculo != null ? Pedido.PlacaVeiculo : "",
                    EstadoPlacaVeiculo = Pedido.EstadoPlacaVeiculo != null ? Pedido.EstadoPlacaVeiculo.Sigla.ToString() : "",
                    TipoEspecie = Pedido.TipoEspecie != null ? Pedido.TipoEspecie : "",
                    Marca = Pedido.Marca != null ? Pedido.Marca : "",
                    NumeracaoVolumesTrans = Pedido.NumeracaoVolumesTrans != null ? Pedido.NumeracaoVolumesTrans : "",
                    TotalImpostosProdutos = Pedido?.TotalImpostosProdutos ?? 0.0,
                    TotalGeral = Pedido.Total + (Pedido?.TotalImpostosProdutos ?? 0.0),
                    Status = Pedido.Status,
                    ParcelaContas = parcelas,
                    ValorFreteTotal = response.ValorFrete.HasValue ? response.ValorFrete.Value : 0,
                    //PRODUTO
                    Id = produtospedido.Id.ToString(),
                    NomeProduto = produtospedido.Produto != null ? produtospedido.Produto.Descricao : string.Empty,
                    QtdProduto = produtospedido.Quantidade,
                    ValorUnitario = produtospedido.Valor,
                    ValorTotal = produtospedido.Total,
                    ItemDesconto = produtospedido.Desconto,
                    ExibirTransportadora = exibirTransportadora.ToString(),
                    EmiteNotaFiscal = emiteNotaFiscal.ToString()
                    
                });
            }
        }

        private static void AdicionarInformacoesPadrao(PedidoVM Pedido, List<ImprimirPedidoVM> reportItems, bool emiteNotaFiscal, bool exibirTransportadora, string parcelas)
        {
            var resource = string.Format("CalculaTotalOrdemCompra?&ordemCompraId={0}&fornecedorId={1}&geraNotaFiscal={2}&tipoCompra={3}&tipoFrete={4}&valorFrete={5}&onList={6}", Pedido.Id, Pedido.FornecedorId, Pedido.GeraNotaFiscal.ToString(), Pedido.TipoCompra, Pedido.TipoFrete, Pedido.ValorFrete.ToString().Replace(",", "."), true);
            var response = RestHelper.ExecuteGetRequest<TotalPedidoNotaFiscalVM>(resource, queryString: null);
            reportItems.Add(new ImprimirPedidoVM
            {
                //PEDIDO
                Estado = Pedido.Fornecedor != null && Pedido.Fornecedor.Estado != null ? Pedido.Fornecedor.Estado.Sigla.ToString() : string.Empty,
                Pais = Pedido.Fornecedor != null && Pedido.Fornecedor.Pais != null ? Pedido.Fornecedor.Pais.Nome.ToString() : string.Empty,
                ComplementoEndereco = Pedido.Fornecedor != null && Pedido.Fornecedor.Complemento != null ? Pedido.Fornecedor.Complemento.ToString() : string.Empty,
                EnderecoFornecedor = Pedido.Fornecedor != null && Pedido.Fornecedor.Endereco != null ? Pedido.Fornecedor.Endereco.ToString() : string.Empty,
                NumeroEndereco = Pedido.Fornecedor != null && Pedido.Fornecedor.Numero != null ? Pedido.Fornecedor.Numero.ToString() : string.Empty,
                Bairro = Pedido.Fornecedor != null && Pedido.Fornecedor.Bairro != null ? Pedido.Fornecedor.Bairro.ToString() : string.Empty,
                Cidade = Pedido.Fornecedor != null && Pedido.Fornecedor.Cidade != null ? Pedido.Fornecedor.Cidade.Nome.ToString() : string.Empty,
                CEP = Pedido.Fornecedor != null && Pedido.Fornecedor.CEP != null ? Pedido.Fornecedor.CEP.ToString() : string.Empty,
                Fornecedor = Pedido.Fornecedor != null ? Pedido.Fornecedor.Nome.ToString() : string.Empty,
                Categoria = Pedido.Categoria != null ? Pedido.Categoria.Descricao : string.Empty,
                CondicaoParcelamento = Pedido.CondicaoParcelamento != null ? Pedido.CondicaoParcelamento.Descricao : string.Empty,
                DataVencimento = Pedido.DataVencimento,
                FormaPagamento = Pedido.FormaPagamento != null ? Pedido.FormaPagamento.Descricao : string.Empty,
                Transportadora = Pedido.Transportadora != null ? Pedido.Transportadora.Nome : string.Empty,
                Numero = Pedido.Numero,
                Observacao = Pedido.Observacao,
                PesoBruto = Pedido.PesoBruto ?? 0,
                PesoLiquido = Pedido.PesoLiquido ?? 0,
                ValorFrete = (exibirTransportadora && Pedido.ValorFrete.HasValue) ? Pedido.ValorFrete : 0,
                TipoFrete = EnumHelper.GetValue(typeof(TipoFrete), Pedido.TipoFrete),
                TotalGeral = Pedido.Total + (Pedido?.TotalImpostosProdutos ?? 0.0),
                Data = Pedido.Data,
                Finalidade = Pedido.TipoCompra,
                PlacaVeiculo = Pedido.PlacaVeiculo,
                EstadoPlacaVeiculo = Pedido?.EstadoPlacaVeiculo?.Sigla?.ToString(),
                TipoEspecie = Pedido.TipoEspecie,
                Marca = Pedido.Marca,
                NumeracaoVolumesTrans = Pedido.NumeracaoVolumesTrans,
                TotalProdutos = Pedido.Total,
                TotalImpostosProdutos = Pedido.TotalImpostosProdutos,
                Status = Pedido.Status,
                ExibirTransportadora = exibirTransportadora.ToString(),
                EmiteNotaFiscal = emiteNotaFiscal.ToString(),
                ParcelaContas = parcelas,
                ValorFreteTotal = response.ValorFrete.HasValue ? response.ValorFrete.Value : 0
            });
        }

        public ContentResult ModalKit()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar Kit Produtos",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("AdicionarKit", "Pedido")
                },
                Id = "fly01mdlfrmPedidoKit",
                ReadyFn = "fnFormReadyPedidoKit"
            };
            config.Elements.Add(new InputHiddenUI { Id = "orcamentoPedidoId" });
            config.Elements.Add(new InputHiddenUI { Id = "adicionarProdutos", Value = "true" });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "kitId",
                Class = "col s12",
                Label = "Kit",
                Required = true,
                DataUrl = Url.Action("Kit", "AutoComplete"),
                LabelId = "kitDescricao",
            }, ResourceHashConst.ComprasCadastrosKit));

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "somarExistentes",
                Class = "col s12 m4",
                Label = "Somar com existentes"
            });
            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "grupoTributarioProdutoIdKit",
                Class = "col s12",
                Label = "Grupo Tributário Padrão",
                Name = "grupoTributarioProdutoId",
                DataUrl = Url.Action("GrupoTributario", "AutoComplete"),
                LabelId = "grupoTributarioProdutoDescricaoKit",
                LabelName = "grupoTributarioProdutoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "GrupoTributario"),
                DataPostField = "descricao"
            }, ResourceHashConst.ComprasCadastrosGrupoTributario));

            #region Helpers            
            config.Helpers.Add(new TooltipUI
            {
                Id = "kitId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Vai ser adicionado os produtos cadastrados no Kit."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "grupoTributarioProdutoIdKit",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se desejar, informe um grupo tributário padrão para todos os produtos do kit, que vão ser adicionados ao pedido."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "somarExistentes",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Os produtos cadastrados no kit, serão somados com a quantidade já existente no pedido."
                }
            });
            #endregion

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult AdicionarKit(UtilizarKitVM entityVM)
        {
            try
            {
                RestHelper.ExecutePostRequest("kitpedido", JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                return JsonResponseStatus.GetSuccess("Produtos do kit adicionados com sucesso.");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public JsonResult ClonarPedido(Guid id)
        {
            try
            {
                ExpandProperties = "";
                PedidoVM pedido = Get(id);
                pedido.Id = Guid.NewGuid();
                pedido.Status = Status.Aberto.ToString();
                pedido.Data = DateTime.Now;
                pedido.DataVencimento = DateTime.Now;
                var postResponse = RestHelper.ExecutePostRequest("Pedido", JsonConvert.SerializeObject(pedido, JsonSerializerSetting.Default));

                List<PedidoItemVM> produtos = GetProdutosPedido(id);
                foreach (var item in produtos)
                {
                    item.Id = Guid.NewGuid();
                    item.PedidoId = pedido.Id;
                    var postResponseProdutos = RestHelper.ExecutePostRequest<PedidoItemVM>("PedidoItem", JsonConvert.SerializeObject(item, JsonSerializerSetting.Default));
                }

                return Json(new
                {
                    success = true,
                    id = pedido.Id
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public List<PedidoItemVM> GetProdutosPedido(Guid id)
        {
            var queryString = new Dictionary<string, string>();

            queryString.AddParam("$filter", $"pedidoId eq {id}");

            return RestHelper.ExecuteGetRequest<ResultBase<PedidoItemVM>>("PedidoItem", queryString).Data;
        }

        #region OnDemmand
        [HttpPost]
        public JsonResult NovaCategoriaDespesa(string term)
        {
            try
            {
                var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));
                var data = RestHelper.ExecutePostRequest<CategoriaVM>(
                    resourceName,
                    new CategoriaVM { Descricao = term, TipoCarteira = "2" },
                    AppDefaults.GetQueryStringDefault()
                );

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public JsonResult PostFornecedor(string term)
        {
            var entity = new PessoaVM
            {
                Nome = term,
                Fornecedor = true,
                TipoIndicacaoInscricaoEstadual = "ContribuinteIsento",
                SituacaoEspecialNFS = "Outro"
            };

            NormarlizarEntidade(ref entity);

            try
            {
                var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
                var data = RestHelper.ExecutePostRequest<PessoaVM>(resourceName, entity, AppDefaults.GetQueryStringDefault());

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        private void NormarlizarEntidade(ref PessoaVM entityVM)
        {
            const string regexSomenteDigitos = @"[^\d]";

            entityVM.CPFCNPJ = Regex.Replace(entityVM.CPFCNPJ ?? "", regexSomenteDigitos, "");
            entityVM.TipoDocumento = GetTipoDocumento(entityVM.CPFCNPJ ?? "");
            entityVM.Celular = Regex.Replace(entityVM.Celular ?? "", regexSomenteDigitos, "");
            entityVM.Telefone = Regex.Replace(entityVM.Telefone ?? "", regexSomenteDigitos, "");
            entityVM.CEP = Regex.Replace(entityVM.CEP ?? "", regexSomenteDigitos, "");
        }

        public JsonResult PostTransportadora(string term)
        {
            var entity = new PessoaVM
            {
                Nome = term,
                Transportadora = true,
                TipoIndicacaoInscricaoEstadual = "ContribuinteIsento",
                SituacaoEspecialNFS = "Outro"
            };

            NormarlizarEntidade(ref entity);

            try
            {
                var resourceName = AppDefaults.GetResourceName(typeof(PessoaVM));
                var data = RestHelper.ExecutePostRequest<PessoaVM>(resourceName, entity, AppDefaults.GetQueryStringDefault());

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }
        #endregion
    }
}