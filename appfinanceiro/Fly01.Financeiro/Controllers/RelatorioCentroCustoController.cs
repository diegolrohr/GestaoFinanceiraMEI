using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Financeiro.Models.Reports;
using Fly01.Financeiro.Models.ViewModel;
using Fly01.Financeiro.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    public abstract class RelatorioCentroCustoController : BaseController<DomainBaseVM>
    {
        protected string tipoConta;

        public RelatorioCentroCustoController(string TipoConta)
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
                //Functions = new List<string> { "fnImprimirCPCR" },
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
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 m4", Label = "descricao", MaxLength = 200 });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "clienteId",
                Class = "col s12 m4",
                Label = "Cliente",
                DataUrl = Url.Action("Cliente", "AutoComplete"),
                LabelId = "clienteNome"
            });
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
                Label = "Data Inicial",
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
                Label = "Data Final",
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
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m4",
                Label = "Categoria Financeira",
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao"
            });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "centroCustoId",
                Class = "col s12 m4",
                Label = "Centro de Custo",
                DataUrl = @Url.Action("CentroCusto", "AutoComplete"),
                LabelId = "centroCustoDescricao",
            }, ResourceHashConst.FinanceiroCadastrosCentroCustos));

            cfg.Content.Add(config);
            return cfg;
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            //if (UserCanWrite)
            //{
            target.Add(new HtmlUIButton { Id = "imprimirRelatorioId", Label = "Imprimir", OnClickFn = "fnImprimirCPCR"});
            //}

            return target;
        }

        [HttpGet]
        public ActionResult Imprimir(DateTime? dataInicial,
                                     DateTime? dataFinal,
                                     DateTime? dataEmissaoInicial,
                                     DateTime? dataEmissaoFinal,
                                     Guid? clienteId,
                                     Guid? formaPagamentoId,
                                     Guid? condicaoParcelamentoId,
                                     Guid? categoriaId,
                                     Guid? centroCustoId)
        {
            var queryString = new Dictionary<string, string>
            {
                { "dataInicial", dataInicial != null ? dataInicial.Value.ToString("yyyy-MM-dd") : ""},
                { "dataFinal", dataFinal != null ? dataFinal.Value.ToString("yyyy-MM-dd") : ""},
                { "dataEmissaoInicial", dataEmissaoInicial != null ? dataEmissaoInicial.Value.ToString("yyyy-MM-dd") : ""},
                { "dataEmissaoFinal", dataEmissaoFinal != null ? dataEmissaoFinal.Value.ToString("yyyy-MM-dd") : ""},
                { "clienteId", clienteId.ToString()},
                { "formaPagamentoId", formaPagamentoId.ToString()},
                { "condicaoParcelamentoId", condicaoParcelamentoId.ToString()},
                { "categoriaId", categoriaId.ToString()},
                { "centroCustoId", centroCustoId.ToString()},
                { "tipoConta", tipoConta.ToString()},
            };

            List<ImprimirListContasVM> reportItens = new List<ImprimirListContasVM>();
            List<ContaFinanceiraVM> resultRelatorio = GetContaFinanceira(queryString, tipoConta);

            var descriptionStatus = "";
            foreach (var item in resultRelatorio)
            {
                descriptionStatus = EnumHelper.GetEnumDescription((StatusContaBancaria)Convert.ToInt64(item.StatusContaBancaria));
                reportItens.Add(new ImprimirListContasVM
                {
                    Id = item.Id,
                    Status = descriptionStatus == "EmAberto" ? "Em Aberto" : descriptionStatus,
                    Descricao = item.Descricao,
                    Valor = item.ValorPrevisto.ToString(),
                    FormaPagamento = item.FormaPagamento != null ? item.FormaPagamento.Descricao : string.Empty,
                    Fornecedor = item.Pessoa != null ? item.Pessoa.Nome : string.Empty,
                    Vencimento = item.DataVencimento,
                    Titulo = tipoConta == "ContaPagar" ? "Contas a Pagar" : "Contas a Receber",
                    Numero = item.Numero
                });
            }

            var reportViewer = new WebReportViewer<ImprimirListContasVM>(ReportListContasCentroCusto.Instance);
            return File(reportViewer.Print(reportItens, SessionManager.Current.UserData.PlatformUrl), "application/pdf");
        }

        private static List<ContaFinanceiraVM> GetContaFinanceira(Dictionary<string, string> queryString, string tipo)
        {
            try
            {
                var result = new List<ContaFinanceiraVM>();
                if (tipo == "ContaPagar")
                {
                    var response = RestHelper.ExecuteGetRequest<ResultBase<ContaFinanceiraVM>>("relatorioContaFinanceira", queryString);
                    result.AddRange(response.Data.Cast<ContaFinanceiraVM>().ToList());
                }
                else {
                    var response = RestHelper.ExecuteGetRequest<ResultBase<ContaFinanceiraVM>>("contareceber", queryString);
                    result.AddRange(response.Data.Cast<ContaFinanceiraVM>().ToList());
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
