using Fly01.Financeiro.Controllers.Base;
using Fly01.Financeiro.ViewModel;
using Fly01.Financeiro.Models.Reports;
using Fly01.Financeiro.Models.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Financeiro.Controllers
{
    public class ContaReceberController : ContaFinanceiraController<ContaReceberVM, ContaFinanceiraBaixaVM, ContaFinanceiraRenegociacaoVM>
    {
        public ContaReceberController()
        {
            ExpandProperties = "condicaoParcelamento($select=id,descricao),pessoa($select=id,nome),categoria($select=id,descricao),formaPagamento($select=descricao)";
        }

        public override ActionResult ImprimirRecibo(Guid id)
        {
            ContaReceberVM itemContaReceber = Get(id);
            //ver??
            //BankTransacVM itemBankTransac = GetBankTransac(id);

            double discount = 0;
            double interest = 0;
            double total = 0;
            string valorTituloTotalFormatado = string.Empty;

            //if (itemBankTransac != null)
            //{
            //    discount = itemBankTransac.Discount.HasValue ? itemBankTransac.Discount.Value : 0;
            //    interest = itemBankTransac.Interest.HasValue ? itemBankTransac.Interest.Value : 0;
            //    total = itemBankTransac.Value + discount - interest;
            //    valorTituloTotalFormatado = itemBankTransac.Value.ToString("C", AppDefaults.CultureInfoDefault);
            //}

            var managerEmpresaVM = GetDadosEmpresa();
            total = itemContaReceber.ValorPago.Value;
            string valorTituloFormatado = total.ToString("C", AppDefaults.CultureInfoDefault);

            StringBuilder assinatura = new StringBuilder();
            assinatura.Append("_________________________________________________________");
            assinatura.Append("<br/>");
            assinatura.Append(SessionManager.Current.UserData.TokenData.Username);

            ReciboContaFinanceiraVM itemRecibo = new ReciboContaFinanceiraVM
            {
                Id = itemContaReceber.Id.ToString(),
                //Conteudo = String.Format("Recebemos de {0} o pagamento de {1} ({2}) referente à:", itemContaReceber.Pessoa, valorTituloTotalFormatado, itemBankTransac.Value.toExtenso()),
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
            //var report = ReportViewerHelper<ReciboContaFinanceiraVM>.GetReport(ReportRecibo.Instance, itemRecibo, SessionManager.Current.UserData.TokenData.Username, "");
            //byte[] data = ReportViewerHelper<ReciboContaFinanceiraVM>.PrepareReportToPrint(report);
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

        public List<ContaReceberVM> GetListContaPagar(string queryStringOdata, string tipoStatus)
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

        public ContentResult ListContaReceber(string gridLoad = "GridLoad")
        {
            var buttonLabel = "Mostrar todas as contas";
            var buttonOnClick = "fnRemoveFilter";

            if (Request.QueryString["action"] == "GridLoadNoFilter")
            {
                gridLoad = Request.QueryString["action"];
                buttonLabel = "Mostrar contas do mês atual";
                buttonOnClick = "fnAddFilter";
            }

            ContentUI cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Contas a Receber",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" },
                        new HtmlUIButton { Id = "baixasBtn", Label = "Baixas múltiplas", OnClickFn = "fnBaixaEmLote" },
                        new HtmlUIButton { Id = "renegociacaoBtn", Label = "Renegociação", OnClickFn = "fnNovaRenegociacaoCR" },
                        new HtmlUIButton { Id = "printBtn", Label = "Imprimir", OnClickFn = "fnImprimirListContas" },
                        new HtmlUIButton { Id = "filterGrid", Label = buttonLabel, OnClickFn = buttonOnClick },
                    }
                },
                UrlFunctions = Url.Action("Functions", "ContaReceber", null, Request.Url.Scheme) + "?fns="
            };

            if (gridLoad == "GridLoad")
            {
                var cfgForm = new FormUI
                {
                    ReadyFn = "fnUpdateDataFinal",
                    UrlFunctions = Url.Action("Functions") + "?fns=",
                    Elements = new List<BaseUI>()
                    {
                        new PeriodpickerUI()
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
                        },
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
                    }
                };

                cfg.Content.Add(cfgForm);
            }

            DataTableUI config = new DataTableUI
            {
                UrlGridLoad = Url.Action(gridLoad),
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() {Id = "dataInicial", Required = (gridLoad == "GridLoad") },
                    new DataTableUIParameter() {Id = "dataFinal", Required = (gridLoad == "GridLoad") }
                },
                UrlFunctions = Url.Action("Functions", "ContaReceber", null, Request.Url.Scheme) + "?fns="
            };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "(row.statusEnum == 'EmAberto')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnVisualizar", Label = "Visualizar" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "(row.statusEnum == 'EmAberto')" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnNovaBaixa", Label = "Nova baixa", ShowIf = "row.statusEnum == 'EmAberto' || row.statusEnum == 'BaixadoParcialmente'" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnCancelarBaixas", Label = "Cancelar baixas", ShowIf = "row.statusEnum == 'Pago' || row.statusEnum == 'BaixadoParcialmente'" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnImprimirRecibo", Label = "Emitir recibo", ShowIf = "row.statusEnum == 'Pago'" });

            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "statusContaBancaria",
                DisplayName = "Status",
                Priority = 0,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusContaBancaria))),
                RenderFn = "function(data, type, full, meta) { return \"<span class=\\\"new badge \" + full.statusContaBancariaCssClass + \" left\\\" data-badge-caption=\\\" \\\">\" + full.statusContaBancaria + \"</span>\" }"
            });

            config.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Nº", Priority = 1, Type = "number" });
            config.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Vencimento", Priority = 2, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 3 });
            config.Columns.Add(new DataTableUIColumn { DataField = "valorPrevisto", DisplayName = "Valor", Priority = 4, Type = "currency" });
            config.Columns.Add(new DataTableUIColumn { DataField = "saldo", DisplayName = "Saldo", Priority = 5, Orderable = false, Searchable = false });
            //config.Columns.Add(new DataTableUIColumn { DataField = "formaPagamento_descricao", DisplayName = "Forma", Priority = 6, Orderable = false });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricaoParcela", DisplayName = "Parcela", Priority = 7 });
            config.Columns.Add(new DataTableUIColumn { DataField = "pessoa_nome", DisplayName = "Cliente", Priority = 8 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
                filters = new Dictionary<string, string>();

            filters.Add("dataVencimento le ", Request.QueryString["dataFinal"]);
            filters.Add(" and dataVencimento ge ", Request.QueryString["dataInicial"]);

            return base.GridLoad(filters);
        }

        public JsonResult GridLoadNoFilter()
        {
            return base.GridLoad();
        }

        public override ContentResult Form()
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
                    Title = "Dados do título a receber",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" },
                        new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "ContaReceber", null, Request.Url.Scheme) + "?fns="
            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions", "ContaReceber", null, Request.Url.Scheme) + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "statusContaBancaria" });
            config.Elements.Add(new InputHiddenUI { Id = "descricaoParcela" });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 l6", Label = "Descrição", Required = true });

            config.Elements.Add(new AutocompleteUI
            {
                Id = "pessoaId",
                Class = "col s12 l6",
                Label = "Cliente",
                Required = true,
                DataUrl = Url.Action("Cliente", "AutoComplete"),
                LabelId = "pessoaNome",
                DataUrlPost = Url.Action("PostCliente", "Cliente")
            });
            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorPrevisto",
                Class = "col s6 l2",
                Label = "Valor",
                Required = true,
                Value = "0"
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
                Label = "Vencimento",
                Required = true,
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeVencimento" }
                }
            });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 l6",
                Label = "Forma Pagamento",
                Required = true,
                DataUrl = @Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "FormaPagamento"),
                DataPostField = "descricao"
            });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 l6",
                Label = "Condição Parcelamento",
                Required = true,
                DataUrl = @Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CondicaoParcelamento"),
                DataPostField = "descricao"
            });
            config.Elements.Add(new AutocompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 l6",
                Label = "Categoria Financeira",
                Required = true,
                DataUrl = @Url.Action("CategoriaCR", "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = Url.Action("NovaCategoriaReceita")
            });

            config.Elements.Add(new TextareaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });

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

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public ContentResult FormRenegociacaoContaReceber()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Renegociacao"),
                    WithParams = Url.Action("Renegociacao")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Dados da renegociação",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" },
                        new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Id = "fly01frmCabecalho",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new AutocompleteUI
            {
                Id = "renegociacaoPessoaId",
                Class = "col s12 m10 l10",
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
                Class = "col s12 m2 l2",
                Value = "Listar contas",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "click", Function = "fnListaTitulosCR" }
                }
            });

            config.Elements.Add(new LabelsetUI { Id = "selecaoTitulosLabel", Class = "col s12", Label = "Seleção das contas" });

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

            config2.Elements.Add(new LabelsetUI { Id = "informacoesRenegociacaoLabel", Class = "col s12", Label = "Informações da renegociação" });

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

            config2.Elements.Add(new LabelsetUI { Id = "informacoesNovoTituloLabel", Class = "col s12", Label = "Informações da nova conta a receber" });

            config2.Elements.Add(new InputTextUI { Id = "descricaoRenegociacao", Class = "col s12 m6 l6", Label = "Descrição", Required = true, MaxLength = 200 });
            config2.Elements.Add(new InputDateUI { Id = "dataEmissao", Class = "col s12 m3 l3", Label = "Emissão", Required = true });
            config2.Elements.Add(new InputDateUI { Id = "dtVencimento", Class = "col s12 m3 l3", Label = "Vencimento", Required = true });
            config2.Elements.Add(new AutocompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 l6",
                Label = "Forma Pagamento",
                Required = true,
                DataUrl = @Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "FormaPagamento"),
                DataPostField = "descricao"
            });
            config2.Elements.Add(new AutocompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 l6",
                Label = "Condição Parcelamento",
                Required = true,
                DataUrl = @Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "CondicaoParcelamento"),
                DataPostField = "descricao"
            });
            config2.Elements.Add(new AutocompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 l6",
                Label = "Categoria Financeira",
                Required = true,
                DataUrl = @Url.Action("CategoriaCR", "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = Url.Action("NovaCategoriaReceita")
            });

            cfg.Content.Add(config2);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
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
                return "F";
            if (documento.Length > 11)
                return "J";

            return null;
        }

        #endregion

    }
}