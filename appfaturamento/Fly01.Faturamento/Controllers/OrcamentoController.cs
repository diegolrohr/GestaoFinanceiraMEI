using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Faturamento.ViewModel;
using Fly01.Core;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using System.Dynamic;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using System.Text.RegularExpressions;
using Fly01.Core.Rest;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoFaturamentoVendas)]
    public class OrcamentoController : OrdemVendaController
    {
        //pedido e orçamento são ordem de venda, apenas a propriedade TipoOrdemVenda que muda
        //porém foi feito os controllers distintos para efeito front ao usuário

        public override Func<OrdemVendaVM, object> GetDisplayData() { throw new NotImplementedException(); }

        protected override ContentUI FormJson() { throw new NotImplementedException(); }

        public override ContentResult List() { throw new NotImplementedException(); }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelarOrcamento" });
            }

            return target;
        }

        public ContentResult FormOrcamento(bool isEdit = false)
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
                    Title = "Orçamento",
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
                    List = @Url.Action("List", "OrdemVenda")
                },
                ReadyFn = "fnFormReadyOrcamento",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Steps = new List<FormWizardUIStep>()
                {
                    new FormWizardUIStep()
                    {
                        Title = "Cadastro",
                        Id = "stepCadastro",
                        Quantity = 10,
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
                        Quantity = 4,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Transporte",
                        Id = "stepTransporte",
                        Quantity = 8,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Finalizar",
                        Id = "stepFinalizar",
                        Quantity = 8,
                    }
                },
                Rule = isEdit ? "parallel" : "linear",
                ShowStepNumbers = true
            };

            #region step Cadastro
            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "status", Value = "Aberto" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoOrdemVenda", Value = "Orcamento" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoVenda", Value = "Normal" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoCarteira", Value = "Receita" });

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
                DataPostField = "descricao"
            }, ResourceHashConst.FaturamentoCadastrosGrupoTributario));

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
            config.Elements.Add(new DivElementUI { Id = "ordemVendaProdutos", Class = "col s12" });
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
            config.Elements.Add(new DivElementUI { Id = "ordemVendaServicos", Class = "col s12" });
            #endregion

            #region step Financeiro
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

            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m3", Label = "Data Vencimento" });
            #endregion

            #region step Transporte
            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "transportadoraId",
                Class = "col s12 m8",
                Label = "Transportadora",
                DataUrl = Url.Action("Transportadora", "AutoComplete"),
                LabelId = "transportadoraNome",
                DataUrlPost = Url.Action("PostTransportadora")
            }, ResourceHashConst.FaturamentoCadastrosTransportadoras));

            config.Elements.Add(new SelectUI
            {
                Id = "tipoFrete",
                Class = "col s12 m4",
                Label = "Tipo Frete",
                Value = "SemFrete",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoFrete))),
                DomEvents = new List<DomEventUI>
                    {
                        new DomEventUI { DomEvent = "change", Function = "fnChangeFrete" }
                    }
            });
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
            config.Elements.Add(new InputFloatUI { Id = "pesoBruto", Class = "col s12 m4", Label = "Peso Bruto" });
            config.Elements.Add(new InputFloatUI { Id = "pesoLiquido", Class = "col s12 m4", Label = "Peso Líquido" });
            config.Elements.Add(new InputNumbersUI { Id = "quantidadeVolumes", Class = "col s12 m4", Label = "Quantidade Volumes" });
            #endregion

            #region step Finalizar
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m4", Label = "Total produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutos", Class = "col s12 m4", Label = "Total impostos produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutosNaoAgrega", Class = "col s12 m4", Label = "Total de impostos não incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalServicos", Class = "col s12 m4", Label = "Total serviços", Readonly = true });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "geraNotaFiscal",
                Class = "col s12 m4",
                Label = "Calcular tributação",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "click", Function = "fnClickGeraNotaFiscal" }
                }
            });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosServicos", Class = "col s12 m4", Label = "Total impostos serviços", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m6", Label = "Frete a pagar", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalOrdemVenda", Class = "col s12 m6", Label = "Total pedido(produtos + serviços + impostos + frete)", Readonly = true });

            #endregion

            #region Helpers
            config.Helpers.Add(new TooltipUI
            {
                Id = "geraNotaFiscal",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Calcular Tributação, será calculado as tributações e consideradas no total. É necessário informar o estado no cadastro da empresa e no cadastro do cliente informado. Também é necessário ter salvo as configurações no cadastro dos parâmetros tributários."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "totalImpostosServicos",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Calcular Tributação, será calculado de acordo com as configurações ajustadas no cadastro dos parâmetros tributários."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "totalImpostosProdutos",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Calcular Tributação, será calculado de acordo com as configurações do grupo tributário informado em cada produto. Impostos que agregam no total, como IPI e Substituição Tributária."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "totalImpostosProdutosNaoAgrega",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Calcular Tributação, será calculado de acordo com as configurações do grupo tributário informado em cada produto. Impostos que não agregam no total, como ICMS e FCP."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "totalOrdemVenda",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Total da soma dos produtos, serviços, frete (se for por conta da empresa) e da soma dos impostos."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "grupoTributarioPadraoId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Será setado para cada produto/serviço adicionado, podendo ser alterado."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "transportadoraId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Informe a transportadora, quando configurar frete a ser pago por sua empresa Normal(CIF/Remetente)."
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
            #endregion

            cfg.Content.Add(config);
            cfg.Content.Add(GetDtOrdemVendaProdutosCfg());
            cfg.Content.Add(GetDtOrdemVendaServicosCfg());
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult ConverterParaPedido(string id)
        {
            try
            {
                dynamic orcamento = new ExpandoObject();
                orcamento.tipoOrdemVenda = "Pedido";
                orcamento.movimentaEstoque = true;
                orcamento.geraFinanceiro = true;

                var resourceNamePut = $"OrdemVenda/{id}";
                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(orcamento, JsonSerializerSetting.Edit));

                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Edit);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        #region OnDemmand

        [HttpPost]
        public JsonResult NovaCategoriaReceita(string term)
        {
            return NovaCategoria(new CategoriaVM { Descricao = term, TipoCarteira = "1" });
        }

        private JsonResult NovaCategoria(CategoriaVM entity)
        {
            try
            {
                var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));
                var data = RestHelper.ExecutePostRequest<CategoriaVM>(resourceName, entity, AppDefaults.GetQueryStringDefault());

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public JsonResult PostCliente(string term)
        {
            var entity = new PessoaVM
            {
                Nome = term,
                Cliente = true,
                TipoIndicacaoInscricaoEstadual = "ContribuinteICMS"
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
                TipoIndicacaoInscricaoEstadual = "ContribuinteICMS"
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