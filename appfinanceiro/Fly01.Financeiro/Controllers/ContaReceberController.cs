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
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroFinanceiroContasReceber)]
    public class ContaReceberController : ContaFinanceiraController<ContaReceberVM, ContaFinanceiraBaixaVM, ContaFinanceiraRenegociacaoVM>
    {
        public ContaReceberController()
        {
            ExpandProperties = "condicaoParcelamento($select=descricao,qtdParcelas,condicoesParcelamento),pessoa($select=nome),categoria($select=descricao),formaPagamento($select=descricao,tipoFormaPagamento),centroCusto";
        }

        public override ActionResult ImprimirRecibo(Guid id)
        {
            ContaReceberVM itemContaReceber = Get(id);

            double discount = 0;
            double interest = 0;
            double total = 0;
            string valorTituloTotalFormatado = string.Empty;

            var managerEmpresaVM = GetDadosEmpresa();
            total = itemContaReceber.ValorPago.Value;
            string valorTituloFormatado = total.ToString("C", AppDefaults.CultureInfoDefault);

            StringBuilder assinatura = new StringBuilder();
            assinatura.Append("_________________________________________________________");
            assinatura.Append("<br/>");
            assinatura.Append(SessionManager.Current.UserData.TokenData.UserName);

            ReciboContaFinanceiraVM itemRecibo = new ReciboContaFinanceiraVM
            {
                Id = itemContaReceber.Id.ToString(),
                Conteudo = String.Format("Recebemos de {0} o pagamento de {1} ({2}) referente à:", itemContaReceber.Pessoa.Nome, valorTituloTotalFormatado, itemContaReceber.ValorPago.Value.toExtenso()),
                DescricaoTitulo = !String.IsNullOrWhiteSpace(itemContaReceber.Descricao) ? itemContaReceber.Descricao : itemContaReceber.Categoria.Descricao,
                ValorTitulo = valorTituloFormatado,
                DataAtual = String.Format("{0}, {1} de {2} de {3}", managerEmpresaVM.Cidade != null ? managerEmpresaVM.Cidade.Nome : "", DateTime.Now.Day, AppDefaults.CultureInfoDefault.DateTimeFormat.GetMonthName(DateTime.Now.Month), DateTime.Now.Year),
                Assinatura = assinatura.ToString(),
                DescricaoJuros = "Juros",
                ValorJuros = string.Format("(+) {0}", interest.ToString("C", AppDefaults.CultureInfoDefault)),
                DescricaoDesconto = "Desconto",
                ValorDesconto = string.Format("(-) {0}", discount.ToString("C", AppDefaults.CultureInfoDefault)),
                DescricaoTituloTotal = "TOTAL",
                ValorTituloTotal = valorTituloTotalFormatado,
                Observacao = itemContaReceber.Observacao,
                Numero = itemContaReceber.Numero.ToString()
            };

            var reportViewer = new WebReportViewer<ReciboContaFinanceiraVM>(ReportRecibo.Instance);
            return File(reportViewer.Print(itemRecibo, SessionManager.Current.UserData.PlatformUrl), "application/pdf");
        }

        public override string GetResourceDeleteTituloBordero(string id)
        {
            return String.Format("AccountReceivableRemoveBordero/{0}", id);
        }

        public override JsonResult GridLoadTitulosARenegociar(string renegociacaoPessoaId)
        {
            try
            {
                var filters = new Dictionary<string, string>()
                {
                    {
                        "pessoaId",
                        $" eq {renegociacaoPessoaId} and ativo and (statusContaBancaria eq {AppDefaults.APIEnumResourceName}StatusContaBancaria'EmAberto' or statusContaBancaria eq {AppDefaults.APIEnumResourceName}StatusContaBancaria'BaixadoParcialmente')"
                    }
                };
                return base.GridLoad(filters);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = string.Format("Ocorreu um erro ao carregar dados: {0}", ex.Message) }, JsonRequestBehavior.AllowGet);
            }
        }

        public List<ContaReceberVM> GetListContaPagar(string queryStringOdata = "", string tipoStatus = "")
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

            return RestHelper.ExecuteGetRequest<ResultBase<ContaReceberVM>>("ContaReceber", queryString).Data;
        }

        public virtual ActionResult ImprimirListContas(string queryStringOdata, string tipoStatus)
        {
            var contas = GetListContaPagar(queryStringOdata, tipoStatus);
            return base.PrintList(contas, "Lista de Contas a Receber");
        }

        public override JsonResult ListRenegociacaoRelacionamento(string contaFinanceiraId)
        {
            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>();
                queryString.AddParam("contaFinanceiraId", contaFinanceiraId);

                var response = RestHelper.ExecuteGetRequest<RenegociacaoRelacionamentoContaReceberVM>("RenegociacaoContasRelacionamento", queryString);

                return Json(new
                {
                    success = true,
                    recordsTotal = response.Data.Count,
                    recordsFiltered = response.Data.Count,
                    renegociacao = response.Renegociacao,
                    data = response.Data.Select(GetDisplayDataRenegociacao())
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = string.Format("Ocorreu um erro ao carregar dados: {0}", ex.Message) }, JsonRequestBehavior.AllowGet);
            }
        }

        public override ContentResult List()
        {
            return ListContaReceber();
        }

        public List<HtmlUIButton> GetListButtonsOnHeaderCustom(string bntLabel, string btnOnClick)
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "baixasBtn", Label = "Baixas múltiplas", OnClickFn = "fnBaixaMultipla", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "renegociacaoBtn", Label = "Renegociação", OnClickFn = "fnNovaRenegociacaoCR", Position = HtmlUIButtonPosition.In });
                target.Add(new HtmlUIButton { Id = "printBtn", Label = "Imprimir", OnClickFn = "fnImprimirListContas", Position = HtmlUIButtonPosition.In });
                target.Add(new HtmlUIButton { Id = "filterGrid", Label = bntLabel, OnClickFn = btnOnClick, Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo", Position = HtmlUIButtonPosition.Main });
            }

            return target;
        }

        public ContentResult ListContaReceber(string gridLoad = "GridLoad")
        {
            var buttonLabel = "Todas as contas";
            var buttonOnClick = "fnRemoveFilter";

            if (Request.QueryString["action"] == "GridLoadNoFilter")
            {
                gridLoad = Request.QueryString["action"];
                buttonLabel = "Contas do mês";
                buttonOnClick = "fnAddFilter";
            }

            ContentUI cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Contas a Receber",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeaderCustom(buttonLabel, buttonOnClick))
                },
                UrlFunctions = Url.Action("Functions", "ContaReceber", null, Request.Url.Scheme) + "?fns=",
                Functions = { "fnCardList" },
                ReadyFn = (Request.QueryString["action"] != "GridLoadNoFilter") ? "fnCardList" : ""
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

            if (Request.QueryString["action"] != "GridLoadNoFilter")
            {
                cfg.Content.Add(new CardUI
                {
                    Class = "col s12 m4 center",
                    Color = "totvs-blue",
                    Id = "fly01cardEmAberto",
                    Placeholder = "A Receber"
                });
                cfg.Content.Add(new CardUI
                {
                    Class = "col s12 m4 center",
                    Color = "totvs-blue",
                    Id = "fly01cardPago",
                    Placeholder = "Pago"
                });
                cfg.Content.Add(new CardUI
                {
                    Class = "col s12 m4 center",
                    Color = "totvs-blue",
                    Id = "fly01cardRenegociado",
                    Placeholder = "Renegociado"
                });
            }

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
            DataTableUI config = new DataTableUI
            {
                Id = "fly01dt",
                UrlGridLoad = Url.Action(gridLoad),
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() {Id = "dataInicial", Required = (gridLoad == "GridLoad") },
                    new DataTableUIParameter() {Id = "dataFinal", Required = (gridLoad == "GridLoad") }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum", "fnImprimirBoleto" },
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
                new DataTableUIAction { OnClickFn = "fnVisualizar", Label = "Visualizar" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "(row.statusEnum == 'EmAberto' && row.repeticaoPai == false && row.repeticaoFilha == false)" },
                new DataTableUIAction { OnClickFn = "fnExcluirRecorrencias", Label = "Excluir", ShowIf = "(row.statusEnum == 'EmAberto' && (row.repeticaoPai == true || row.repeticaoFilha == true))" },
                new DataTableUIAction { OnClickFn = "fnNovaBaixa", Label = "Nova baixa", ShowIf = "row.statusEnum == 'EmAberto' || row.statusEnum == 'BaixadoParcialmente'" },
                new DataTableUIAction { OnClickFn = "fnCancelarBaixas", Label = "Cancelar baixas", ShowIf = "row.statusEnum == 'Pago' || row.statusEnum == 'BaixadoParcialmente'" },
                new DataTableUIAction { OnClickFn = "fnImprimirRecibo", Label = "Emitir recibo", ShowIf = "row.statusEnum == 'Pago'" },
                new DataTableUIAction { OnClickFn = "fnModalImprimeBoleto", Label = "Imprimir boleto", ShowIf = "row.statusEnum != 'Pago' && row.formaPagamento == 'Boleto'" },
                new DataTableUIAction { OnClickFn = "fnEditImprimeBoleto", Label = "Imprimir boleto", ShowIf = "row.statusEnum != 'Pago' && row.formaPagamento != 'Boleto'" }
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
            config.Columns.Add(new DataTableUIColumn { DataField = "pessoa_nome", DisplayName = "Cliente", Priority = 1 });
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

            //Request.QueryString

            return base.GridLoad(filters);
        }

        public JsonResult GridLoadNoFilter()
        {
            return GridLoad();
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "saveNew", Label = "Salvar e Novo", OnClickFn = "fnSalvarEnovo", Type = "submit", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvarRecorrencia", Type = "submit", Position = HtmlUIButtonPosition.Main });
            }

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
                    Title = "Dados do título a receber",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions", "ContaReceber", null, Request.Url.Scheme) + "?fns=",
                Functions = new List<string> { "fnSalvar" }
            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List"),
                    Form = @Url.Action("Form")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions", "ContaReceber", null, Request.Url.Scheme) + "?fns=",
                Id = "fly01frmContaReceber",
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "statusContaBancaria" });
            config.Elements.Add(new InputHiddenUI { Id = "descricaoParcela" });
            config.Elements.Add(new InputHiddenUI { Id = "repeticaoPai" });
            config.Elements.Add(new InputHiddenUI { Id = "repeticaoFilha" });
            config.Elements.Add(new InputHiddenUI { Id = "contaFinanceiraRepeticaoPaiId" });

            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 l6", Label = "Descrição", Required = true, MaxLength = 150 });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "pessoaId",
                Class = "col s12 l6",
                Label = "Cliente",
                Required = true,
                DataUrl = Url.Action("Cliente", "AutoComplete"),
                LabelId = "pessoaNome",
                DataUrlPost = Url.Action("PostCliente", "Cliente")
            }, ResourceHashConst.FinanceiroCadastrosClientes));

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

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 l6",
                Label = "Condição Parcelamento",
                Required = true,
                DataUrl = @Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CondicaoParcelamento", new { readyFn = "fnFormReadyOnDemandContaReceber" }),
                DataPostField = "descricao",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeCondicaoParcelamento" }
                }
            }, ResourceHashConst.FinanceiroCadastrosCondicoesParcelamento));

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

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 m4",
                Label = "Forma Pagamento",
                Required = true,
                DataUrl = @Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "FormaPagamento"),
                DataPostField = "descricao"
            }, ResourceHashConst.FinanceiroCadastrosFormasPagamento));


            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m4",
                Label = "Categoria Financeira",
                Required = true,
                DataUrl = @Url.Action("CategoriaCR", "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = Url.Action("NovaCategoriaReceita")
            }, ResourceHashConst.FinanceiroCadastrosCategoria));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "centroCustoId",
                Class = "col s12 m4",
                Label = "Centro de Custo",
                DataUrl = @Url.Action("CentroCusto", "AutoComplete"),
                LabelId = "centroCustoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CentroCusto"),
                DataPostField = "descricao"
            }, ResourceHashConst.FinanceiroCadastrosCentroCustos));

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "repetir",
                Class = "col s6",
                Label = "Repetir",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChkRepetir" }
                }
            });

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "baixarTitulo",
                Class = "col s6",
                Label = "Marcar título como recebido?",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChkBaixar" }
                }
            });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoPeriodicidade",
                Class = "col s6 m3 l3",
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
                Class = "col s6 m3 l3",
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
                Class = "col s6 m3 l3",
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
                Class = "col s6 m3 l3",
                Label = "Fim",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangePeriodoFim" }
                }
            });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "contaBancariaId",
                Class = "col s12",
                Label = "Conta Bancária",
                Required = true,
                DataUrl = @Url.Action("ContaBancariaBanco", "AutoComplete"),
                LabelId = "contaBancariaNomeConta",
                DataUrlPostModal = @Url.Action("FormModal", "ContaBancaria"),
                DataPostField = "nomeConta",
            }, ResourceHashConst.FinanceiroCadastrosContasBancarias));

            cfg.Content.Add(config);
            return cfg;
        }

        public List<HtmlUIButton> GetFormButtonsRenegociacaoOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" });
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" });
            }

            return target;
        }

        public ContentResult FormRenegociacaoContaReceber()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Renegociacao"),
                    WithParams = Url.Action("Renegociacao")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Dados da renegociação",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsRenegociacaoOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Id = "fly01frmCabecalho",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "renegociacaoPessoaId",
                Class = "col s12 m9 l9",
                Label = "Cliente",
                Required = true,
                DataUrl = @Url.Action("Cliente", "AutoComplete"),
                LabelId = "clienteNome",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "autocompleteselect", Function = "fnChangeRenegociacaoPessoa" }
                }
            });
            config.Elements.Add(new ButtonUI
            {
                Id = "btnListaTitulos",
                Class = "col s12 m3 l3 visible",
                Value = "Listar contas",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "click", Function = "fnListaTitulosCR" }
                }
            });

            config.Elements.Add(new LabelSetUI { Id = "selecaoTitulosLabel", Class = "col s12", Label = "Seleção das contas" });

            cfg.Content.Add(config);

            DataTableUI dtcfg = new DataTableUI
            {
                Id = "datatableSelecaoTitulos",
                UrlGridLoad = Url.Action("GridLoadTitulosARenegociar"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Options = new DataTableUIConfig() { Select = new { style = "multi" } }
            };

            dtcfg.Parameters.Add(new DataTableUIParameter()
            {
                Id = "renegociacaoPessoaId",
                Required = true
            });

            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição" });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "valorPrevisto", DisplayName = "Valor", Type = "currency" });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "saldo", DisplayName = "Saldo", Type = "currency", Orderable = false, Searchable = false });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "descricaoParcela", DisplayName = "Parcelas" });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Vencimento", Type = "date" });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "diasVencidos", DisplayName = "Dias Vencidos", Orderable = false, Searchable = false });

            cfg.Content.Add(dtcfg);

            var config2 = new FormUI()
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = @Url.Action("IncluirRenegociacao"),
                    Get = @Url.Action("IncluirRenegociacao"),
                    List = Url.Action("List")
                },
                ReadyFn = "fnFormReadyRenegociacao",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config2.Elements.Add(new InputHiddenUI { Id = "pessoaId" });
            config2.Elements.Add(new InputHiddenUI { Id = "tipoContaFinanceira", Value = "ContaReceber" });
            config2.Elements.Add(new InputHiddenUI { Id = "contasFinanceirasIds" });

            config2.Elements.Add(new LabelSetUI { Id = "informacoesRenegociacaoLabel", Class = "col s12", Label = "Informações da renegociação" });

            config2.Elements.Add(new InputTextUI { Id = "motivo", Class = "col s12 m9 l9", Label = "Motivo", Required = true, MaxLength = 200 });
            config2.Elements.Add(new InputCurrencyUI { Id = "valorAcumulado", Class = "col s12 m3 l3", Label = "Valor Acumulado", Readonly = true });

            config2.Elements.Add(new SelectUI
            {
                Id = "tipoRenegociacaoValorDiferenca",
                Class = "col s12 m3 l3",
                Label = "Tipo do Valor Diferença",
                Options = new List<SelectOptionUI>
                {
                    new SelectOptionUI {Label= "Acréscimo", Value = "Acrescimo", Selected = true},
                    new SelectOptionUI {Label= "Desconto", Value = "Desconto"}
                },
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnRenegociacaoCalculaValorFinal" }
                }
            });
            config2.Elements.Add(new SelectUI
            {
                Id = "tipoRenegociacaoCalculo",
                Class = "col s12 m3 l3",
                Label = "Tipo Cálculo Diferença",
                Options = new List<SelectOptionUI>
                {
                    new SelectOptionUI {Label= "Valor", Value = "Valor", Selected = true},
                    new SelectOptionUI {Label= "Percentual", Value = "Percentual"}
                },
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnRenegociacaoCalculaValorFinal" }
                }
            });
            config2.Elements.Add(new InputFloatUI
            {
                Id = "valorDiferenca",
                Class = "col s12 m3 l3",
                Label = "Valor Diferença",
                Required = true,
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnRenegociacaoCalculaValorFinal" }
                }
            });
            config2.Elements.Add(new InputCurrencyUI { Id = "valorFinal", Class = "col s12 m3 l3", Label = "Valor Final ", Readonly = true });

            config2.Elements.Add(new LabelSetUI { Id = "informacoesNovoTituloLabel", Class = "col s12", Label = "Informações da nova conta a receber" });

            config2.Elements.Add(new InputTextUI { Id = "descricaoRenegociacao", Class = "col s12 m6 l6", Label = "Descrição", Required = true, MaxLength = 200 });
            config2.Elements.Add(new InputDateUI { Id = "dataEmissao", Class = "col s12 m3 l3", Label = "Emissão", Required = true });
            config2.Elements.Add(new InputDateUI { Id = "dtVencimento", Class = "col s12 m3 l3", Label = "Data Referência", Required = true });
            config2.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 l6",
                Label = "Forma Pagamento",
                Required = true,
                DataUrl = @Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "FormaPagamento"),
                DataPostField = "descricao"
            }, ResourceHashConst.FinanceiroCadastrosFormasPagamento));

            config2.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 l6",
                Label = "Condição Parcelamento",
                Required = true,
                DataUrl = @Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CondicaoParcelamento"),
                DataPostField = "descricao"
            }, ResourceHashConst.FinanceiroCadastrosCondicoesParcelamento));

            config2.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 l6",
                Label = "Categoria Financeira",
                Required = true,
                DataUrl = @Url.Action("CategoriaCR", "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = Url.Action("NovaCategoriaReceita")
            }, ResourceHashConst.FinanceiroCadastrosCategoria));

            config2.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "centroCustoId",
                Class = "col s12 m6",
                Label = "Centro de Custo",
                DataUrl = @Url.Action("CentroCusto", "AutoComplete"),
                LabelId = "centroCustoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CentroCusto"),
                DataPostField = "descricao"
            }, ResourceHashConst.FinanceiroCadastrosCentroCustos));

            cfg.Content.Add(config2);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public ContentResult FormImprimeBoleto(string contaReceberId, string formaPagamentoDescricao)
        {
            var config = new ModalUIForm()
            {
                Title = "Selecionar Conta Bancária",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Imprimir", OnClickFn = "fnImprimirBoletoDeContasReceber" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("ImprimeBoleto", "Cnab") + $"?contaReceberId={contaReceberId}&contaBancariaId=como pegar o id da conta bancária aqui?"
                },
                Id = "fly01mdlfrmSelecionaContaBancaria"
            };
            config.Elements.Add(new InputHiddenUI { Id = "contaReceberId", Value = contaReceberId });
            config.Elements.Add(new InputHiddenUI { Id = "descricaoFormaPagamento", Value = formaPagamentoDescricao });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "contaBancariaId",
                Class = "col s12",
                Label = "Conta bancária cedente",
                Required = true,
                DataUrl = @Url.Action("ContaBancariaBancoEmiteBoleto", "AutoComplete") + "?emiteBoleto=true",
                LabelId = "bancoNome"
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public override JsonResult Delete(Guid id)
        {
            if (Request.QueryString["excluirRecorrencias"] == "true")
            {
                var queryString = new Dictionary<string, string>
                {
                    { "excluirRecorrencias", "true" }
                };

                RestHelper.ExecuteDeleteRequest($"{AppDefaults.UrlApiGateway}",
                    $"{ResourceName}/{id}/",
                    null,
                    queryString);
                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Delete);
            }

            return base.Delete(id);
        }

        public override JsonResult Edit(ContaReceberVM entityVM)
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
                var response = RestHelper.ExecuteGetRequest<List<ContaFinanceiraPorStatusVM>>("dashboardcontareceberdia", queryString);
                var emAberto = "R$ 0,00";
                var pago = "R$ 0,00";
                var renegociado = "R$ 0,00";
                var baixadoParcialmente = "R$ 0,00";

                foreach (var item in response)
                {
                    if (item.Status == "Em aberto") emAberto = item.Valortotal.ToString("C", AppDefaults.CultureInfoDefault);
                    else if (item.Status == "Pago") pago = item.Valortotal.ToString("C", AppDefaults.CultureInfoDefault);
                    else if (item.Status == "Renegociado") renegociado = item.Valortotal.ToString("C", AppDefaults.CultureInfoDefault);
                    else if (item.Status == "Baixado Parcialmente") baixadoParcialmente = item.Valortotal.ToString("C", AppDefaults.CultureInfoDefault);
                }

                var responseToView = new
                {
                    emAberto,
                    pago,
                    renegociado,
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