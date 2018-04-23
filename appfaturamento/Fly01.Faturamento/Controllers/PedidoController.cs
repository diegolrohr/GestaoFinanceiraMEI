﻿using Fly01.Faturamento.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Fly01.Core;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Faturamento.Controllers
{
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
                    new DataTableUIParameter { Id = "id", Required = true }
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

        public override Func<OrdemVendaVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public ContentResult FormPedido(bool isEdit = false)
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
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelarPedido" },
                    }
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
                ReadyFn = "fnFormReadyPedido",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnChangeEstado" },
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
                        Quantity = 5,
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
                        Quantity = 15,
                    }
                },
                Rule = isEdit ? "parallel" : "linear",
                ShowStepNumbers = true
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "status", Value = "Aberto" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoOrdemVenda", Value = "Pedido" });
            config.Elements.Add(new InputHiddenUI { Id = "grupoTributarioPadraoTipoTributacaoICMS" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoVenda", Value = "Normal" });

            #region step Cadastro
            config.Elements.Add(new InputNumbersUI { Id = "numero", Class = "col s12 m3", Label = "Número", Disabled = true });
            //config.Elements.Add(new SelectUI
            //{
            //    Id = "tipoVenda",
            //    Class = "col s12 m4",
            //    Label = "Tipo Venda",
            //    Value = "Normal",
            //    Required = true,
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoVenda", true, false))
            //});
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m3", Label = "Data", Required = true });

            config.Elements.Add(new AutocompleteUI
            {
                Id = "grupoTributarioPadraoId",
                Class = "col s12 m6",
                Label = "Grupo Tributário Padrão",
                DataUrl = Url.Action("GrupoTributario", "AutoComplete"),
                LabelId = "grupoTributarioPadraoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "GrupoTributario"),
                DataPostField = "descricao",
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeGrupoTribPadrao" } }
            });

            config.Elements.Add(new AutocompleteUI
            {
                Id = "clienteId",
                Class = "col s12",
                Label = "Cliente",
                Required = true,
                DataUrl = Url.Action("Cliente", "AutoComplete"),
                LabelId = "clienteNome",
                DataUrlPost = Url.Action("PostCliente")
            });
            
            config.Elements.Add(new TextareaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });
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
            config.Elements.Add(new AutocompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 m6",
                Label = "Forma Pagamento",
                DataUrl = Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "FormaPagamento"),
                DataPostField = "descricao"

            });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 m6",
                Label = "Condição Parcelamento",
                DataUrl = Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CondicaoParcelamento"),
                DataPostField = "descricao"

            });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m6",
                Label = "Categoria",
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = Url.Action("NovaCategoriaReceita")

            });
            #endregion

            #region step Transporte
            config.Elements.Add(new AutocompleteUI
            {
                Id = "transportadoraId",
                Class = "col s12 m8",
                Label = "Transportadora",
                DataUrl = Url.Action("Transportadora", "AutoComplete"),
                LabelId = "transportadoraNome",
                DataUrlPost = Url.Action("PostTransportadora")

            });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoFrete",
                Class = "col s12 m4",
                Label = "Tipo Frete",
                Value = "SemFrete",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoFrete)).ToList()
                    .FindAll(x => "FOB,CIF,Terceiro,SemFrete".Contains(x.Value))),
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
            config.Elements.Add(new AutocompleteUI
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
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutos", Class = "col s12 m4", Label = "Total de impostos incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutosNaoAgrega", Class = "col s12 m4", Label = "Total de impostos não incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalServicos", Class = "col s12 m6", Label = "Total serviços", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosServicos", Class = "col s12 m6", Label = "Total impostos serviços", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m6", Label = "Frete fornecedor paga (CIF/Remetente)", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalOrdemVenda", Class = "col s12 m6", Label = "Total pedido (produtos + serviços + impostos + frete)", Readonly = true });
            config.Elements.Add(new InputCheckboxUI { Id = "movimentaEstoque", Class = "col s12 m4", Label = "Movimentar estoque" });
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
            config.Elements.Add(new InputTextUI { Id = "naturezaOperacao", Class = "col s12", Label = "Natureza de Operação", MaxLength = 60});
            config.Elements.Add(new DivElementUI { Id = "infoEstoqueNegativo", Class = "col s12 text-justify", Label = "Informação" });
            config.Elements.Add(new LabelsetUI { Id = "produtosEstoqueNegativoLabel", Class = "col s12 m8", Label = "Produtos com estoque faltante" });
            config.Elements.Add(new InputCheckboxUI { Id = "ajusteEstoqueAutomatico", Class = "col s12 m4", Label = "Ajustar negativo" });
            config.Elements.Add(new DivElementUI { Id = "produtosEstoqueNegativo", Class = "col s12" });
            #endregion

            #region Helpers 
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
                    Text = "Se marcar Gerar Financeiro, serão criadas contas a receber do valor total do pedido."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "movimentaEstoque",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Movimentar Estoque, serão realizadas as movimentações de saída da quantidade total dos produtos."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "totalOrdemVenda",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Total da soma dos produtos, serviços, frete (somente se for do tipo CIF ou Remetente) e da soma dos impostos."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "totalImpostosProdutos",
                Tooltip = new HelperUITooltip()
                {,
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
                Id = "totalImpostosServicos",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se marcar Faturar, será calculado de acordo com as configurações ajustadas no cadastro dos parâmetros tributários."
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
            #endregion

            cfg.Content.Add(config);
            cfg.Content.Add(GetDtOrdemVendaProdutosCfg());
            cfg.Content.Add(GetDtOrdemVendaServicosCfg());
            cfg.Content.Add(GetDtProdutosEstoqueNegativoCfg());
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");

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

        public JsonResult VerificaEstoqueNegativo(string id)
        {
            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.AddParam("pedidoId", id);

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