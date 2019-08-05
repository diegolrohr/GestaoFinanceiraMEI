using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Financeiro.Controllers.Base;
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
using System.Web.Mvc;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroCobrancaBoletos)]
    public class CnabController : BoletoController<CnabVM>
    {
        public CnabController()
        {
            ExpandProperties = "contaReceber($select=id),contaReceber($expand=pessoa($select=nome,email)),contaBancariaCedente($expand=banco($select=nome))";
        }

        public override Func<CnabVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                contaReceberId = x.ContaReceberId,
                contaBancariaId = x.ContaBancariaCedenteId,
                contaBancariaCedente_banco_nome = x.ContaBancariaCedente?.Banco?.Nome,
                contaReceber_pessoa_nome = x.ContaReceber.Pessoa.Nome,
                dataVencimento = x.DataVencimento.ToString("dd/MM/yyyy"),
                valorBoleto = x.ValorBoleto.ToString("C", AppDefaults.CultureInfoDefault),
                valorDesconto = x.ValorDesconto,
                status = x.Status,
                statusCssClass = EnumHelper.GetCSS(typeof(StatusCnab), x.Status),
                statusDescription = EnumHelper.GetDescription(typeof(StatusCnab), x.Status),
                statusTooltip = EnumHelper.GetTooltipHint(typeof(StatusCnab), x.Status),
                dataEmissao = x.DataEmissao.ToString("dd/MM/yyyy"),
                nossoNumeroFormatado = x.NossoNumeroFormatado,
                contaReceber_pessoa_email = x.ContaReceber?.Pessoa?.Email,
                selected = false
            };
        }

        protected override ContentUI FormJson()
        {
            #region Headers

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create", "Cnab"),
                    WithParams = Url.Action("Edit", "Cnab"),
                },
                Header = new HtmlUIHeader
                {
                    Title = "Dados para emissão de boleto",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "cancel", Label = "Voltar", OnClickFn = "fnCancelar" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var configCnab = new FormUI
            {
                Id = "fly01frmBoleto",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnImprimirBoleto" },
                ReadyFn = "fnFormReady"
            };

            configCnab.Elements.Add(new InputHiddenUI { Id = "id" });
            configCnab.Elements.Add(new AutoCompleteUI
            {
                Id = "bancoId",
                Class = "col s12 m12 l12",
                Label = "Conta bancária cedente",
                Required = true,
                DataUrl = @Url.Action("ContaBancariaBancoEmiteBoleto", "AutoComplete") + "?emiteBoleto=true",
                LabelId = "bancoNome"
            });
            configCnab.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "pessoaId",
                Class = "col s12 m6 l6",
                Label = "Cliente",
                Required = true,
                DataUrl = @Url.Action("Cliente", "AutoComplete"),
                LabelId = "pessoaNome",
                DataUrlPost = Url.Action("PostCliente", "Cliente")
            }, ResourceHashConst.FinanceiroCadastrosClientes));

            configCnab.Elements.Add(new PeriodPickerUI()
            {
                Label = "Selecione o período",
                Id = "dataPicker",
                Name = "dataPicker",
                Class = "col s12 m6 l6",
                Selectable = true
            });

            configCnab.Elements.Add(new ButtonUI
            {
                Id = "btnListarContas",
                Class = "col s4 m4",
                Value = "Listar contas",
                DomEvents = new List<DomEventUI>() {
                    new DomEventUI() { DomEvent = "click", Function = "fnShowListCnab" }
    }
            });
            cfg.Content.Add(configCnab);

            #endregion

            #region CnabItem
            var dtConfig = new DataTableUI
            {
                Id = "dtCnabItem",
                UrlGridLoad = Url.Action("GridLoadContaCnabItem", "CnabItem"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" },
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "pessoaId", Required = true, Value = "PessoaId" },
                    new DataTableUIParameter { Id = "dataPickerFim", Value = "DataPickerFim" },
                    new DataTableUIParameter { Id = "dataPickerInicio", Value = "DataPickerInicio" }
                }
            };

            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1 });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Vencimento", Priority = 2, Type = "date" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "valorPrevisto", DisplayName = "Valor", Priority = 3, Type = "currency" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "descricaoParcela", DisplayName = "Parcela", Priority = 7 });
            dtConfig.Columns.Add(new DataTableUIColumn
            {
                DataField = "statusContaBancaria",
                DisplayName = "Status",
                Priority = 6,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusContaBancaria))),
                RenderFn = "fnRenderEnum(full.statusContaBancariaCssClass, full.statusContaBancariaNomeCompleto)"
            });
            dtConfig.Columns.Add(new DataTableUIColumn { DisplayName = "Imprimir boleto", Priority = 4, Searchable = false, Orderable = false, RenderFn = "fnImprimirBoletoCnab" });
            dtConfig.Columns.Add(new DataTableUIColumn { DisplayName = "Compartilhar", Priority = 5, Searchable = false, Orderable = false, RenderFn = "fnModalEmail" });
            #endregion

            cfg.Content.Add(dtConfig);

            return cfg;
        }

        public List<HtmlUIButton> GetListButtonsOnHeader(string buttonLabel, string buttonOnClick)
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "btnGerarBoleto", Label = "Gerar Boleto", OnClickFn = "fnNovo", Position = HtmlUIButtonPosition.Main });
                target.Add(new HtmlUIButton { Id = "btnGerarArqRemessa", Label = "Gerar Arq. Remessa", OnClickFn = "fnGerarArquivoRemessa", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "btnSendManyMails", Label = "Enviar Emails", OnClickFn = "fnSendManyMails", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "new", Label = buttonLabel, OnClickFn = buttonOnClick });
            }

            return target;
        }

        public override ContentResult List()
        {
            return ListBoletos();
        }

        public ContentResult ListBoletos(string gridLoad = "GridLoad")
        {
            var buttonLabel = "Mostrar todos os boletos";
            var buttonOnClick = "fnRemoveFilter";

            if (Request.QueryString["action"] == "GridLoadNoFilter")
            {
                gridLoad = Request.QueryString["action"];
                buttonLabel = "Mostrar boletos do mes atual";
                buttonOnClick = "fnAddFilter";
            }

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory() { Default = Url.Action("Index", "Cnab") },
                Header = new HtmlUIHeader()
                {
                    Title = "Boletos",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader(buttonLabel, buttonOnClick))
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnFormReadyCnab", "fnImprimirBoleto" }
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
                }
            };

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

            var dtConfig = new DataTableUI()
            {
                Id = "dtBoletos",
                UrlGridLoad = Url.Action("GridLoad"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnFormReadyCnab", "fnRenderEnum" },
                Options = new DataTableUIConfig()
                {
                    Select = new { style = "multi" },
                    OrderColumn = 2,
                    OrderDir = "asc",
                    NoExportButtons = true
                },
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() {Id = "dataInicial" },
                    new DataTableUIParameter() {Id = "dataFinal" }
                },
            };


            dtConfig.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusCnab))),
                Priority = 6,
                Width = "12%",
                RenderFn = "fnRenderEnum(full.statusCssClass, full.statusDescription, full.statusTooltip)"

            });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "nossoNumeroFormatado", DisplayName = "Nº boleto", Priority = 6 });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "contaReceber_pessoa_nome", Priority = 4, DisplayName = "Cliente" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", Priority = 4, DisplayName = "Data Vencimento", Type = "date" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "contaBancariaCedente_banco_nome", Priority = 4, DisplayName = "Banco" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "contaReceber_pessoa_email", Priority = 5, DisplayName = "Email" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "valorBoleto", Priority = 5, DisplayName = "Valor" });
            dtConfig.Columns.Add(new DataTableUIColumn { DisplayName = "Imprimir", Priority = 2, Searchable = false, Orderable = false, RenderFn = "fnImprimirBoletoCnab" });
            dtConfig.Columns.Add(new DataTableUIColumn { DisplayName = "Compartilhar", Priority = 2, Searchable = false, Orderable = false, RenderFn = "fnModalEmail" });

            cfg.Content.Add(dtConfig);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public ContentResult ModalConfigEmail(string email = "", string contaReceberId = "", string contaBancariaId = "", string ids = "")
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Configuração para o envio de e-mail",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Enviar", OnClickFn = "fnEnviarEmail" },
                CancelAction = new ModalUIAction() { Label = "Cancelar", OnClickFn = "fnCancelarEnvioEmail" },
                Action = new FormUIAction
                {
                    Create = "",
                    Edit = "",
                    List = ""
                },
                Id = "fly01mdlfrmModalConfigEmail",
                ReadyFn = "fnFormReadyModal"
            };

            config.Elements.Add(new InputHiddenUI { Id = "ids", Value = ids });
            config.Elements.Add(new InputHiddenUI { Id = "idContaReceber", Value = contaReceberId });
            config.Elements.Add(new InputHiddenUI { Id = "idContaBancaria", Value = contaBancariaId });
            config.Elements.Add(new InputTextUI { Id = "email", Class = "col s12 l12", Label = "E-mail", Value = email, Required = true, MaxLength = 50 });
            config.Helpers.Add(new TooltipUI
            {
                Id = "email",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Para enviar os boletos para múltiplos endereços de e-mail, separe-os por ponto e vírgula. Exemplo: email1@gmail.com;email2@hotmail.com"
                }
            });

            config.Elements.Add(new InputTextUI { Id = "assunto", Class = "col s12 l12", Label = "Assunto", Readonly = false });
            config.Elements.Add(new TextAreaUI { Id = "mensagem", Class = "col s12 l12", Label = "Mensagem", MaxLength = 150 });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [HttpGet]
        public JsonResult GetTemplateBoleto()
        {
            try
            {
                var templete = RestHelper.ExecuteGetRequest<ResultBase<TemplateBoletoVM>>("templateboleto");

                return Json(new
                {
                    success = true,
                    data = templete.Data.FirstOrDefault()
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
            {
                filters = new Dictionary<string, string>();
            }

            if (Request.QueryString["dataFinal"] != "")
            {
                filters.Add("dataVencimento le ", Request.QueryString["dataFinal"]);
            }

            if (Request.QueryString["dataInicial"] != "")
            {
                filters.Add(" and dataVencimento ge ", Request.QueryString["dataInicial"]);
            }

            return base.GridLoad(filters);
        }

        public JsonResult GridLoadNoFilter()
        {
            return GridLoad();
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}