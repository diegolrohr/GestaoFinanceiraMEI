﻿using Fly01.Financeiro.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Fly01.Core.Rest;
using Fly01.Core.Presentation.Commons;
using Fly01.Financeiro.Models.ViewModel;
using Fly01.Financeiro.Models.Reports;
using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;

namespace Fly01.Financeiro.Controllers.Base
{
    public abstract class ContaFinanceiraController<TEntity, TEntityBaixa, TEntityRenegociacao> : BaseController<TEntity>
        where TEntity : ContaFinanceiraVM
        where TEntityBaixa : ContaFinanceiraBaixaVM
        where TEntityRenegociacao : ContaFinanceiraRenegociacaoVM
    {
        /// <summary>
        /// Construtor
        /// </summary>
        protected ContaFinanceiraController()
        {
            ResourceName = AppDefaults.GetResourceName(typeof(TEntity));
        }

        /// <summary>
        /// Método Responsável por definir as colunas que serão apresentadas 
        /// no grid de listagem
        /// </summary>
        /// <returns></returns>
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
                descricao = x.Descricao,
                valorPrevisto = x.ValorPrevisto.ToString("C", AppDefaults.CultureInfoDefault),
                formaPagamento_descricao = x.FormaPagamento.Descricao,
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
                FormaPagamentoObject = x.FormaPagamento,
                Pessoa = x.Pessoa,
                dataVencimentoObject = x.DataVencimento
            };
        }
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
        public Func<TEntity, object> GetDisplayDataRenegociacao()
        {
            return x => new
            {
                id = x.Id,
                descricao = x.Descricao,
                valorPrevisto = x.ValorPrevisto.ToString("C", AppDefaults.CultureInfoDefault),
                saldo = x.Saldo.ToString("C", AppDefaults.CultureInfoDefault),
                dataVencimento = x.DataVencimento.ToString("dd/MM/yyyy"),
                diasVencidos = x.DiasVencidos,
                descricaoParcela = string.IsNullOrEmpty(x.DescricaoParcela) ? "" : x.DescricaoParcela,
                formaPagamento_descricao = x.FormaPagamento.Descricao,
                categoria_descricao = x.Categoria.Descricao,
                pessoa_nome = x.Pessoa.Nome,
                saldoSemFormatacao = x.Saldo
            };
        }
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

        /// <summary>
        /// Método responsável por salvar um novo registro
        /// Tratado os campos responsáveis pela recorrência
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public override JsonResult Create(TEntity entityVM)
        {
            try
            {
                entityVM.StatusContaBancaria = "EmAberto";
                RestHelper.ExecutePostRequest(ResourceName, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));

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

            foreach (TEntity  ListContas in contas)
                reportItens.Add(new ImprimirListContasVM
                {
                    Id = ListContas.Id,
                    Status = ListContas.StatusContaBancaria == "EmAberto" ? "Em Aberto" : ListContas.StatusContaBancaria,
                    Descricao = ListContas.Descricao,
                    Valor = ListContas.ValorPrevisto.ToString(),
                    FormaPagamento = ListContas.FormaPagamento != null ? ListContas.FormaPagamento.Descricao : string.Empty,
                    Fornecedor = ListContas.Pessoa != null ? ListContas.Pessoa.Nome : string.Empty,
                    Vencimento = ListContas.DataVencimento,
                    Titulo = titulo, 
                    Numero = ListContas.Numero
                });

            var reportViewer = new WebReportViewer<ImprimirListContasVM>(ReportListContas.Instance);

            return File(reportViewer.Print(reportItens, SessionManager.Current.UserData.PlatformUrl), "application/pdf");
        }

        /// <summary>
        /// Método responsável por visualizar as informações salvas, sem a possibilidade de alterar
        /// </summary>
        /// <returns></returns>
        public virtual ContentResult Visualizar(Guid id)
        {
            return Json(id);
        }

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

        [HttpPost]
        public abstract JsonResult ListRenegociacaoRelacionamento(string contaFinanceiraId);

        #region Baixa de Títulos
        /// <summary>
        /// Retorna Modal View para efetuar a operação de Baixa de um título
        /// </summary>
        /// <param name="id">Identificador do titulo</param>
        /// <returns></returns>
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

        /// <summary>
        /// Operação de POST da baixa de um título
        /// </summary>
        /// <param name="entityVM">Entidade em questão</param>
        /// <returns></returns>
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

        /// <summary>
        /// Operação de POST do Cancelamento de Baixa de Titulo
        /// </summary>
        /// <param name="id">Identificador do titulo</param>
        /// <returns></returns>
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

        //public JsonResult ListRelacionamentoBaixas(string contaId, string subtitleCode)
        //{
        //    dynamic dataToView;
        //    int dataTotal;
        //    var queryString = new Dictionary<string, string>();

        //    if (subtitleCode == "2" || subtitleCode == "3")
        //    {
        //        var account = AppDefaults.GetResourceName(typeof(TEntity))
        //                        .Replace("Accounts", "Account");
        //        queryString.AddParam(account + "Id", contaId);

        //        var response = RestHelper.ExecuteGetRequest<ResultBase<BankTransacVM>>(
        //                        AppDefaults.GetResourceName(typeof(BankTransacVM)), queryString);
        //        dataToView = response.Data.Select(GetDisplayDataBankTransac());
        //        dataTotal = response.Total;
        //    }
        //    else
        //    {
        //        throw new ArgumentOutOfRangeException("Somente SubtitleCode 2 e 3");
        //    }

        //    return Json(new
        //    {
        //        recordsTotal = dataTotal,
        //        recordsFiltered = dataTotal,
        //        data = dataToView
        //    }, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Renegociação
        public virtual ActionResult Renegociacao()
        {
            return View("Renegociacao");
        }


        public ContentResult IncluirRenegociacao()
        {
            //string contaController
            ContaFinanceiraRenegociacaoVM entity = new ContaFinanceiraRenegociacaoVM
            {
                DataEmissao = DateTime.Now,
                DataVencimento = DateTime.Now,
                TipoContaFinanceira = ResourceName.Equals("ContaPagar") ? TipoContaFinanceiraVM.ContaPagar.ToString() : TipoContaFinanceiraVM.ContaReceber.ToString(),
                ValorAcumulado = 0,
                ValorDiferenca = 0,
                ValorFinal = 0
            };

            return Content(JsonConvert.SerializeObject(entity, JsonSerializerSetting.Front), "application/json");
        }

        [HttpPost]
        public virtual JsonResult IncluirRenegociacao(ContaFinanceiraRenegociacaoVM entityVM)
        {
            try
            {
                string resourceRenegociacao = AppDefaults.GetResourceName(typeof(TEntityRenegociacao));
                //para não serializar null e Odata não dar erro de "Does not support untyped value in non-open type”
                //pois sao propriedades que só tem na VM
                entityVM.Descricao = entityVM.DescricaoRenegociacao;
                entityVM.DataVencimento = entityVM.DtVencimento.Value;

                entityVM.DescricaoRenegociacao = null;
                entityVM.DtVencimento = null;

                RestHelper.ExecutePostRequest(resourceRenegociacao, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Create);
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public abstract JsonResult GridLoadTitulosARenegociar(string renegociaoPessoaId);
        #endregion

        #region Adiantamento
        public virtual ActionResult IncluirAdiantamento(string origemChamada)
        {
            ViewBag.OrigemChamada = origemChamada;

            return PartialView("_IncluirAdiantamento", Activator.CreateInstance<TEntity>());
        }

        #endregion
        
        #region Bordero/CNAB
        /// <summary>
        /// Retorna Títulos que podem ser inclusos em um bordero
        /// Este método é chamado apenas na inclusão de um bordero
        /// </summary>
        /// <param name="dataInicial">Filtro de Data Inicial</param>
        /// <param name="dataFinal">Filtro de Data Final</param>
        /// <param name="bankId">Filtro de Conta Bancária</param>
        /// <returns></returns>
        public JsonResult GridLoadTitulosPassiveisBordero(string dataInicial, string dataFinal, string bankId)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            filters.AddParam("toBordero", bankId);
            filters.AddParam("dueDate_ge", Convert.ToDateTime(dataInicial).ToString("yyyyMMdd"));
            filters.AddParam("dueDate_le", Convert.ToDateTime(dataFinal).ToString("yyyyMMdd"));
            filters.AddParam("type_le", "9");

            return GridLoad(filters);
        }

        /// <summary>
        /// Retorna todos os Títulos de um determinado bordero
        /// Este método é chamado apenas na edição/visualização de um bordero
        /// </summary>
        /// <param name="idBordero">Identificador de Bordero</param>
        /// <returns></returns>
        public JsonResult GridLoadTitulosBordero(string idBordero)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            filters.AddParam("numberBordero", idBordero);
            return GridLoad(filters);
        }

        public JsonResult GridLoadTitulosConciliacaoBancaria(double? balance = 0)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            filters.AddParam("hasBalance", "");
            filters.AddParam("type_le", "9");

            if (balance.HasValue && Math.Abs(balance.Value) > 0)
            {
                balance = Math.Abs(balance.Value);
                filters.AddParam("balance_le", balance.Value.ToString("0.0", CultureInfo.GetCultureInfo("en-US")));
            }

            return GridLoad(filters);
        }

        public abstract string GetResourceDeleteTituloBordero(string id);

        /// <summary>
        /// Método responsável por retirar um titulo de um determinado bordero
        /// </summary>
        /// <param name="id">Identificador do Título</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteTituloBordero(string id)
        {
            try
            {
                string resource = GetResourceDeleteTituloBordero(id);

                RestHelper.ExecutePutRequest(resource, string.Empty);
                return JsonResponseStatus.GetSuccess("Conta excluida com sucesso.");
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
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "contaBancariaId",
                Class = "col s12",
                Label = "Conta Bancária",
                Required = true,
                DataUrl = @Url.Action("ContaBancaria", "AutoComplete"),
                LabelId = "contaBancariaNomeConta",
                DataUrlPostModal = @Url.Action("FormModal", "ContaBancaria"),
                DataPostField = "nomeConta",
            });

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
                Class = "col s12 m6 l6",
                Label = "Forma Pagamento",
                Disabled = true,
                DataUrl = @Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 m6 l6",
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