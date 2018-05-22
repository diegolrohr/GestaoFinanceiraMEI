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

namespace Fly01.Compras.Controllers
{
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

        protected DataTableUI getDtPedidoItensCfg()
        {
            DataTableUI dtPedidoItensCfg = new DataTableUI
            {
                Parent = "pedidoProdutosField",
                Id = "dtPedidoItens",
                UrlGridLoad = Url.Action("GetPedidoItens", "PedidoItem"),
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

            dtPedidoItensCfg.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditarPedidoItem", Label = "Editar" });
            dtPedidoItensCfg.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluirPedidoItem", Label = "Excluir" });

            dtPedidoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "produto_descricao", DisplayName = "Produto", Priority = 1, Searchable = false, Orderable = false });
            dtPedidoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantidade", DisplayName = "Quantidade", Priority = 3, Type = "float", Searchable = false, Orderable = false });
            dtPedidoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "valor", DisplayName = "Valor", Priority = 4, Type = "currency", Searchable = false, Orderable = false });
            dtPedidoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "desconto", DisplayName = "Desconto", Priority = 5, Type = "currency", Searchable = false, Orderable = false });
            dtPedidoItensCfg.Columns.Add(new DataTableUIColumn() { DataField = "total", DisplayName = "Total", Priority = 6, Type = "currency", Searchable = false, Orderable = false });

            return dtPedidoItensCfg;
        }

        public override ContentResult Form()
        {
            return FormPedido();
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
                    List = @Url.Action("List", "Home")
                },
                ReadyFn = "fnFormReadyPedido",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Steps = new List<FormWizardUIStep>()
                {
                    new FormWizardUIStep()
                    {
                        Title = "Cadastro",
                        Id = "stepCadastro",
                        Quantity = 8,
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
                        Quantity = 6,
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
                        Quantity = 5,
                    }
                },
                Rule = isEdit ? "parallel" : "linear",
                ShowStepNumbers = true
            };

            #region step Cadastro
            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "status", Value = "Aberto" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoOrdemCompra", Value = "Pedido" });
            config.Elements.Add(new InputNumbersUI { Id = "orcamentoOrigemNumero", Class = "col s12 m4", Label = "Orçamento Origem", Disabled = true });
            config.Elements.Add(new InputNumbersUI { Id = "numero", Class = "col s12 m4", Label = "Número", Disabled = true });
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m4", Label = "Data", Required = true });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "fornecedorId",
                Class = "col s12",
                Label = "Fornecedor",
                Required = true,
                DataUrl = Url.Action("Fornecedor", "AutoComplete"),
                LabelId = "fornecedorNome",
                DataUrlPost = Url.Action("PostFornecedor")

            });
            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });
            #endregion

            #region step Financeiro
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "geraFinanceiro",
                Class = "col s12 m3",
                Label = "Gera financeiro",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnValidaCamposGeraFinanceiro" }
                }
            });
            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m3", Label = "Data Vencimento" });
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
                DataUrlPost = Url.Action("NovaCategoriaDespesa")

            });
            #endregion

            #region step Transporte
            config.Elements.Add(new AutoCompleteUI
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
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoFrete))),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeFrete" }
                }
            });
            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorFrete",
                Class = "col s12 m3",
                Label = "Valor Frete",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeFrete" }
                }
            });
            config.Elements.Add(new InputFloatUI { Id = "pesoBruto", Class = "col s12 m3", Label = "Peso bruto" });
            config.Elements.Add(new InputFloatUI { Id = "pesoLiquido", Class = "col s12 m3", Label = "Peso líquido" });
            config.Elements.Add(new InputNumbersUI { Id = "quantidadeVolumes", Class = "col s12 m3", Label = "Quant. volumes" });
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

            #region step Finalizar
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m4", Label = "Total produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m4", Label = "Frete comprador paga (FOB/Destinatário)", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalPedido", Class = "col s12 m4", Label = "Total pedido(produtos + frete)", Readonly = true });
            config.Elements.Add(new InputCheckboxUI { Id = "movimentaEstoque", Class = "col s12 m4", Label = "Movimenta estoque" });
            config.Elements.Add(new InputCheckboxUI { Id = "finalizarPedido", Class = "col s12 m4", Label = "Salvar e já finalizar" });

            #endregion

            cfg.Content.Add(config);
            cfg.Content.Add(getDtPedidoItensCfg());
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public JsonResult FinalizarPedido(string id)
        {
            try
            {
                var queryString = AppDefaults.GetQueryStringDefault();
                queryString.AddParam("$filter", $"id eq {id}");

                var pedido = RestHelper.ExecuteGetRequest<ResultBase<PedidoVM>>("Pedido", queryString).Data.FirstOrDefault();
                pedido.Status = "Finalizado";

                return Edit(pedido);
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
                Title = "Visualizar pedido",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                ReadyFn = "fnFormReadyVisualizarPedido",
                Id = "fly01mdlfrmPedido"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputTextUI { Id = "orcamentoOrigemNumero", Class = "col s12 m4", Label = "Orçamento Origem", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "numero", Class = "col s12 m4", Label = "Número", Disabled = true });
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m4", Label = "Data", Disabled = true });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "fornecedorId",
                Class = "col s12",
                Label = "Fornecedor",
                Disabled = true,
                DataUrl = Url.Action("Fornecedor", "AutoComplete"),
                LabelId = "fornecedorNome"
            });
            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", Disabled = true, MaxLength = 200 });
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

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "transportadoraId",
                Class = "col s12",
                Label = "Transportadora",
                Disabled = true,
                DataUrl = Url.Action("Transportadora", "AutoComplete"),
                LabelId = "transportadoraNome"
            });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoFrete",
                Class = "col s12 m4",
                Label = "Tipo Frete",
                Value = "SemFrete",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoFrete)))
            });
            config.Elements.Add(new InputCurrencyUI { Id = "valorFrete", Class = "col s12 m4", Label = "Valor Frete", Disabled = true });
            config.Elements.Add(new InputCurrencyUI { Id = "pesoBruto", Class = "col s12 m4", Label = "Peso bruto", Disabled = true });
            config.Elements.Add(new InputCurrencyUI { Id = "pesoLiquido", Class = "col s12 m6", Label = "Peso líquido", Disabled = true });
            config.Elements.Add(new InputNumbersUI { Id = "quantidadeVolumes", Class = "col s12 m6", Label = "Quant. volumes", Disabled = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m4", Label = "Total produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m4", Label = "Frete comprador paga (FOB/Destinatário)", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalPedido", Class = "col s12 m4", Label = "Total pedido(produtos + frete)", Readonly = true });
            config.Elements.Add(new InputCheckboxUI { Id = "movimentaEstoque", Class = "col s12 m6", Label = "Movimenta estoque", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "geraFinanceiro", Class = "col s12 m6", Label = "Gera financeiro", Disabled = true });

            config.Elements.Add(new LabelSetUI { Id = "labelSetProdutos", Class = "col s12", Label = "Produtos" });
            config.Elements.Add(new TableUI
            {
                Id = "pedidoItensDataTable",
                Class = "col s12",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Produto", Value = "0"},
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