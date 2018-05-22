using Fly01.Compras.Controllers.Base;
using Fly01.Compras.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Compras.Models.ViewModel;
using Fly01.Compras.Models.Reports;
using Fly01.Core.Config;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Compras.Controllers
{
    public class OrcamentoController : BaseController<OrcamentoVM>
    {
        //OrcamentoVM e PedidoVM na mesma controller ordemCompra(gridLoad, form), direcionado para a controller via javaScript
        public OrcamentoController()
        {
            ExpandProperties = "condicaoParcelamento,formaPagamento,categoria";
        }

        public override Func<OrcamentoVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        protected DataTableUI getDtOrcamentoItensCfg()
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

            dtOrcamentoItensCfg.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditarOrcamentoItem", Label = "Editar" });
            dtOrcamentoItensCfg.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluirOrcamentoItem", Label = "Excluir" });

            dtOrcamentoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "produto_descricao", DisplayName = "Produto", Priority = 1, Searchable = false, Orderable = false });
            dtOrcamentoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "fornecedor_nome", DisplayName = "Fornecedor", Priority = 2, Searchable = false, Orderable = false });
            dtOrcamentoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantidade", DisplayName = "Quantidade", Priority = 3, Type = "float", Searchable = false, Orderable = false });
            dtOrcamentoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "valor", DisplayName = "Valor", Priority = 4, Type = "currency", Searchable = false, Orderable = false });
            dtOrcamentoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "desconto", DisplayName = "Desconto", Priority = 5, Type = "currency", Searchable = false, Orderable = false });
            dtOrcamentoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "total", DisplayName = "Total", Priority = 6, Type = "currency", Searchable = false, Orderable = false });

            return dtOrcamentoItensCfg;
        }

        public override ContentResult Form()
        {
            return FormOrcamento();
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
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelarOrcamento" },
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnFormReadyOrcamentoItem" }
            };

            var config = new FormWizardUI
            {
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List", "Home")
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
                        Title = "Financeiro",
                        Id = "stepFinanceiro",
                        Quantity = 4,
                    },
                    new FormWizardUIStep()
                    {
                        Title = "Produtos",
                        Id = "stepProdutos",
                        Quantity = 2,
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

            #region step Financeiro
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 m6",
                Label = "Forma Pagamento",
                DataUrl = Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "FormaPagamento"),
                DataPostField = "descricao"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 m6",
                Label = "Condição Parcelamento",
                DataUrl = Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CondicaoParcelamento"),
                DataPostField = "descricao"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m6",
                Label = "Categoria",
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = Url.Action("NovaCategoria")
            });
            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m3", Label = "Data Vencimento" });
            #endregion

            #region step Produtos
            config.Elements.Add(new ButtonUI
            {
                Id = "btnAddOrcamentoItem",
                Class = "col s12 m2",
                Value = "Adicionar produto",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "click", Function = "fnModalOrcamentoItem" }
                }
            });
            config.Elements.Add(new DivElementUI { Id = "orcamentoProdutos", Class = "col s12" });
            #endregion

            #region step Finalizar
            config.Elements.Add(new InputCurrencyUI { Id = "totalOrcamento", Class = "col s12 m4", Label = "Total", Readonly = true });
            config.Elements.Add(new InputCheckboxUI { Id = "finalizarOrcamento", Class = "col s12 m4", Label = "Salvar e já finalizar" });

            #endregion

            cfg.Content.Add(config);
            cfg.Content.Add(getDtOrcamentoItensCfg());
            return Content(JsonConvert.SerializeObject(cfg, uiJS.Defaults.JsonSerializerSetting.Front), "application/json");
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
                //return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, postResult.Id);
                var response = new JsonResult();
                response.Data = new { success = true, message = AppDefaults.CreateSuccessMessage, id = postResult.Id.ToString(), numero = postResult.Numero.ToString() };
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
        {
            return base.Edit(entityVM);
        }

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

        public virtual ActionResult ImprimirOrcamento(Guid id)
        {
            OrcamentoVM Orcamento = Get(id);

            var produtos = GetOrcamento(id);

            List<ImprimirOrcamentoVM> reportItems = new List<ImprimirOrcamentoVM>();

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
                    //PRODUTO
                    Id = produtosorcamento.Id.ToString(),
                    NomeProduto = produtosorcamento.Produto != null ? produtosorcamento.Produto.Descricao : string.Empty,
                    Fornecedor = produtosorcamento.Fornecedor != null ? produtosorcamento.Fornecedor.Nome.ToString() : string.Empty,
                    QtdProduto = produtosorcamento.Quantidade,
                    ValorUnitario = produtosorcamento.Valor,
                    ValorTotal = produtosorcamento.Total,
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

        #endregion

    }
}