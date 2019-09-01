using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Financeiro.Models.Reports;
using Fly01.Financeiro.Models.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    public abstract class RelatorioContaFinanceiraController : BaseController<DomainBaseVM>
    {
        protected string tipoConta;

        public RelatorioContaFinanceiraController(string TipoConta)
        {
            tipoConta = TipoConta;
        }

        public override Func<DomainBaseVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            return Content(JsonConvert.SerializeObject(FormRelatorioJson(Url, Request.Url.Scheme), JsonSerializerSetting.Front), "application/json");
        }

        protected virtual ContentUI FormRelatorioJson(UrlHelper url, string scheme)
        {
            if (!UserCanRead)
            {
                return new ContentUIBase(Url.Action("Sidebar", "Home"));
            }

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = $"Relatório -  Contas a {tipoConta.Replace("Conta", "")}",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = url.Action("Functions") + "?fns=",
            };

            var config = new FormUI
            {
                Id = "fly01frmRelatorio",
                Action = new FormUIAction
                {
                    //Create = @Url.Action("Create"),
                    //List = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "tipoId", Value = tipoConta });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 m4", Label = "Descrição", MaxLength = 200 });
            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "pessoaId",
                Class = "col s12 m4",
                Label = tipoConta == "ContaPagar" ? "Fornecedor" : "Cliente",
                DataUrl = tipoConta == "ContaPagar" ? @Url.Action("Fornecedor", "AutoComplete") : Url.Action("Cliente", "AutoComplete"),
                LabelId = "pessoaNome",
            }, tipoConta == "ContaPagar" ? ResourceHashConst.FinanceiroCadastrosFornecedores : ResourceHashConst.FinanceiroCadastrosClientes));

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 m4",
                Label = "Forma Pagamento",
                DataUrl = Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao"
            });
            config.Elements.Add(new InputDateUI
            {
                Id = "dataEmissaoInicial",
                Class = "col s6 m3",
                Label = "Emissão Inicial",
                Value = DateTime.Now.ToString("dd/MM/yyyy")
            });
            config.Elements.Add(new InputDateUI
            {
                Id = "dataEmissaoFinal",
                Class = "col s6 m3",
                Label = "Emissão Final",
                Value = DateTime.Now.ToString("dd/MM/yyyy")
            });
            config.Elements.Add(new InputDateUI
            {
                Id = "dataInicial",
                Class = "col s12 m3",
                Label = "Vencimento Inicial",
                Value = DateTime.Now.ToString("dd/MM/yyyy"),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });
            config.Elements.Add(new InputDateUI
            {
                Id = "dataFinal",
                Class = "col s12 m3",
                Label = "Vencimento Final",
                Value = DateTime.Now.ToString("dd/MM/yyyy"),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "" }
                }
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 m4",
                Label = "Condição Parcelamento",
                DataUrl = Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao"
            });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m4",
                Label = "Categoria Financeira",
                DataUrl = tipoConta == "ContaPagar" ? @Url.Action("CategoriaCP", "AutoComplete") : @Url.Action("CategoriaCR", "AutoComplete"),
                LabelId = "categoriaDescricao",
            }, ResourceHashConst.FinanceiroCadastrosCategoria));

            config.Helpers.Add(new TooltipUI
            {
                Id = "descricao",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Exibido o limite de até 2000 registros do resultado da busca. Se necessário aprimorar o resultado, informe todos os parâmetros possíveis."
                }
            });

            cfg.Content.Add(config);
            return cfg;
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
            target.Add(new HtmlUIButton { Id = "imprimirRelatorioId", Label = "Imprimir", OnClickFn = "fnImprimirRelatorioCPCR" });
            }

            return target;
        }

        [HttpGet]
        public ActionResult Imprimir(DateTime? dataInicial,
                                     DateTime? dataFinal,
                                     DateTime? dataEmissaoInicial,
                                     DateTime? dataEmissaoFinal,
                                     Guid? pessoaId,
                                     Guid? formaPagamentoId,
                                     Guid? condicaoParcelamentoId,
                                     Guid? categoriaId,
                                     string descricao)
        {
            try
            {
                var queryString = new Dictionary<string, string>
            {
                { "dataInicial", dataInicial != null ? dataInicial.Value.ToString("yyyy-MM-dd") : ""},
                { "dataFinal", dataFinal != null ? dataFinal.Value.ToString("yyyy-MM-dd") : ""},
                { "dataEmissaoInicial", dataEmissaoInicial != null ? dataEmissaoInicial.Value.ToString("yyyy-MM-dd") : ""},
                { "dataEmissaoFinal", dataEmissaoFinal != null ? dataEmissaoFinal.Value.ToString("yyyy-MM-dd") : ""},
                { "pessoaId", pessoaId.ToString()},
                { "formaPagamentoId", formaPagamentoId.ToString()},
                { "condicaoParcelamentoId", condicaoParcelamentoId.ToString()},
                { "categoriaId", categoriaId.ToString()},
                { "tipoConta", tipoConta.ToString()},
                { "descricao", descricao},
            };
                List<ImprimirListContasVM> reportItens = GetContaFinanceira(queryString, tipoConta);
                
                var reportViewer = new WebReportViewer<ImprimirListContasVM>(RelatorioContasPagarReceber.Instance);
                return File(reportViewer.Print(reportItens, SessionManager.Current.UserData.PlatformUrl), "application/pdf");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private static List<ImprimirListContasVM> GetContaFinanceira(Dictionary<string, string> queryString, string tipo)
        {
            try
            {
                var result = new List<ImprimirListContasVM>();
                var response = RestHelper.ExecuteGetRequest<ResultBase<ImprimirListContasVM>>("relatorioContaFinanceira", queryString);
                result.AddRange(response?.Data);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public JsonResult ValidaImpressao(DateTime? dataInicial,
                                     DateTime? dataFinal,
                                     DateTime? dataEmissaoInicial,
                                     DateTime? dataEmissaoFinal,
                                     Guid? pessoaId,
                                     Guid? formaPagamentoId,
                                     Guid? condicaoParcelamentoId,
                                     Guid? categoriaId,
                                     string descricao)
        {
            try
            {
                var queryString = new Dictionary<string, string>
            {
                { "dataInicial", dataInicial != null ? dataInicial.Value.ToString("yyyy-MM-dd") : ""},
                { "dataFinal", dataFinal != null ? dataFinal.Value.ToString("yyyy-MM-dd") : ""},
                { "dataEmissaoInicial", dataEmissaoInicial != null ? dataEmissaoInicial.Value.ToString("yyyy-MM-dd") : ""},
                { "dataEmissaoFinal", dataEmissaoFinal != null ? dataEmissaoFinal.Value.ToString("yyyy-MM-dd") : ""},
                { "pessoaId", pessoaId.ToString()},
                { "formaPagamentoId", formaPagamentoId.ToString()},
                { "condicaoParcelamentoId", condicaoParcelamentoId.ToString()},
                { "categoriaId", categoriaId.ToString()},                
                { "tipoConta", tipoConta.ToString()},
                { "descricao", descricao},
            };
                List<ImprimirListContasVM> records = GetContaFinanceira(queryString, tipoConta);

                return Json(new
                {
                    success = true,
                    records = records.Count
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

    }
}
