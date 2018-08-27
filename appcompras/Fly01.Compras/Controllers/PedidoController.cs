﻿using Fly01.Compras.ViewModel;
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

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasOrcamentoPedido)]
    public class PedidoController : BaseController<PedidoVM>
    {
        //OrcamentoVM e PedidoVM na mesma controller ordemCompra(gridLoad, form), direcionado para a controller via javaScript
        public PedidoController()
        {
            ExpandProperties = "condicaoParcelamento,formaPagamento,fornecedor,transportadora,orcamentoOrigem,categoria";
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

        public ContentUI FormPedidoJson(bool isEdit = false)
        {
            var cfg = new ContentUI
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
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List", "OrdemCompra")
                },
                ReadyFn = "fnFormReadyPedido",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnChangeEstado" },
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
                        Quantity = 11,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Produtos",
                        Id = "stepProdutos",
                        Quantity = 2,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Financeiro",
                        Id = "stepFinanceiro",
                        Quantity = 5,
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
            #endregion

            #region step Cadastro
            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoCompra", Value = "Normal" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoCarteira", Value = "Despesa" });
            config.Elements.Add(new InputHiddenUI { Id = "status", Value = "Aberto" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoOrdemCompra", Value = "Pedido" });
            config.Elements.Add(new InputHiddenUI { Id = "grupoTributarioPadraoTipoTributacaoICMS" });
            config.Elements.Add(new InputNumbersUI { Id = "numero", Class = "col s12 m2", Label = "Número", Disabled = true });


            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m3", Label = "Data", Required = true });

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
                Class = "col s12 m2",
                Value = "Adicionar produto",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "click", Function = "fnModalPedidoItem" }
                }
            });
            config.Elements.Add(new DivElementUI { Id = "pedidoProdutos", Class = "col s12" });
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
                Class = "col s12 m6",
                Label = "Condição Parcelamento",
                DataUrl = Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CondicaoParcelamento"),
                DataPostField = "descricao"
            }, ResourceHashConst.ComprasCadastrosCondicoesParcelamento));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m6",
                Label = "Categoria",
                PreFilter = "tipoCarteira",
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = @Url.Action("NovaCategoriaDespesa")
            }, ResourceHashConst.ComprasCadastrosCategoria));

            #endregion

            #region step Transporte
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
                Data = new { inputmask = "'mask':'AAA-9999', 'showMaskOnHover': false, 'autoUnmask':true" }
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
            #endregion

            #region step Finalizar
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m4", Label = "Total produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutos", Class = "col s12 m4", Label = "Total de impostos incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutosNaoAgrega", Class = "col s12 m4", Label = "Total de impostos não incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m6", Label = "Frete a pagar", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalOrdemCompra", Class = "col s12 m6", Label = "Total pedido (produtos + impostos + frete)", Readonly = true });
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
            config.Elements.Add(new InputCheckboxUI { Id = "finalizarPedido", Class = "col s12 m4", Label = "Salvar e Finalizar" });
            config.Elements.Add(new InputTextUI { Id = "naturezaOperacao", Class = "col s12", Label = "Natureza de Operação", MaxLength = 60 });
            config.Elements.Add(new TextAreaUI { Id = "mensagemPadraoNota", Class = "col s12", Label = "Informações Adicionais", MaxLength = 4000 });
            config.Elements.Add(new DivElementUI { Id = "infoEstoqueNegativo", Class = "col s12 text-justify", Label = "Informação" });
            config.Elements.Add(new LabelSetUI { Id = "produtosEstoqueNegativoLabel", Class = "col s8", Label = "Produtos com estoque faltante" });
            config.Elements.Add(new InputCheckboxUI { Id = "ajusteEstoqueAutomatico", Class = "col s4", Label = "Ajustar negativo" });
            config.Elements.Add(new DivElementUI { Id = "produtosEstoqueNegativo", Class = "col s12" });
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
                    Text = "Informe a transportadora, quando configurar frete a ser pago por sua empresa(FOB/Destinatário)."
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
            #endregion

            cfg.Content.Add(config);
            cfg.Content.Add(GetDtPedidoItensCfg());
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
                Id = "fly01mdlfrmVisualizarPedido"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputNumbersUI { Id = "numero", Class = "col s12 m6 l2", Label = "Número", Disabled = true });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoCompra",
                Class = "col s12 m6 l4",
                Label = "Tipo Compra",
                //Value = "Normal",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoVenda))),
            });
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m6 l2", Label = "Data", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "nFeRefComplementarIsDevolucao", Class = "col s12 m6 l4", Label = "NF Referenciada é de Devolução", Disabled = true });


            config.Elements.Add(new InputNumbersUI { Id = "chaveNFeReferenciada", Class = "col s12 m6", Label = "Chave SEFAZ Nota Fiscal Referenciada", Disabled = true });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "fornecedorId",
                Class = "col s12 m6",
                Label = "Fornecedor",
                Disabled = true,
                DataUrl = Url.Action("Fornecedor", "AutoComplete"),
                LabelId = "fornecedorNome"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "grupoTributarioPadraoId",
                Class = "col s12 m6",
                Label = "Grupo Tributário Padrão",
                Disabled = true,
                DataUrl = Url.Action("GrupoTributario", "AutoComplete"),
                LabelId = "grupoTributarioPadraoDescricao"
            });
            config.Elements.Add(new TextAreaUI
            {
                Id = "observacao",
                Class = "col s12",
                Label = "Observação",
                MaxLength = 200,
                Disabled = true
            });

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
            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m6", Label = "Data Vencimento", Disabled = true });

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
                Data = new { inputmask = "'mask':'AAA-9999', 'showMaskOnHover': false, 'autoUnmask':true" }
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

            config.Elements.Add(new InputTextUI { Id = "naturezaOperacao", Class = "col s12", Label = "Natureza de Operação", Disabled = true });

            config.Elements.Add(new LabelSetUI { Id = "labelSetTotais", Class = "col s12", Label = "Totais" });
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m6", Label = "Total produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m6", Label = "Frete a pagar", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutos", Class = "col s12 m6", Label = "Total impostos produtos incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutosNaoAgrega", Class = "col s12 m6", Label = "Total de impostos não incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalOrdemCompra", Class = "col s12", Label = "Total (produtos + impostos + frete)", Readonly = true });
            config.Elements.Add(new InputCheckboxUI { Id = "movimentaEstoque", Class = "col s12 m4", Label = "Movimenta estoque", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "geraNotaFiscal", Class = "col s12 m4", Label = "Faturar", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "geraFinanceiro", Class = "col s12 m4", Label = "Gera financeiro", Disabled = true });

            config.Elements.Add(new LabelSetUI { Id = "labelSetProdutos", Class = "col s12", Label = "Produtos" });
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

        public virtual ActionResult ImprimirPedido(Guid id)
        {

            PedidoVM Pedido = Get(id);

            var produtos = GetProdutos(id);
            List<ImprimirPedidoVM> reportItems = new List<ImprimirPedidoVM>();

            foreach (PedidoItemVM produtospedido in produtos)

                reportItems.Add(new ImprimirPedidoVM
                {
                    //PEDIDO
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
                    ValorFrete = Pedido.ValorFrete != null ? Pedido.ValorFrete : 0,
                    TipoFrete = Pedido.TipoFrete,
                    QuantVolumes = Pedido.QuantidadeVolumes,
                    TotalGeral = Pedido.Total != null ? Pedido.Total : 0,
                    //PRODUTO
                    Id = produtospedido.Id.ToString(),
                    NomeProduto = produtospedido.Produto != null ? produtospedido.Produto.Descricao : string.Empty,
                    QtdProduto = produtospedido.Quantidade,
                    ValorUnitario = produtospedido.Valor,
                    ValorTotal = produtospedido.Total
                });

            if (!produtos.Any())
            {
                reportItems.Add(new ImprimirPedidoVM
                {
                    //PEDIDO
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
                    ValorFrete = Pedido.ValorFrete != null ? Pedido.ValorFrete : 0,
                    TipoFrete = Pedido.TipoFrete,
                    TotalGeral = Pedido.Total != null ? Pedido.Total : 0,
                });
            }

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
                var response = RestHelper.ExecuteGetRequest<ResultBase<ParametroTributarioVM>>("parametrotributario");

                return Json(
                    new { success = true, infcomp = response.Data.FirstOrDefault()?.MensagemPadraoNota },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpGet]
        public JsonResult TotalOrdemCompra(string id, string fornecedorId, bool geraNotaFiscal, string tipoCompra, string tipoFrete, double? valorFrete = 0)
        {
            try
            {
                var resource = string.Format("CalculaTotalOrdemCompra?&ordemCompraId={0}&fornecedorId={1}&geraNotaFiscal={2}&tipoCompra={3}&tipoFrete={4}&valorFrete={5}&onList={6}", id, fornecedorId, geraNotaFiscal.ToString(), tipoCompra, tipoFrete, valorFrete.ToString().Replace(",", "."), false);
                var response = RestHelper.ExecuteGetRequest<TotalOrdemVendaCompraVM>(resource, queryString: null);

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
                TipoIndicacaoInscricaoEstadual = "ContribuinteIsento"
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
                TipoIndicacaoInscricaoEstadual = "ContribuinteIsento"
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