﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Faturamento.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoFaturamentoVendas)]
    public class PedidoController : OrdemVendaController
    {
        //pedido e orçamento são ordem de venda, apenas a propriedade TipoOrdemVenda que muda
        //porém foi feito os controllers distintos para efeito front ao usuário

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
                    new DataTableUIParameter { Id = "tipoVenda", Required = true },
                    new DataTableUIParameter { Id = "tipoNfeComplementar", Required = true },
                    new DataTableUIParameter { Id = "nFeRefComplementarIsDevolucao", Required = true }
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

        public override Func<OrdemVendaVM, object> GetDisplayData() { throw new NotImplementedException(); }

        protected override ContentUI FormJson() { throw new NotImplementedException(); }

        public override ContentResult List() { throw new NotImplementedException(); }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelarPedido" });
            }

            return target;
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public ContentResult FormPedido(bool isEdit = false, string tipoVenda = "Normal")
        {
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
                    List = @Url.Action("List", "OrdemVenda")
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
                        Quantity = 3,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Cadastro",
                        Id = "stepCadastro",
                        Quantity = 12,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Produtos",
                        Id = "stepProdutos",
                        Quantity = 2,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Serviços",
                        Id = "stepServicos",
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
                        Quantity = 18,
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
                    new ButtonGroupOptionUI { Id = "btnComplementar", Value = "Complementar", Label = "Complementar"},
                }
            });
            config.Elements.Add(new InputNumbersUI { Id = "chaveNFeReferenciada", Class = "col s12 m8 offset-m2", Label = "Chave SEFAZ Nota Fiscal Referenciada", MinLength = 44, MaxLength = 44 });
            config.Elements.Add(new InputCheckboxUI {
                Id = "nFeRefComplementarIsDevolucao",
                Class = "col s12 m8 offset-m2",
                Label = "Nota Fiscal Referenciada é de Devolução",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnClickComplementarIsDevolucao" }
                }
            });
            #endregion

            #region step Cadastro

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoVenda", Value = tipoVenda });
            config.Elements.Add(new InputHiddenUI { Id = "tipoCarteira", Value = "Receita" });
            config.Elements.Add(new InputHiddenUI { Id = "status", Value = "Aberto" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoOrdemVenda", Value = "Pedido" });
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
            }, ResourceHashConst.FaturamentoCadastrosGrupoTributario));

            config.Elements.Add(new SelectUI
            {
                Id = "tipoNfeComplementar",
                Class = "col s12 m7",
                Label = "Tipo do Complemento",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoNfeComplementar), false, "NaoComplementar")
                    .ToList().FindAll(x => "NaoComplementar,ComplPrecoQtd,ComplIcms".Contains(x.Value))),
                ConstrainWidth = true
            });
            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "clienteId",
                Class = "col s12",
                Label = "Cliente",
                Required = true,
                DataUrl = Url.Action("Cliente", "AutoComplete"),
                LabelId = "clienteNome",
                DataUrlPost = Url.Action("PostCliente")
            }, ResourceHashConst.FaturamentoCadastrosClientes));

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });


            #endregion

            #region step Produtos
            config.Elements.Add(new ButtonUI
            {
                Id = "btnOrdemVendaProduto",
                Class = "col s12 m2",
                Label = "",
                Value = "Adicionar produto",
                DomEvents = new List<DomEventUI>
                    {
                        new DomEventUI { DomEvent = "click", Function = "fnModalOrdemVendaProduto" }
                    }
            });
            config.Elements.Add(new DivElementUI { Id = "ordemVendaProdutos", Class = "col s12 visible" });
            #endregion

            #region step Serviços
            config.Elements.Add(new ButtonUI
            {
                Id = "btnOrdemVendaServico",
                Class = "col s12 m2",
                Label = "",
                Value = "Adicionar serviço",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "click", Function = "fnModalOrdemVendaServico" }
                }
            });
            config.Elements.Add(new DivElementUI { Id = "ordemVendaServicos", Class = "col s12 visible" });
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
            }, ResourceHashConst.FaturamentoCadastrosFormasPagamento));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 m6",
                Label = "Condição Parcelamento",
                DataUrl = Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CondicaoParcelamento"),
                DataPostField = "descricao"
            }, ResourceHashConst.FaturamentoCadastrosCondicoesParcelamento));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m6",
                Label = "Categoria",
                PreFilter = "tipoCarteira",
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = @Url.Action("NovaCategoria")
            }, ResourceHashConst.FaturamentoCadastrosCategoria));

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
            }, ResourceHashConst.FaturamentoCadastrosTransportadoras));

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
            config.Elements.Add(new InputCurrencyUI { Id = "totalServicos", Class = "col s12 m4", Label = "Total serviços", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalRetencoesServicos", Class = "col s12 m4", Label = "Total retenções serviços", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosServicosNaoAgrega", Class = "col s12 m4", Label = "Total de impostos não incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m6", Label = "Frete a pagar", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalOrdemVenda", Class = "col s12 m6", Label = "Total pedido", Readonly = true });
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
            config.Elements.Add(new TextAreaUI { Id = "mensagemPadraoNota", Class = "col s12", Label = "Informações Adicionais NF-e", MaxLength = 4000 });
            config.Elements.Add(new TextAreaUI { Id = "informacoesCompletamentaresNFS", Class = "col s12", Label = "Informações Adicionais NFS-e", MaxLength = 1000 });
            config.Elements.Add(new DivElementUI { Id = "infoEstoqueNegativo", Class = "col s12 text-justify", Label = "Informação" });
            config.Elements.Add(new LabelSetUI { Id = "produtosEstoqueNegativoLabel", Class = "col s8", Label = "Produtos com estoque faltante" });
            config.Elements.Add(new InputCheckboxUI { Id = "ajusteEstoqueAutomatico", Class = "col s4", Label = "Ajustar negativo" });
            config.Elements.Add(new DivElementUI { Id = "produtosEstoqueNegativo", Class = "col s12 visible" });
            #endregion

            #region Helpers            
            config.Helpers.Add(new TooltipUI
            {
                Id = "chaveNFeReferenciada",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se o pedido for do tipo Devolução/Complementar, informe a chave de acesso sefaz da nota fiscal de origem referenciada. A chave é numérica é de tamanho 44. Se existir esta nota fiscal referenciada, o sistema irá preencher as informações como sugestão, somente na criação do novo pedido. Se o pedido não gerar nota fiscal, pode preencher com sequencia de 1. Após avançar a etapa da finalidade, não é mais possível voltar e editar estes dados."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "finalizarPedido",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Salvar e Finalizar, serão efetivadas as opções marcadas (Gerar financeiro, Movimentar estoque, Faturar e Ajustar negativo). Não será mais possível editar ou excluir este pedido."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "geraFinanceiro",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Gerar Financeiro, serão criadas contas a Receber(Normal/Complementar-Normal) ou contas a Pagar(Devolução/Complementar-Devolução) ao cliente, e conta a Pagar a transportadora do valor de frete, se for configurado por conta da sua empresa."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "formaPagamentoId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se o pedido vai ser faturado, informe a forma de pagamento."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "movimentaEstoque",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Movimentar Estoque, serão realizadas as movimentações de Saída(Normal/Complementar-Normal) ou Entrada(Devolução/Complementar-Devolução) da quantidade total dos produtos."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "totalOrdemVenda",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Total da soma (dos produtos + impostos incidentes nos produtos + serviços + frete (se for por conta da empresa)) menos as retenções dos serviços."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "totalImpostosProdutos",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Faturar, será calculado de acordo com as configurações do grupo tributário informado em cada produto. Impostos que agregam no total, como IPI e Substituição Tributária."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "totalImpostosProdutosNaoAgrega",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Faturar, será calculado de acordo com as configurações do grupo tributário informado em cada produto. Impostos que não agregam no total, como ICMS, COFINS, PIS e FCP."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "totalImpostosServicosNaoAgrega",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Faturar, será calculado de acordo com as configurações do grupo tributário informado em cada serviço. Impostos que não agregam no total, como ISS, COFINS, PIS, CSLL, INSS e Imposto de Renda."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "totalRetencoesServicos",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Calcular Faturar, será calculado de acordo com as configurações de retenção do grupo tributário, além das outras retenções informadas em cada serviço."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "geraNotaFiscal",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Calcula as tributações de acordo com o Grupo Tributário e gera as notas fiscais (NFe para produtos e NFSe para serviços)."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "naturezaOperacao",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Faturar, informe a natureza de operação para a nota fiscal a ser emitida."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "totalFrete",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Valor frete a ser pago, se for Normal(CIF/Remetente) ou Devolução(FOB/Destinatário)."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "transportadoraId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Informe a transportadora, quando configurar frete a ser pago por sua empresa, se for tipo pedido Normal(CIF/Remetente) ou Devolução(FOB/Destinatário)."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "grupoTributarioPadraoId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Será setado para cada produto/serviço adicionado, podendo ser alterado. Na devolução informe o grupo tributário com CFOP correspondente, para setar aos produto copiados da nota fiscal referenciada."
                }
            });
            #endregion

            cfg.Content.Add(config);
            cfg.Content.Add(GetDtOrdemVendaProdutosCfg());
            cfg.Content.Add(GetDtOrdemVendaServicosCfg());
            cfg.Content.Add(GetDtProdutosEstoqueNegativoCfg());

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");

        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
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

                var resourceNamePut = $"OrdemVenda/{id}";
                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(pedido, JsonSerializerSetting.Edit));

                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Edit);
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

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public JsonResult VerificaEstoqueNegativo(string id, string tipoVenda, string tipoNfeComplementar, string nFeRefComplementarIsDevolucao)
        {
            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.AddParam("pedidoId", id);
                queryString.AddParam("tipoVenda", tipoVenda);
                queryString.AddParam("tipoNfeComplementar", tipoNfeComplementar);
                queryString.AddParam("isComplementarDevolucao", nFeRefComplementarIsDevolucao);

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

        #region OnDemmand

        public JsonResult PostCliente(string term)
        {
            var entity = new PessoaVM
            {
                Nome = term,
                Cliente = true,
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

        private string GetTipoDocumento(string documento)
        {
            if (documento.Length <= 11)
                return "F";
            if (documento.Length > 11)
                return "J";

            return null;
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