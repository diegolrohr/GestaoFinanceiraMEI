using Fly01.Compras.Models.Reports;
using Fly01.Compras.Models.ViewModel;
using Fly01.Compras.ViewModel;
using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.uiJS.Enums;
using Fly01.uiJS.Classes.Helpers;
using Fly01.Core.ViewModels;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasComprasOrcamentoPedido)]
    public class OrcamentoController : BaseController<OrcamentoVM>
    {
        //OrcamentoVM e PedidoVM na mesma controller ordemCompra(gridLoad, form), direcionado para a controller via javaScript
        public OrcamentoController()
        {
            ExpandProperties = "condicaoParcelamento($select=id,descricao, qtdParcelas,condicoesParcelamento),formaPagamento($select=id,descricao),categoria,centroCusto,grupoTributarioPadrao($select=id,descricao,tipoTributacaoICMS)";
        }

        public override Func<OrcamentoVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        protected DataTableUI GetDtOrcamentoItensCfg()
        {
            DataTableUI dtOrcamentoItensCfg = new DataTableUI
            {
                Parent = "orcamentoProdutosField",
                Id = "dtOrcamentoItens",
                UrlGridLoad = Url.Action("GetOrcamentoItens", "OrcamentoItem"),
                UrlFunctions = Url.Action("Functions", "OrcamentoItem") + "?fns=",
                Callbacks = new DataTableUICallbacks()
                {
                    FooterCallback = "fnFooterCallbackOrcamentoItem"
                },
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                },
                Functions = new List<string>() { "fnFooterCallbackOrcamentoItem" }
            };

            dtOrcamentoItensCfg.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditarOrcamentoItem", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluirOrcamentoItem", Label = "Excluir" }
            }));

            dtOrcamentoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "produto_descricao", DisplayName = "Produto", Priority = 1, Searchable = false, Orderable = false });
            dtOrcamentoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "fornecedor_nome", DisplayName = "Fornecedor", Priority = 2, Searchable = false, Orderable = false });
            dtOrcamentoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantidade", DisplayName = "Quantidade", Priority = 3, Type = "float", Searchable = false, Orderable = false });
            dtOrcamentoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "valor", DisplayName = "Valor", Priority = 4, Type = "currency", Searchable = false, Orderable = false });
            dtOrcamentoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "desconto", DisplayName = "Desconto", Priority = 5, Type = "currency", Searchable = false, Orderable = false });
            dtOrcamentoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "valorOutrasRetencoes", DisplayName = "Outras Retenções", Priority = 7, Type = "currency", Searchable = false, Orderable = false });
            dtOrcamentoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "total", DisplayName = "Total", Priority = 6, Type = "currency", Searchable = false, Orderable = false });

            return dtOrcamentoItensCfg;
        }

        protected override ContentUI FormJson()
            => FormOrcamentoJson();

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelarOrcamento", Position = HtmlUIButtonPosition.Main });
            }

            return target;
        }

        public ContentResult FormOrcamento(bool isEdit = false)
            => Content(JsonConvert.SerializeObject(FormOrcamentoJson(isEdit), JsonSerializerSetting.Front), "application/json");

        public ContentUI FormOrcamentoJson(bool isEdit = false)
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
                    Title = "Orçamento",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnFormReadyOrcamentoItem" }
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
                ReadyFn = "fnFormReadyOrcamento",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Steps = new List<FormWizardUIStep>()
                {
                    new FormWizardUIStep()
                    {
                        Title = "Cadastro",
                        Id = "stepCadastro",
                        Quantity = 6,
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
                        Quantity = 5,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Finalizar",
                        Id = "stepFinalizar",
                        Quantity = 2,
                    }
                },
                Rule = isEdit ? "parallel" : "linear",
                ShowStepNumbers = true
            };

            #region step Cadastro
            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "status", Value = "Aberto" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoOrdemCompra", Class = "col s12 m4", Label = "Tipo", Value = "Orcamento" });
            config.Elements.Add(new InputNumbersUI { Id = "numero", Class = "col s12 m4", Label = "Número", Disabled = true });
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m4", Label = "Data", Required = true });
            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });
            #endregion

            #region step Produtos
            config.Elements.Add(new ButtonUI
            {
                Id = "btnAddOrcamentoItem",
                Class = "col s12 m3",
                Value = "Adicionar produto",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "click", Function = "fnModalOrcamentoItem" }
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
                        new DomEventUI { DomEvent = "click", Function = "fnModalOrcamentoKit" }
                    }
            });
            config.Elements.Add(new DivElementUI { Id = "orcamentoProdutos", Class = "col s12 visible" });
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
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = Url.Action("NovaCategoria")
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

            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m2", Label = "Data Vencimento" });
            #endregion

            #region step Finalizar
            config.Elements.Add(new InputCurrencyUI { Id = "totalOrcamento", Class = "col s12 m4", Label = "Total", Readonly = true });
            config.Elements.Add(new InputCheckboxUI { Id = "finalizarOrcamento", Class = "col s12 m4", Label = "Salvar e já finalizar" });

            #endregion

            cfg.Content.Add(config);
            cfg.Content.Add(GetDtOrcamentoItensCfg());
            return cfg;
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public JsonResult GerarPedidos(string id)
        {
            try
            {
                var queryString = AppDefaults.GetQueryStringDefault();
                queryString.AddParam("$filter", $"id eq {id}");

                var orcamento = RestHelper.ExecuteGetRequest<ResultBase<OrcamentoVM>>("Orcamento", queryString).Data.FirstOrDefault();
                orcamento.Status = "Finalizado";

                return Edit(orcamento);
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
                Title = "Visualizar orçamento",
                UrlFunctions = @Url.Action("Functions", "Orcamento", null, Request.Url.Scheme) + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                ReadyFn = "fnFormReadyVisualizarOrcamento",
                Id = "fly01mdlfrmOrcamento"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputTextUI { Id = "numero", Class = "col s12 m4", Label = "Número", Disabled = true });
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m4", Label = "Data", Disabled = true });
            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m4", Label = "Data Vencimento", Disabled = true });
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
                Class = "col s12",
                Label = "Categoria",
                Disabled = true,
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao",
            });

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", Disabled = true, MaxLength = 200 });

            config.Elements.Add(new LabelSetUI { Id = "labelSetProdutos", Class = "col s12", Label = "Produtos" });

            config.Elements.Add(new TableUI
            {
                Id = "orcamentoItensDataTable",
                Class = "col s12",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Produto", Value = "0"},
                    new OptionUI { Label = "Fornecedor", Value = "1"},
                    new OptionUI { Label = "Quant.", Value = "2"},
                    new OptionUI { Label = "Valor",Value = "3"},
                    new OptionUI { Label = "Desconto",Value = "4"},                    
                    new OptionUI { Label = "Total",Value = "5"},
                }
            });            

            return Content(JsonConvert.SerializeObject(config, uiJS.Defaults.JsonSerializerSetting.Front), "application/json");
        }

        [HttpPost]
        public override JsonResult Create(OrcamentoVM entityVM)
        {
            try
            {
                var postResponse = RestHelper.ExecutePostRequest(ResourceName, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                OrcamentoVM postResult = JsonConvert.DeserializeObject<OrcamentoVM>(postResponse);
                var response = new JsonResult
                {
                    Data = new { success = true, message = AppDefaults.CreateSuccessMessage, id = postResult.Id.ToString(), numero = postResult.Numero.ToString() }
                };
                return (response);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpPost]
        public override JsonResult Edit(OrcamentoVM entityVM)
            => base.Edit(entityVM);

        [HttpGet]
        public JsonResult TotalOrcamentoItens(string id)
        {
            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.AddParam("$filter", $"orcamentoId eq {id} and ativo eq true");

                var total = RestHelper.ExecuteGetRequest<ResultBase<OrcamentoItemVM>>("OrcamentoItem", queryString).Data.Sum(x => x.Total);

                return Json(
                    new { totalOrcamentoItens = total, success = true },
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
        public List<CondicaoParcelamentoParcelaVM> GetSimulacaoContas(OrcamentoVM orcamento)
        {
            var dadosReferenciaSimulacao = new
            {
                valorReferencia = orcamento.Total,
                dataReferencia = orcamento?.DataVencimento,
                condicoesParcelamento = orcamento?.CondicaoParcelamento?.CondicoesParcelamento,
                qtdParcelas = orcamento.CondicaoParcelamento?.QtdParcelas
            };
            return RestHelper.ExecutePostRequest<ResponseSimulacaoVM>("condicaoparcelamentosimulacao", dadosReferenciaSimulacao)?.Items;
        }

        public virtual ActionResult ImprimirOrcamento(Guid id)
        {
            OrcamentoVM Orcamento = Get(id);

            var produtos = GetOrcamento(id);

            List<ImprimirOrcamentoVM> reportItems = new List<ImprimirOrcamentoVM>();

            var simulacao = GetSimulacaoContas(Orcamento);
            var parcelas = "";

            for (var i = 0; i < simulacao.Count; i++)
            {
                parcelas += $"{simulacao[i].DescricaoParcela} - Vencimento {simulacao[i].DataVencimento.ToString("dd/MM/yyyy")} - {simulacao[i].Valor.ToString("C", AppDefaults.CultureInfoDefault)}    ";
                if (i % 2 != 0 && i > 0 && i < (simulacao.Count-1))
                {
                    parcelas += "\n";
                }
            }
            
            foreach (OrcamentoItemVM produtosorcamento in produtos)

                reportItems.Add(new ImprimirOrcamentoVM
                {
                    //ORCAMENTO
                    Categoria = Orcamento.Categoria != null ? Orcamento.Categoria.Descricao : string.Empty,
                    CondicaoParcelamento = Orcamento.CondicaoParcelamento != null ? Orcamento.CondicaoParcelamento.Descricao : string.Empty,
                    DataVencimento = Orcamento.DataVencimento,
                    FormaPagamento = Orcamento.FormaPagamento != null ? Orcamento.FormaPagamento.Descricao : string.Empty,
                    Numero = Orcamento.Numero,
                    Observacao = Orcamento.Observacao,
                    ParcelaConta = parcelas,
                    //PRODUTO
                    Id = produtosorcamento.Id.ToString(),
                    NomeProduto = produtosorcamento.Produto != null ? produtosorcamento.Produto.Descricao : string.Empty,
                    Fornecedor = produtosorcamento.Fornecedor != null ? produtosorcamento.Fornecedor.Nome.ToString() : string.Empty,
                    QtdProduto = produtosorcamento.Quantidade,
                    ValorUnitario = produtosorcamento.Valor,
                    ValorTotal = produtosorcamento.Total
                });

            if (!produtos.Any())
            {
                reportItems.Add(new ImprimirOrcamentoVM
                {
                    //ORCAMENTO
                    Categoria = Orcamento.Categoria != null ? Orcamento.Categoria.Descricao : string.Empty,
                    CondicaoParcelamento = Orcamento.CondicaoParcelamento != null ? Orcamento.CondicaoParcelamento.Descricao : string.Empty,
                    DataVencimento = Orcamento.DataVencimento,
                    FormaPagamento = Orcamento.FormaPagamento != null ? Orcamento.FormaPagamento.Descricao : string.Empty,
                    Numero = Orcamento.Numero,
                    Observacao = Orcamento.Observacao,
                    ParcelaConta = parcelas
                });
            }

            var reportViewer = new WebReportViewer<ImprimirOrcamentoVM>(ReportImprimirOrcamento.Instance);
            return File(reportViewer.Print(reportItems, SessionManager.Current.UserData.PlatformUrl), "application/pdf");

        }

        public List<OrcamentoItemVM> GetOrcamento(Guid id)
        {
            var queryString = new Dictionary<string, string>();
            queryString.AddParam("$filter", $"orcamentoId eq {id}");
            queryString.AddParam("$expand", "produto,fornecedor");

            return RestHelper.ExecuteGetRequest<ResultBase<OrcamentoItemVM>>("OrcamentoItem", queryString).Data;
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
                    Create = @Url.Action("AdicionarKit", "Orcamento")
                },
                Id = "fly01mdlfrmOrcamentoKit",
                ReadyFn = "fnFormReadyOrcamentoKit"
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
                Id = "fornecedorPadraoIdKit",
                Name = "fornecedorPadraoId",
                Class = "col s12",
                Required = true,
                Label = "Fornecedor padrão",
                DataUrl = Url.Action("Fornecedor", "AutoComplete"),
                LabelId = "fornecedorPadraoNomeKit",
                LabelName = "fornecedorPadraoNome",
                DataUrlPostModal = Url.Action("FormModal", "Fornecedor"),
                DataPostField = "nome"
            }, ResourceHashConst.ComprasCadastrosFornecedores));

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
                Id = "fornecedorId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Informe um fornecedor padrão para todos os produtos do kit, que vão ser adicionados ao orçamento."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "somarExistentes",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Os produtos cadastrados no kit, serão somados com a quantidade já existente no orçamento."
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
                RestHelper.ExecutePostRequest("kitorcamento", JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                return JsonResponseStatus.GetSuccess("Produtos do kit adicionados com sucesso.");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        #region OnDemmand

        [HttpPost]
        public JsonResult NovaCategoria(string term)
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

        private string GetTipoDocumento(string documento)
        {
            if (documento.Length <= 11)
                return "F";
            if (documento.Length > 11)
                return "J";

            return null;
        }

        #endregion

    }
}