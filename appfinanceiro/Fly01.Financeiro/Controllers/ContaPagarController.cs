using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Financeiro.Controllers.Base;
using Fly01.Financeiro.Models.Reports;
using Fly01.Financeiro.Models.ViewModel;
using Fly01.Financeiro.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    public class ContaPagarController : ContaFinanceiraController<ContaPagarVM, ContaFinanceiraBaixaVM>
    {
        public ContaPagarController()
        {
            ExpandProperties = "condicaoParcelamento($select=descricao,qtdParcelas,condicoesParcelamento),pessoa($select=nome),categoria($select=descricao),formaPagamento($select=descricao)";
        }

        public override ActionResult ImprimirRecibo(Guid id)
        {
            ContaPagarVM itemContaPagar = Get(id);

            double discount = 0;
            double interest = 0;
            double total = 0;
            string valorTituloTotalFormatado = itemContaPagar.ValorPrevisto.ToString("C", AppDefaults.CultureInfoDefault);
            var managerEmpresaVM = GetDadosEmpresa();

            if (itemContaPagar.StatusContaBancaria == StatusContaBancaria.Pago.ToString())
            {
                total = itemContaPagar.ValorPago.Value;
            }
            else
            {
                total = itemContaPagar.ValorPrevisto;
            }

            string valorTituloFormatado = total.ToString("C", AppDefaults.CultureInfoDefault);

            StringBuilder assinatura = new StringBuilder();
            assinatura.Append("_________________________________________________________");
            assinatura.Append("<br/>");
            assinatura.Append(itemContaPagar.Pessoa.Nome);

            ReciboContaFinanceiraVM itemRecibo = new ReciboContaFinanceiraVM
            {
                Id = itemContaPagar.Id.ToString(),
                //Conteudo = String.Format("Recebemos de {0} o pagamento de {1} ({2}) referente à:", branchVM.Name, valorTituloTotalFormatado, itemBankTransac.Value.toExtenso()),
                Conteudo = String.Format("Recebemos de {0} o pagamento de {1} ({2}) referente à:", managerEmpresaVM.RazaoSocial, valorTituloTotalFormatado, itemContaPagar.StatusContaBancaria == StatusContaBancaria.Pago.ToString() ? itemContaPagar.ValorPago.Value.toExtenso() : itemContaPagar.ValorPrevisto.toExtenso()),
                DescricaoTitulo = !String.IsNullOrWhiteSpace(itemContaPagar.Descricao) ? itemContaPagar.Descricao : itemContaPagar.Categoria.Descricao,
                ValorTitulo = valorTituloFormatado,
                DataAtual = String.Format("{0}, {1} de {2} de {3}", managerEmpresaVM.Cidade != null ? managerEmpresaVM.Cidade.Nome : "", DateTime.Now.Day, AppDefaults.CultureInfoDefault.DateTimeFormat.GetMonthName(DateTime.Now.Month), DateTime.Now.Year),
                Assinatura = assinatura.ToString(),
                DescricaoJuros = "Juros",
                ValorJuros = string.Format("(+) {0}", interest.ToString("C", AppDefaults.CultureInfoDefault)),
                DescricaoDesconto = "Desconto",
                ValorDesconto = string.Format("(-) {0}", discount.ToString("C", AppDefaults.CultureInfoDefault)),
                DescricaoTituloTotal = "TOTAL",
                ValorTituloTotal = valorTituloTotalFormatado,
                Observacao = itemContaPagar.Observacao == null ? "" : itemContaPagar.Observacao,
                Numero = itemContaPagar.Numero.ToString()
            };

            var reportViewer = new WebReportViewer<ReciboContaFinanceiraVM>(ReportRecibo.Instance);
            return File(reportViewer.Print(itemRecibo, platformUrl: ""), "application/pdf");
        }

        public List<ContaPagarVM> GetListContaPagar(string queryStringOdata = "", string tipoStatus = "")
        {
            var queryString = new Dictionary<string, string>();
            var strStatusConta = !string.IsNullOrEmpty(queryStringOdata)
                ? $" and statusContaBancaria eq {AppDefaults.APIEnumResourceName}StatusContaBancaria" + "'" + tipoStatus + "'"
                : $" statusContaBancaria eq {AppDefaults.APIEnumResourceName}StatusContaBancaria" + "'" + tipoStatus + "'";

            if (string.IsNullOrEmpty(queryStringOdata) && string.IsNullOrEmpty(tipoStatus))
            {
                queryString.AddParam("$orderby", "numero");
                queryString.AddParam("$expand", "pessoa($select=nome),formaPagamento($select=descricao)");
            }
            else
            {
                queryString.AddParam("$filter", $"{queryStringOdata}" + (!string.IsNullOrEmpty(tipoStatus) ? strStatusConta : ""));
                queryString.AddParam("$orderby", "numero");
                queryString.AddParam("$expand", "pessoa($select=nome),formaPagamento($select=descricao)");
            }
            return RestHelper.ExecuteGetRequest<ResultBase<ContaPagarVM>>("ContaPagar", queryString).Data;
        }

        public virtual ActionResult ImprimirListContas(string queryStringOdata, string tipoStatus)
        {
            var contas = GetListContaPagar(queryStringOdata, tipoStatus);
            return base.PrintList(contas, "Lista de Contas a Pagar");
        }

        public override ContentResult List()
        {
            return ListContaPagar();
        }

        public List<HtmlUIButton> GetListButtonsOnHeaderCustom(string bntLabel, string btnOnClick)
        {
            var target = new List<HtmlUIButton>();

            target.Add(new HtmlUIButton { Id = "baixasBtn", Label = "Baixas múltiplas", OnClickFn = "fnBaixaMultipla", Position = HtmlUIButtonPosition.Out });
            //target.Add(new HtmlUIButton { Id = "newPrint", Label = "Imprimir", OnClickFn = "fnImprimirListContas", Position = HtmlUIButtonPosition.In });
            target.Add(new HtmlUIButton { Id = "filterGrid", Label = bntLabel, OnClickFn = btnOnClick, Position = HtmlUIButtonPosition.Out });
            target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo", Position = HtmlUIButtonPosition.Main });


            return target;
        }

        public ContentResult ListContaPagar(string gridLoad = "GridLoad")
        {
            var buttonLabel = "Todas as contas";
            var buttonOnClick = "fnRemoveFilter";

            if (Request.QueryString["action"] == "GridLoadNoFilter")
            {
                gridLoad = Request.QueryString["action"];
                buttonLabel = "Contas do mês";
                buttonOnClick = "fnAddFilter";
            }

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Contas a Pagar",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeaderCustom(buttonLabel, buttonOnClick))
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = { "fnCardList" },
                //ReadyFn = (Request.QueryString["action"] != "GridLoadNoFilter") ? "fnCardList" : ""
            };

            var cfgForm = new FormUI
            {
                Id = "fly01frm",
                UrlFunctions = Url.Action("Functions") + "?fns=",
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
                },
                Functions = { "fnRowCallbackContasFinanceiras" }
            };

            //if (Request.QueryString["action"] != "GridLoadNoFilter")
            //{
            //    cfg.Content.Add(new CardUI
            //    {
            //        Class = "col s12 m4 center",
            //        Color = "totvs-blue",
            //        Id = "fly01cardEmAberto",
            //        Placeholder = "A pagar"
            //    });
            //    cfg.Content.Add(new CardUI
            //    {
            //        Class = "col s12 m4 center",
            //        Color = "totvs-blue",
            //        Id = "fly01cardBaixadoParcialmente",
            //        Placeholder = "Baixado Parcialmente"
            //    });
            //    cfg.Content.Add(new CardUI
            //    {
            //        Class = "col s12 m4 center",
            //        Color = "totvs-blue",
            //        Id = "fly01cardPago",
            //        Placeholder = "Pago"
            //    });
            //}

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
                UrlGridLoad = Url.Action(gridLoad),
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() {Id = "dataInicial", Required = (gridLoad == "GridLoad") },
                    new DataTableUIParameter() {Id = "dataFinal", Required = (gridLoad == "GridLoad") }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" },
                Options = new DataTableUIConfig
                {
                    OrderColumn = 1,
                    OrderDir = "desc"
                },
                Callbacks = new DataTableUICallbacks()
                {
                    RowCallback = "fnRowCallbackContasFinanceiras"
                }
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "(row.statusEnum == 'EmAberto')" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "(row.statusEnum == 'EmAberto' && row.repeticaoPai == false && row.repeticaoFilha == false)" },
                new DataTableUIAction { OnClickFn = "fnExcluirRecorrencias", Label = "Excluir", ShowIf = "(row.statusEnum == 'EmAberto' && (row.repeticaoPai == true || row.repeticaoFilha == true))" },
                new DataTableUIAction { OnClickFn = "fnNovaBaixa", Label = "Nova baixa", ShowIf = "row.statusEnum == 'EmAberto' || row.statusEnum == 'BaixadoParcialmente'" },
                new DataTableUIAction { OnClickFn = "fnCancelarBaixas", Label = "Cancelar baixas", ShowIf = "row.statusEnum == 'Pago' || row.statusEnum == 'BaixadoParcialmente'" },
                new DataTableUIAction { OnClickFn = "fnImprimirRecibo", Label = "Emitir recibo", ShowIf = "row.statusEnum == 'Pago'" }
            }));

            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "statusContaBancaria",
                DisplayName = "Status",
                Priority = 2,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusContaBancaria))),
                RenderFn = "fnRenderEnum(full.statusContaBancariaCssClass, full.statusContaBancariaNomeCompleto)"
            });

            config.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Nº", Priority = 4, Type = "number" });
            config.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Vencimento", Priority = 6, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "pessoa_nome", DisplayName = "Fornecedor", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "valorPrevisto", DisplayName = "Valor", Priority = 5, Type = "currency" });
            config.Columns.Add(new DataTableUIColumn { DataField = "saldo", DisplayName = "Saldo", Priority = 3, Orderable = false, Searchable = false });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricaoParcela", DisplayName = "Parcela", Priority = 8 });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 7 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
                filters = new Dictionary<string, string>();

            if (Request.QueryString["dataFinal"] != "")
                filters.Add("dataVencimento le ", Request.QueryString["dataFinal"]);
            if (Request.QueryString["dataInicial"] != "")
                filters.Add(" and dataVencimento ge ", Request.QueryString["dataInicial"]);

            return base.GridLoad(filters);
        }

        public JsonResult GridLoadNoFilter()
        {
            return GridLoad();
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();
            target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out });
            target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvarRecorrencia", Type = "submit", Position = HtmlUIButtonPosition.Main });

            return target;
        }

        protected override ContentUI FormJson()
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
                    Title = "Conta a Pagar",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnSalvar", "fnSimulaParcelamento" }
            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = Url.Action("List"),
                    Form = @Url.Action("Form")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Id = "fly01frmContaPagar",
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "statusContaBancaria" });
            config.Elements.Add(new InputHiddenUI { Id = "descricaoParcela" });
            config.Elements.Add(new InputHiddenUI { Id = "repeticaoPai" });
            config.Elements.Add(new InputHiddenUI { Id = "repeticaoFilha" });
            config.Elements.Add(new InputHiddenUI { Id = "contaFinanceiraRepeticaoPaiId" });

            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 l6", Label = "Descrição", Required = true, MaxLength = 150 });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "pessoaId",
                Class = "col s12 l6",
                Label = "Fornecedor",
                Required = true,
                DataUrl = @Url.Action("Fornecedor", "AutoComplete"),
                LabelId = "pessoaNome",
                DataUrlPost = Url.Action("PostFornecedor", "Fornecedor")
            });

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorPrevisto",
                Class = "col s6 l2",
                Label = "Valor",
                Required = true,
                Value = "0",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnHideSimulacaoCondicao" }
                }
            });
            config.Elements.Add(new InputDateUI
            {
                Id = "dataEmissao",
                Class = "col s6 l2",
                Label = "Emissão",
                Required = true,
                Value = DateTime.Now.ToString("dd/MM/yyyy")
            });
            config.Elements.Add(new InputDateUI
            {
                Id = "dataVencimento",
                Class = "col s12 l2",
                Label = "Data Referência",
                Required = true,
                Value = DateTime.Now.ToString("dd/MM/yyyy"),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeVencimento" }
                }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 l6",
                Label = "Condição Parcelamento",
                Required = true,
                DataUrl = @Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CondicaoParcelamento", new { readyFn = "fnFormReadyOnDemandContaPagar" }),
                DataPostField = "descricao",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeCondicaoParcelamento" }
                }
            });

            config.Elements.Add(new InputHiddenUI { Id = "condicaoParcelamentoCondicoesParcelamento" });
            config.Elements.Add(new InputHiddenUI { Id = "condicaoParcelamentoQtdParcelas" });

            config.Elements.Add(new DivElementUI { Id = "collapseSimulacao", Class = "col s12 visible" });

            config.Elements.Add(new ButtonUI
            {
                Id = "btnAtualizaSimulacao",
                Class = "col s12",
                Value = "Simular",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "click", Function = "fnSimulaParcelamento" }
                }
            });
            config.Elements.Add(new TableUI
            {
                Id = "dtSimulacaoParcelamento",
                Class = "col s12",
                Label = "Simulação",
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Parcela", Value = "0"},
                    new OptionUI { Label = "Data", Value = "1"},
                    new OptionUI { Label = "Valor",Value = "2"}
                }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 m6",
                Label = "Forma Pagamento",
                Required = true,
                DataUrl = @Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "FormaPagamento"),
                DataPostField = "descricao"
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m6",
                Label = "Categoria Financeira",
                Required = true,
                DataUrl = @Url.Action("CategoriaCP", "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = Url.Action("NovaCategoriaDespesa")
            });

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "repetir",
                Class = "col s12",
                Label = "Repetir",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChkRepetir" }
                }
            });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoPeriodicidade",
                Class = "col s6 m3",
                Label = "Periodicidade",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoPeriodicidade), false, "Mensal")),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeTipoPeriodicidade" }
                }
            });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoRepeticao",
                Class = "col s6 m3",
                Label = "Repetição",
                Options = new List<SelectOptionUI>
                {
                    new SelectOptionUI {Label= "Quantidade", Value = "Q", Selected = true},
                    new SelectOptionUI {Label= "Período", Value = "P"}
                },
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeTipoRepeticao" }
                }
            });
            config.Elements.Add(new InputTextUI
            {
                Id = "numeroRepeticoes",
                Class = "col s6 m3",
                Label = "Número de Repetições",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeNumeroRepeticoes" }
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "numeroRepeticoes",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Será gerado um registro principal mais o numero de repetições. Este total de recorrências também será multiplicado pelo número de parcelas da condição de parcelamento.",
                    Position = TooltopUIPosition.Top
                }
            });
            config.Elements.Add(new InputDateUI
            {
                Id = "periodoFim",
                Class = "col s6 m3",
                Label = "Fim",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangePeriodoFim" }
                }
            });

            cfg.Content.Add(config);

            return cfg;
        }

        public ContentResult ModalNovoAdiantamento()
        {
            var config = new ModalUIForm()
            {
                Title = "Novo Adiantamento",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction { Label = "Confirmar" },
                CancelAction = new ModalUIAction { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("IncluirAdiantamento"),
                    List = @Url.Action("ContaPagar")
                }
            };

            config.Elements.Add(new InputHiddenUI { Id = "Id" });
            config.Elements.Add(new InputTextUI { Id = "Descricao", Class = "col s12 l3", Label = "Descrição" });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "PessoaId",
                Class = "col s8 l7",
                Label = "Pessoa",
                MaxLength = 35,
                DataUrl = @Url.Action("Person", "AutoComplete", "Person"),
                LabelId = "Pessoa"
            });

            config.Elements.Add(new InputTextUI { Id = "Valor", Class = "col s4 l2", Label = "Valor" });
            config.Elements.Add(new InputTextUI { Id = "Categoria", Class = "col s6 l6", Label = "Categoria" });
            config.Elements.Add(new InputTextUI { Id = "Banco", Class = "col s6 l6", Label = "Banco" });
            config.Elements.Add(new TextAreaUI { Id = "Observacao", Class = "col s12 l12", Label = "Observação", MaxLength = 200 });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public override JsonResult Delete(Guid id)
        {
            if (Request.QueryString["excluirRecorrencias"] == "true")
            {
                var queryString = new Dictionary<string, string>
                {
                    { "excluirRecorrencias", "true" }
                };

                RestHelper.ExecuteDeleteRequest($"{AppDefaults.UrlFinanceiroApi}",
                                                $"{ResourceName}/{id}/",
                                                null,
                                                queryString);
                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Delete);
            }

            return base.Delete(id);
        }

        public override JsonResult Edit(ContaPagarVM entityVM)
        {
            if (Request.QueryString["editarRecorrencias"] == "true")
            {
                var queryString = new Dictionary<string, string>
                {
                    { "editarRecorrencias", "true" }
                };

                var resourceNamePut = $"{ResourceName}/{entityVM.Id}";
                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Edit), queryString);

                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Delete);
            }
            return base.Edit(entityVM);
        }

        #region OnDemmand

        [HttpPost]
        public JsonResult NovaCategoriaDespesa(string term)
        {
            return NovaCategoria(new CategoriaVM { Descricao = term, TipoCarteira = "2" });
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
            {
                return "F";
            }

            if (documento.Length > 11)
            {
                return "J";
            }

            return null;
        }

        public JsonResult LoadCards(DateTime? dataFinal = null, DateTime? dataInicial = null)
        {
            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                    { "dataFinal", dataFinal.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd") },
                    { "dataInicial", dataInicial.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd") }
                };
                var response = RestHelper.ExecuteGetRequest<List<ContaFinanceiraPorStatusVM>>("DashboardContaPagarDia", queryString);
                var emAberto = "R$ 0,00";
                var pago = "R$ 0,00";
                var baixadoParcialmente = "R$ 0,00";

                foreach (var item in response)
                {
                    if (item.Status == "Em aberto") emAberto = item.Valortotal.ToString("C", AppDefaults.CultureInfoDefault);
                    else if (item.Status == "Pago") pago = item.Valortotal.ToString("C", AppDefaults.CultureInfoDefault);
                    else if (item.Status == "Baixado Parcialmente") baixadoParcialmente = item.Valortotal.ToString("C", AppDefaults.CultureInfoDefault);
                }

                var responseToView = new
                {
                    emAberto,
                    pago,
                    baixadoParcialmente
                };

                return Json(responseToView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonResponseStatus.GetFailure(ex.Message);
            }
        }

        #endregion
    }
}