using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.uiJS.Classes.Helpers;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Financeiro.Entities.ViewModel;
using Fly01.Core.Presentation;

namespace Fly01.Financeiro.Controllers
{
    [AllowAnonymous]
    public class ConciliacaoBancariaController : BaseController<ConciliacaoBancariaVM>
    {
        public ConciliacaoBancariaController()
        {
            ExpandProperties = "contaBancaria($expand=banco)";
        }

        public override Func<ConciliacaoBancariaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                contaBancaria_nomeConta = x.ContaBancaria.NomeConta,
                contaBancaria_banco_codigo = x.ContaBancaria.Banco.Codigo,
                contaBancaria_banco_nome = x.ContaBancaria.Banco.Nome,
                contaBancaria_agencia = x.ContaBancaria.Agencia,
                contaBancaria_conta = x.ContaBancaria.Conta
            };
        }

        private string GetExtratoFromRequest()
        {
            if (Request.Files.Count == 0)
            {
                return null;
            }
            HttpPostedFileWrapper extratoFile = (HttpPostedFileWrapper)Request.Files[0];

            string tempFile = Path.GetTempFileName();
            extratoFile.SaveAs(tempFile);

            var fileBytes = System.IO.File.ReadAllBytes(tempFile);
            string fileBase64 = Convert.ToBase64String(fileBytes);

            return fileBase64;
        }

        [HttpPost]
        public JsonResult CreateConciliacao()
        {
            try
            {
                if (Request.Files.Count == 0)
                {
                    return JsonResponseStatus.GetFailure("Arquivo do extrato não enviado");
                }
                else if (Request.Form["contaBancariaId"] == null || string.IsNullOrWhiteSpace(Request.Form["contaBancariaId"]))
                {
                    return JsonResponseStatus.GetFailure("Conta bancária não foi informada");
                }
                else
                {
                    Guid contaBancariaId = Guid.Parse(Request.Form["contaBancariaId"]);
                    return base.Create(new ConciliacaoBancariaVM() { Arquivo = GetExtratoFromRequest(), ContaBancariaId = contaBancariaId });
                }
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpPost]
        public JsonResult EditConciliacao()
        {
            try
            {
                if (Request.Form["id"] == null || string.IsNullOrWhiteSpace(Request.Form["id"]))
                {
                    return JsonResponseStatus.GetFailure("Id não foi informado na edição da conciliação");
                }
                else if (Request.Files.Count == 0)
                {
                    return JsonResponseStatus.GetFailure("Arquivo do extrato não enviado");
                }
                else if (Request.Form["contaBancariaId"] == null || string.IsNullOrWhiteSpace(Request.Form["contaBancariaId"]))
                {
                    return JsonResponseStatus.GetFailure("Conta bancária não foi informada");
                }
                else
                {
                    Guid id = Guid.Parse(Request.Form["id"]);
                    Guid contaBancariaId = Guid.Parse(Request.Form["contaBancariaId"]);

                    return Edit(new ConciliacaoBancariaVM() { Arquivo = GetExtratoFromRequest(), Id = id, ContaBancariaId = contaBancariaId });
                }
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpPost]
        public override JsonResult Edit(ConciliacaoBancariaVM entityVM)
        {
            try
            {
                var resourceNamePut = $"{ResourceName}/{entityVM.Id}";
                RestHelper.ExecutePutRequest(resourceNamePut, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));

                return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Edit);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpPost]
        public JsonResult ExluirLancamentoExtrato(Guid conciliacaoBancariaItemId)
        {
            try
            {
                RestHelper.ExecuteDeleteRequest(String.Format("{0}/{1}", "ConciliacaoBancariaItem", conciliacaoBancariaItemId));
                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Delete);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult SalvarConciliacaoBancariaItemConta(ConciliacaoBancariaItemContaFinanceiraVM CBItemConta)
        {
            try
            {
                CBItemConta.ValorConciliado = Math.Abs(CBItemConta.ValorConciliado);
                RestHelper.ExecutePostRequest("ConciliacaoBancariaItemContaFinanceira", JsonConvert.SerializeObject(CBItemConta, JsonSerializerSetting.Default));
                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpGet]
        public ContentResult BuscarExistentes(Guid id)
        {
            return Content(JsonConvert.SerializeObject(GetConciliacaoBancariaItem(id), JsonSerializerSetting.Front), "application/json");
        }

        [HttpGet]
        public ActionResult BuscarExistentesCP(Guid id)
        {
            return View("BuscarExistentesCP", id);
        }

        [HttpGet]
        public ActionResult BuscarExistentesCR(Guid id)
        {
            return View("BuscarExistentesCR", id);
        }

        [HttpPost]
        public virtual JsonResult BuscarExistentes(ConciliacaoBancariaItemVM conciliacaoBancariaItem)
        {
            try
            {
                if (conciliacaoBancariaItem.ConciliacaoBancariaItemContasFinanceiras == null || !conciliacaoBancariaItem.ConciliacaoBancariaItemContasFinanceiras.Any())
                {
                    return JsonResponseStatus.GetFailure("Necessário vincular ao menos uma conta financeira para salvar");
                }
                else
                {
                    double somaConciliados = Math.Round(conciliacaoBancariaItem.ConciliacaoBancariaItemContasFinanceiras.Sum(x => x.ValorConciliado), 2);
                    double valor = Math.Round(Math.Abs(conciliacaoBancariaItem.Valor), 2);
                    if (somaConciliados > valor)
                    {
                        return JsonResponseStatus.GetFailure("A soma dos valores conciliados não pode ser superior ao valor do lançamento");
                    }
                    else if (somaConciliados <= 0)
                    {
                        return JsonResponseStatus.GetFailure("A soma dos valores conciliados deve ser superior a zero");
                    }
                    else if (!(somaConciliados == valor))
                    {
                        return JsonResponseStatus.GetFailure("A soma dos valores conciliados deve ser igual ao valor do lançamento no extrato");
                    }
                }
                RestHelper.ExecutePostRequest("ConciliacaoBancariaBuscarExistentes", JsonConvert.SerializeObject(conciliacaoBancariaItem, JsonSerializerSetting.Default));
                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpGet]
        public JsonResult GetConciliacaoBancariaItens(string id)
        {

            var param = JQueryDataTableParams.CreateFromQueryString(Request.QueryString);

            var pageNo = param.Start > 0 ? (param.Start / 20) + 1 : 1;

            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                    { "conciliacaoBancariaId", string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id },
                    { "pageNo", pageNo.ToString() },
                    { "pageSize", "20" }
                };

                var resource = "ConciliacaoBancariaItemSugestao";

                var response = RestHelper.ExecuteGetRequest<PagedResult<ConciliacaoBancariaItemVM>>(resource, queryString);
                //var conciliacaoBancariaItens = JsonConvert.SerializeObject(response.Data, JsonSerializerSetting.Front);

                return JsonResponseStatus.GetJson(new
                {
                    data = response.Data.Select(x => new
                    {
                        id = x.Id,
                        conciliacaoBancariaId = x.ConciliacaoBancariaId,
                        descricao = x.Descricao,
                        valor = x.Valor,
                        valorFormat = x.Valor.ToString("C", AppDefaults.CultureInfoDefault),
                        valorFormat2 = x.Valor.ToString("N", AppDefaults.CultureInfoDefault).Replace(".", ""),
                        data = x.Data.ToString("dd/MM/yyyy"),
                        statusConciliado = x.StatusConciliado,
                        conciliadoDescription = EnumHelper.GetDescription(typeof(StatusConciliado), x.StatusConciliado),
                        conciliadoCssClass = EnumHelper.GetCSS(typeof(StatusConciliado), x.StatusConciliado),
                        conciliadoValue = EnumHelper.GetValue(typeof(StatusConciliado), x.StatusConciliado),
                        conciliacaoBancariaItemContasFinanceiras = x.ConciliacaoBancariaItemContasFinanceiras != null ?
                            x.ConciliacaoBancariaItemContasFinanceiras.Select(y => new
                            {
                                contaFinanceiraId = y.ContaFinanceiraId,
                                valorConciliado = y.ValorConciliado.ToString("N", AppDefaults.CultureInfoDefault),
                                contaFinanceira = new
                                {
                                    id = y.ContaFinanceira.Id.ToString(),
                                    dataVencimento = y.ContaFinanceira.DataVencimento.ToString("dd/MM"),
                                    descricao = y.ContaFinanceira.Descricao
                                }
                            })
                            : null
                    }),
                    recordsTotal = response.Paging.TotalRecordCount,
                    recordsFiltered = response.Paging.TotalRecordCount,
                    success = true
                });
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        private ConciliacaoBancariaItemVM GetConciliacaoBancariaItem(Guid id)
        {
            string resourceByIdCBitem = String.Format("{0}/{1}", "conciliacaoBancariaItem", id);
            return RestHelper.ExecuteGetRequest<ConciliacaoBancariaItemVM>(resourceByIdCBitem, queryString: null);
        }

        public ContentResult NovaTransacao(Guid id)
        {
            var conciliacaoBancariaItem = GetConciliacaoBancariaItem(id);
            ConciliacaoBancariaTransacaoVM novaTransacao = Activator.CreateInstance<ConciliacaoBancariaTransacaoVM>();

            //dados da nova conta financeira
            novaTransacao.DataVencimento = conciliacaoBancariaItem.Data;
            novaTransacao.ValorPrevisto = Math.Abs(conciliacaoBancariaItem.Valor);
            novaTransacao.Descricao = conciliacaoBancariaItem.Descricao;
            //dados da ConciliacaoBancariaItemContaFinanceira
            novaTransacao.ValorConciliado = Math.Abs(conciliacaoBancariaItem.Valor);
            novaTransacao.ConciliacaoBancariaItemId = conciliacaoBancariaItem.Id;

            return Content(JsonConvert.SerializeObject(novaTransacao, JsonSerializerSetting.Front), "application/json");
        }

        [HttpPost]
        private JsonResult NovaTransacao(ConciliacaoBancariaTransacaoVM novaTransacao)
        {
            try
            {
                RestHelper.ExecutePostRequest("ConciliacaoBancariaTransacao", JsonConvert.SerializeObject(novaTransacao, JsonSerializerSetting.Default));
                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpPost]
        public JsonResult NovaTransacaoCP(ConciliacaoBancariaTransacaoVM novaTransacao)
        {
            try
            {
                novaTransacao.TipoContaFinanceira = "ContaPagar";
                return NovaTransacao(novaTransacao);
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpPost]
        public JsonResult NovaTransacaoCR(ConciliacaoBancariaTransacaoVM novaTransacao)
        {
            try
            {
                novaTransacao.TipoContaFinanceira = "ContaReceber";
                return NovaTransacao(novaTransacao);
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Conciliação Bancária",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions", "ConciliacaoBancaria", null, Request.Url.Scheme) + "?fns="
            };

            DataTableUI config = new DataTableUI
            {
                Id = "fly01dt",
                UrlGridLoad = Url.Action("GridLoad"),
                UrlFunctions = Url.Action("Functions", "ConciliacaoBancaria", null, Request.Url.Scheme) + "?fns="
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" }
            }));

            config.Columns.Add(new DataTableUIColumn() { DataField = "contaBancaria_nomeConta", DisplayName = "Conta nome", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn() { DataField = "contaBancaria_banco_codigo", DisplayName = "Banco", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn() { DataField = "contaBancaria_agencia", DisplayName = "Agência", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn() { DataField = "contaBancaria_conta", DisplayName = "Conta", Priority = 3 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();


            target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" });
            target.Add(new HtmlUIButton { Id = "save", Label = "Importar Extrato", OnClickFn = "fnSalvarConciliacaoBancaria", Type = "submit" });

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
                    Title = "Conciliação Bancária",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions", "ConciliacaoBancaria", null, Request.Url.Scheme) + "?fns="
            };

            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = @Url.Action("CreateConciliacao"),
                    Edit = @Url.Action("EditConciliacao"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                ReadyFn = "fnFormReady",
                EncType = "multipart/form-data",
                UrlFunctions = Url.Action("Functions", "ConciliacaoBancaria", null, Request.Url.Scheme) + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "contaBancariaId",
                Class = "col s12 m6",
                Label = "Conta bancária",
                Required = true,
                DataUrl = Url.Action("ContaBancariaConciliacao", "AutoComplete"),
                LabelId = "contaBancariaNomeConta",
                DataUrlPostModal = @Url.Action("FormModal", "ContaBancaria"),
                DataPostField = "nomeConta"
            });

            config.Elements.Add(new InputFileUI { Id = "arquivo", Class = "col s12 m6", Label = "Arquivo do extrato bancário (.ofx)", Accept = ".ofx" });

            cfg.Content.Add(config);

            config.Elements.Add(new LabelSetUI { Id = "extratoLancamentosLabel", Class = "col s12", Label = "Movimentações importadas" });

            DataTableUI dtcfg = new DataTableUI
            {
                Id = "dtConciliacaoItens",
                UrlGridLoad = Url.Action("GetConciliacaoBancariaItens"),
                UrlFunctions = Url.Action("Functions", "ConciliacaoBancaria", null, Request.Url.Scheme) + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                },
                Options = new DataTableUIConfig
                {
                    PageLength = 20
                }
            };

            dtcfg.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnNovaTransacaoCP", Label = "Nova conta", ShowIf = "(row.valor < 0.0)" },
                new DataTableUIAction { OnClickFn = "fnNovaTransacaoCR", Label = "Nova conta", ShowIf = "(row.valor > 0.0)" },
                new DataTableUIAction { OnClickFn = "fnBuscarExistentesCP", Label = "Buscar existentes", ShowIf = "(row.valor < 0.0)" },
                new DataTableUIAction { OnClickFn = "fnBuscarExistentesCR", Label = "Buscar existentes", ShowIf = "(row.valor > 0.0)" },
                new DataTableUIAction { OnClickFn = "fnExcluirCBItem", Label = "Excluir", ShowIf = "(row.conciliadoDescription == 'NAO')" }
            }));

            dtcfg.Columns.Add(new DataTableUIColumn() { DataField = "descricao", DisplayName = "Descrição", Priority = 2, Searchable = false, Orderable = false });
            dtcfg.Columns.Add(new DataTableUIColumn() { DataField = "data", DisplayName = "Data", Priority = 3, Type = "date", Searchable = false, Orderable = false });
            dtcfg.Columns.Add(new DataTableUIColumn() { DataField = "valorFormat", DisplayName = "Valor", Priority = 4, Type = "currency", Searchable = false, Orderable = false });

            dtcfg.Columns.Add(new DataTableUIColumn() { Priority = 5, Searchable = false, Orderable = false, RenderFn = "fnRenderSugestao", Width = "30%" });

            cfg.Content.Add(dtcfg);

            return cfg;
        }

        public List<HtmlUIButton> GetFormButtonsBuscaExistenteOnHeader()
        {
            var target = new List<HtmlUIButton>();
            target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelarBuscarExistentes" });
            target.Add(new HtmlUIButton { Id = "save", Label = "Conciliar", OnClickFn = "fnSalvar", Type = "submit" });

            return target;
        }

        public ContentResult FormBuscarExistentes(string tipoConta)
        {
            string titulo = (tipoConta == "ContaPagar" ? "pagar" : "receber");
            string abrev = (tipoConta == "ContaPagar" ? "CP" : "CR");

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("BuscarExistentes" + abrev),
                    WithParams = Url.Action("BuscarExistentes" + abrev)
                },
                Header = new HtmlUIHeader
                {
                    Title = "Buscar contas a " + titulo,
                    Buttons = new List<HtmlUIButton>(GetFormButtonsBuscaExistenteOnHeader())
                },
                UrlFunctions = Url.Action("Functions", "ConciliacaoBancaria", null, Request.Url.Scheme) + "?fns=",
                Functions = { "fnEditar", "fnRowCallbackContasExistentes" }
            };

            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = @Url.Action("BuscarExistentes"),
                    Edit = @Url.Action("BuscarExistentes"),
                    Get = @Url.Action("BuscarExistentes", "ConciliacaoBancaria") + "/",
                },
                ReadyFn = "fnFormReadyBuscarExistentes",
                UrlFunctions = Url.Action("Functions", "ConciliacaoBancaria", null, Request.Url.Scheme) + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "conciliacaoBancariaId" });
            config.Elements.Add(new InputHiddenUI { Id = "ofxLancamentoMD5" });
            config.Elements.Add(new InputHiddenUI { Id = "statusConciliado" });
            config.Elements.Add(new InputHiddenUI { Id = "conciliacaoBancariaItemContasFinanceiras" });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 m6", Label = "Descrição", Readonly = true });
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m6", Label = "Data", Disabled = true });
            config.Elements.Add(new InputCurrencyUI { Id = "valor", Class = "col s12 m6", Label = "Valor", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "somaValoresSelecionados", Class = "col s12 m6", Label = "Soma valores conciliados", Value = "0", Readonly = true });

            config.Helpers.Add(new TooltipUI
            {
                Id = "somaValoresSelecionados",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se não houver contas existentes suficientes, cancele e escolha a opção Nova Conta, para conseguir conciliar."
                }
            });

            cfg.Content.Add(config);

            config.Elements.Add(new LabelSetUI { Id = "contasFinanceirasLabel", Class = "col s12", Label = "Contas financeiras" });

            DataTableUI dtcfg = new DataTableUI
            {
                Id = "dtContasExistentes",
                UrlGridLoad = Url.Action("GridLoadContasNaoPagas", tipoConta),
                UrlFunctions = Url.Action("Functions", "ConciliacaoBancaria", null, Request.Url.Scheme) + "?fns=",
                Options = new DataTableUIConfig()
                {
                    Select = new { style = "multi" },
                    WithoutRowMenu = true
                },
                Callbacks = new DataTableUICallbacks()
                {
                    RowCallback = "fnRowCallbackContasExistentes"
                }
            };
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 3 });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "pessoa_nome", DisplayName = "Pessoa", Priority = 4 });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Vencimento", Priority = 7, Type = "date" });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "valorPrevisto", DisplayName = "Valor", Priority = 6, Type = "currency" });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "saldo", DisplayName = "Saldo", Priority = 2, Orderable = false, Searchable = false });
            //dtcfg.Columns.Add(new DataTableUIColumn { DataField = "valorConciliado", DisplayName = "Valor conciliado", Priority = 1, Type = "currency", Orderable = false, Searchable = false, Width = "10%" });
            dtcfg.Columns.Add(new DataTableUIColumn { DataField = "valorConciliado", DisplayName = "Valor conciliado", Priority = 1, Type = "currency", Orderable = false, Searchable = false, RenderFn = "fnRenderValorConciliado" });

            cfg.Content.Add(dtcfg);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public ContentResult ModalNovaTransacao(string tipoConta)
        {
            string tituloModal = "Nova conta a " + (tipoConta == "ContaPagar" ? "pagar" : "receber");
            string actionCreate = (tipoConta == "ContaPagar" ? "CP" : "CR");

            var config = new ModalUIForm()
            {
                Title = tituloModal,
                Id = "fly01mdlfrm",
                UrlFunctions = @Url.Action("Functions", "ConciliacaoBancaria", null, Request.Url.Scheme) + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction()
                {
                    Create = @Url.Action("NovaTransacao" + actionCreate, "ConciliacaoBancaria"),
                    Get = @Url.Action("NovaTransacao", "ConciliacaoBancaria") + "/",
                },
                ReadyFn = "fnFormReadyNovaTransacao"
            };

            config.Elements.Add(new InputHiddenUI { Id = "conciliacaoBancariaItemId" });
            config.Elements.Add(new InputCurrencyUI { Id = "valorConciliado", Readonly = true });

            config.Elements.Add(new InputTextUI { Id = "dataVencimento", Class = "col s12 l6", Label = "Data Referência", Required = true, Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "valorPrevisto", Class = "col s12 l6", Label = "Valor", Required = true, Readonly = true });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 l6", Label = "Descrição", Required = true });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "pessoaId",
                Class = "col s12 l6",
                Label = tipoConta == "ContaPagar" ? "Fornecedor" : "Cliente",
                Required = true,
                DataUrl = tipoConta == "ContaPagar" ? @Url.Action("Fornecedor", "AutoComplete") : @Url.Action("Cliente", "AutoComplete"),
                LabelId = "pessoaNome",
                DataUrlPost = tipoConta == "ContaPagar" ? @Url.Action("PostFornecedor", "Fornecedor") : @Url.Action("PostCliente", "Cliente")
            });

            config.Elements.Add(new AutoCompleteUI
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

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 l6",
                Label = "Condição Parcelamento à vista",
                Required = true,
                DataUrl = @Url.Action("CondicaoParcelamentoAVista", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao",
                DataUrlPost = Url.Action("PostCondicaoParcelamento", "CondicaoParcelamento"),
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m6",
                Label = "Categoria Financeira",
                Required = true,
                DataUrl = @Url.Action("Categoria" + actionCreate, "AutoComplete"),
                LabelId = "categoriaDescricao",
                DataUrlPost = tipoConta == "ContaPagar" ? Url.Action("NovaCategoriaDespesa") : Url.Action("NovaCategoriaReceita")
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [HttpPost]
        public JsonResult NovaCategoriaReceita(string term)
        {
            return NovaCategoria(new CategoriaVM { Descricao = term, TipoCarteira = "1" });
        }

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
    }
}