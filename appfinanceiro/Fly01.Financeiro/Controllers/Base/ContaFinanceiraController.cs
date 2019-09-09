using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
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
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers.Base
{
    public abstract class ContaFinanceiraController<TEntity, TEntityBaixa> : BaseController<TEntity>
        where TEntity : ContaFinanceiraVM
        where TEntityBaixa : ContaFinanceiraBaixaVM
    {
        protected ContaFinanceiraController() { }

        [OperationRole(NotApply = true)]
        public override Func<TEntity, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                statusEnum = x.StatusContaBancaria,
                statusContaBancaria = EnumHelper.GetDescription(typeof(StatusContaBancaria), x.StatusContaBancaria),
                statusContaBancariaCssClass = EnumHelper.GetCSS(typeof(StatusContaBancaria), x.StatusContaBancaria),
                statusContaBancariaNomeCompleto = EnumHelper.GetValue(typeof(StatusContaBancaria), x.StatusContaBancaria),
                contaFinanceiraRepeticaoPaiId = x.ContaFinanceiraRepeticaoPaiId,
                tipoPeriodicidade = x.TipoPeriodicidade,
                numero = x.Numero,
                pessoaId = x.PessoaId,
                dataEmissao = x.DataEmissao.ToString("dd/MM/yyyy"),
                dataVencimento = x.DataVencimento.ToString("dd/MM/yyyy"),
                descricao = x.Descricao.Substring(0, x.Descricao.Length > 35 ? 35 : x.Descricao.Length),
                valorPrevisto = x.ValorPrevisto.ToString("C", AppDefaults.CultureInfoDefault),
                formaPagamento_descricao = x.FormaPagamento.Descricao,
                formaPagamento = x.FormaPagamento.TipoFormaPagamento,
                descricaoParcela = string.IsNullOrEmpty(x.DescricaoParcela) ? "" : x.DescricaoParcela,
                categoria_descricao = x.Categoria.Descricao,
                pessoa_nome = x.Pessoa.Nome,
                saldo = x.Saldo.ToString("C", AppDefaults.CultureInfoDefault),
                saldoSemFormatacao = x.Saldo,
                diasVencidos = x.DiasVencidos,
                condicaoParcelamento_descricao = x.CondicaoParcelamento.Descricao,
                observacao = x.Observacao,
                repetir = x.Repetir,
                valorConciliado = x.Saldo.ToString("C", AppDefaults.CultureInfoDefault),
                NumeroRepeticoes = x.NumeroRepeticoes,
                valorPago = x.ValorPago,
                //FormaPagamentoObject = x.FormaPagamento,
                Pessoa = x.Pessoa,
                dataVencimentoObject = x.DataVencimento,
                repeticaoPai = x.ContaFinanceiraRepeticaoPaiId == null && x.Repetir,
                repeticaoFilha = x.ContaFinanceiraRepeticaoPaiId != null && x.Repetir
            };
        }

        [OperationRole(NotApply = true)]
        public Func<TEntityBaixa, object> GetDisplayDataBaixas()
        {
            return x => new
            {
                data = x.Data.ToString("dd/MM/yyyy"),
                valor = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                contaBancaria_NomeConta = x.ContaBancaria.NomeConta,
                observacao = x.Observacao
            };
        }

        [OperationRole(NotApply = true)]
        public Func<TEntity, object> GetDisplayDataBaixaMultipla()
        {
            return x => new
            {
                id = x.Id != null ? x.Id : Guid.Empty,
                statusContaBancaria = EnumHelper.GetCSS(typeof(StatusContaBancaria), x.StatusContaBancaria),
                numero = x.Id.ToString(),
                dataEmissao = x.DataEmissao.ToString("dd/MM/yyyy"),
                dataVencimento = x.DataVencimento.ToString("dd/MM/yyyy"),
                valorPrevisto = x.ValorPrevisto.ToString("C", AppDefaults.CultureInfoDefault),
                saldo = x.ValorPago.HasValue ? (x.ValorPrevisto - x.ValorPago.Value).ToString("C", AppDefaults.CultureInfoDefault) : string.Empty
            };
        }

        [HttpPost]
        public override JsonResult Create(TEntity entityVM)
        {
            try
            {
                entityVM.StatusContaBancaria = "EmAberto";

                if (Request.Form["BaixarTitulo"] != null && bool.Parse(Request.Form["BaixarTitulo"]))
                    entityVM.StatusContaBancaria = "Pago";

                var conta = RestHelper.ExecutePostRequest<TEntity>(ResourceName, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));

                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Create);
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public virtual ActionResult PrintList(List<TEntity> contas, string titulo)
        {
            List<ImprimirListContasVM> reportItens = new List<ImprimirListContasVM>();

            foreach (TEntity ListContas in contas)
                reportItens.Add(new ImprimirListContasVM
                {
                    Id = ListContas.Id,
                    Status = ListContas.StatusContaBancaria == "EmAberto" ? "Em Aberto" : ListContas.StatusContaBancaria,
                    Descricao = ListContas.Descricao,
                    Valor = ListContas.ValorPrevisto,
                    FormaPagamento = ListContas.FormaPagamento != null ? ListContas.FormaPagamento.Descricao : string.Empty,
                    Fornecedor = ListContas.Pessoa != null ? ListContas.Pessoa.Nome : string.Empty,
                    Cliente = ListContas.Pessoa != null ? ListContas.Pessoa.Nome : string.Empty,
                    Vencimento = ListContas.DataVencimento,
                    Titulo = titulo,
                    Numero = ListContas.Numero,
                    TipoConta = ResourceName
                });

            var reportViewer = new WebReportViewer<ImprimirListContasVM>(ReportListContas.Instance);

            return File(reportViewer.Print(reportItens, SessionManager.Current.UserData.PlatformUrl), "application/pdf");
        }

        public virtual ContentResult Visualizar(Guid id)
        {
            return Json(id);
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public JsonResult ListContaBaixas(string contaFinanceiraId)
        {
            try
            {
                var resourceName = AppDefaults.GetResourceName(typeof(TEntityBaixa));

                var queryString = AppDefaults.GetQueryStringDefault();
                queryString.AddParam("$expand", $"contaBancaria($select=nomeConta)");
                queryString.AddParam("$filter", $"contaFinanceiraId eq {contaFinanceiraId} and ativo");
                queryString.AddParam("$orderby", "data desc, dataInclusao desc");

                var response = RestHelper.ExecuteGetRequest<ResultBase<TEntityBaixa>>(resourceName, queryString);

                return Json(new
                {
                    success = true,
                    data = response.Data.Select(GetDisplayDataBaixas()),
                    recordsFiltered = response.Total,
                    recordsTotal = response.Total
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = string.Format("Ocorreu um erro ao carregar dados: {0}", ex.Message) }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Baixa de Títulos
        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public virtual ContentResult BaixaTitulo(Guid id)
        {
            TEntity entity = Get(id);

            TEntityBaixa baixa = Activator.CreateInstance<TEntityBaixa>();
            baixa.Data = DateTime.Now.Date;
            baixa.Valor = entity.Saldo;
            baixa.ContaFinanceiraId = entity.Id;
            baixa.Observacao = entity.Descricao;

            return Content(JsonConvert.SerializeObject(baixa, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult BaixaTitulo(TEntityBaixa entityVM)
        {
            try
            {
                string resourceBaixa = AppDefaults.GetResourceName(typeof(TEntityBaixa));

                string message = "Pagamento";
                if (resourceBaixa.Trim().ToLower().Contains("receber"))
                    message = "Recebimento";

                RestHelper.ExecutePostRequest(resourceBaixa, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                return JsonResponseStatus.GetSuccess(string.Format("{0} realizado com sucesso.", message));
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult CancelarBaixaTitulo(string id)
        {
            string resourceAllBaixas = AppDefaults.GetResourceName(typeof(TEntityBaixa));
            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"contaFinanceiraId eq {id}");
            queryString.AddParam("$select", "id");
            try
            {
                ResultBase<TEntityBaixa> allBaixas = RestHelper.ExecuteGetRequest<ResultBase<TEntityBaixa>>(resourceAllBaixas, queryString);
                foreach (var item in allBaixas.Data)
                {
                    RestHelper.ExecuteDeleteRequest(String.Format("{0}/{1}", resourceAllBaixas, item.Id));
                }
                return JsonResponseStatus.GetSuccess("Pagamento/s cancelado/s com sucesso.");
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        #endregion

        public Dictionary<string, string> ProcessQueryString(string queryString)
        {
            //mesma função do TaskExportItemBase
            //exporta os mesmos registros do Grid 
            Dictionary<string, string> queryStringResult =
                Regex.Matches(queryString, "([^?=&]+)(=([^&]*))?").Cast<Match>().ToDictionary(x => x.Groups[1].Value, x => x.Groups[3].Value);

            // Altera limite de registros para 50 (Máximo da API)
            queryStringResult.AddParam("max", AppDefaults.MaxRecordsPerPageAPI.ToString());

            // Altera página sempre para a primeira
            queryStringResult.AddParam("page", "1");

            return queryStringResult;
        }
        public abstract ActionResult ImprimirRecibo(Guid id);

        public List<TEntity> GetContasParaImprimir(string queryStringGrid)
        {
            Dictionary<string, string> queryStringRequest = ProcessQueryString(queryStringGrid);

            int page = Convert.ToInt32(queryStringRequest.FirstOrDefault(x => x.Key == "page").Value);
            string resource = AppDefaults.GetResourceName(typeof(TEntity));

            List<TEntity> recordsToPrint = new List<TEntity>();
            bool hasNextRecord = true;
            while (hasNextRecord)
            {
                queryStringRequest.AddParam("page", page.ToString());
                ResultBase<TEntity> response = RestHelper.ExecuteGetRequest<ResultBase<TEntity>>(resource, queryStringRequest);
                recordsToPrint.AddRange(response.Data);
                hasNextRecord = response.HasNext && response.Data.Count > 0;
                page++;
            }
            return recordsToPrint;
        }

        public virtual JsonResult GridLoadContasNaoPagas()
        {
            var filters = new Dictionary<string, string>()
            {
                { "statusContaBancaria",
                    "eq " + AppDefaults.APIEnumResourceName + "StatusContaBancaria'EmAberto' or statusContaBancaria eq " + AppDefaults.APIEnumResourceName + "StatusContaBancaria'BaixadoParcialmente'" },
            };

            return base.GridLoad(filters);
        }

        #region Modals
        public ContentResult ModalNovaBaixa(string contaController)
        {
            string tituloModal = "Marcar como " + (contaController == "ContaPagar" ? "pago" : "recebido");
            var config = new ModalUIForm()
            {
                Title = tituloModal,
                Id = "fly01mdlfrm",
                UrlFunctions = @Url.Action("Functions", contaController, null, Request.Url.Scheme) + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Salvar", OnClickFn = "fnAtualizaGrid" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction()
                {
                    Create = @Url.Action("BaixaTitulo", contaController),
                    Get = @Url.Action("BaixaTitulo", contaController) + "/",
                }
            };

            config.Elements.Add(new InputHiddenUI { Id = "contaFinanceiraId" });
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 l6", Label = "Data", Required = true });
            config.Elements.Add(new InputCurrencyUI { Id = "valor", Class = "col s12 l6", Label = "Valor", Required = true });
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

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", MaxLength = 200 });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
        public ContentResult ModalVisualizarConta(string contaController)
        {
            string tituloPessoa = (contaController == "ContaPagar" ? "Fornecedor" : "Cliente");
            var config = new ModalUIForm()
            {
                Title = "Visualizar",
                Id = "fly01mdlfrm",
                UrlFunctions = @Url.Action("Functions", contaController, null, Request.Url.Scheme) + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Fechar" },
                Action = new FormUIAction()
                {
                    Get = @Url.Action("Visualizar", contaController) + "/",
                },
                ReadyFn = "fnFormReadyVisualizar",
            };
            #region Dados da conta
            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "statusContaBancaria" });
            config.Elements.Add(new InputTextUI { Id = "numero", Class = "col s12 m4 l3", Label = "Número", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 m8 l9", Label = "Descrição", Disabled = true });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "pessoaId",
                Class = "col s12 m6 l6",
                Label = tituloPessoa,
                Disabled = true,
                DataUrl = @Url.Action("Pessoa", "AutoComplete"),
                LabelId = "pessoaNome"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m6 l6",
                Label = "Categoria Financeira",
                Disabled = true,
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao"
            });
            config.Elements.Add(new InputCurrencyUI { Id = "valorPrevisto", Class = "col s12 m4 l4", Label = "Valor", Disabled = true });
            config.Elements.Add(new InputDateUI { Id = "dataEmissao", Class = "col s12 m4 l4", Label = "Emissão", Disabled = true });
            config.Elements.Add(new InputDateUI
            {
                Id = "dataVencimento",
                Class = "col s12 m4 l4",
                Label = "Vencimento",
                Disabled = true,
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnChangeVencimento" }
                }
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 m4",
                Label = "Forma Pagamento",
                Disabled = true,
                DataUrl = @Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 m4",
                Label = "Condição Parcelamento",
                Disabled = true,
                DataUrl = @Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao"
            });

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col s12", Label = "Observação", Disabled = true, MaxLength = 200 });
            #endregion

            #region Relação Baixas
            config.Elements.Add(new LabelSetUI { Id = "relacaoBaixaLabel", Class = "col s12", Label = "Relação de baixas" });

            config.Elements.Add(new TableUI
            {
                Id = "datatableContaBaixas",
                Class = "col s12",
                Label = "Relação de baixas",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI() { Label = "Data", Value = "1"},
                    new OptionUI() { Label = "Valor", Value = "0" },
                    new OptionUI() { Label = "Conta Bancária",Value = "2" },
                    new OptionUI() { Label = "Observação", Value = "3"}
                }
            });
            #endregion

            #region Dados Renegociação
            config.Elements.Add(new LabelSetUI { Id = "dadosRenegociacaoLabel", Class = "col s12", Label = "Dados da renegociação" });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoRenegociacaoValorDiferenca",
                Class = "col s12 m6",
                Label = "Tipo do Valor Diferença",
                Disabled = true,
                Options = new List<SelectOptionUI>
                {
                    new SelectOptionUI() {Label= "Acréscimo", Value = "Acrescimo"},
                    new SelectOptionUI() {Label= "Desconto", Value = "Desconto"}
                }
            });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoRenegociacaoCalculo",
                Class = "col s12 m6",
                Label = "Tipo Cálculo Diferença",
                Disabled = true,
                Options = new List<SelectOptionUI>
                {
                    new SelectOptionUI() {Label= "Valor", Value = "Valor"},
                    new SelectOptionUI() {Label= "Percentual", Value = "Percentual"}
                }
            });
            config.Elements.Add(new InputFloatUI { Id = "valorDiferenca", Class = "col s12 m6", Label = "Valor Diferença ", Disabled = true });
            config.Elements.Add(new InputCurrencyUI { Id = "valorFinal", Class = "col s12 m6", Label = "Valor Final ", Disabled = true });

            config.Elements.Add(new LabelSetUI { Id = "lblRenegociacaoOrigem", Class = "col s12", Label = "Contas originais renegociadas" });
            config.Elements.Add(new LabelSetUI { Id = "lblRenegociacaoRenegociadas", Class = "col s12", Label = "Contas geradas pela renegociação" });
            config.Elements.Add(new TableUI
            {
                Id = "datatableRenegociacaoRelacionamento",
                Class = "col s12",
                Label = "Renegociação Relacionamento",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI() { Label = "Descrição", Value = "0"},
                    new OptionUI() { Label = "Valor", Value = "1" },
                    new OptionUI() { Label = "Vencimento",Value = "2" },
                    new OptionUI() { Label = "Parcelas",Value = "2" },
                    new OptionUI() { Label = "Forma Pagamento", Value = "3"},
                    new OptionUI() { Label = tituloPessoa, Value = "4"}
                }
            });
            #endregion

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
        #endregion
    }
}