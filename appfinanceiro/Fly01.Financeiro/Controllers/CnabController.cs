﻿using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Fly01.Financeiro.ViewModel;
using System.Collections.Generic;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Financeiro.Controllers.Base;
using Fly01.Core;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Controllers
{
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
                banco_nome = x.ContaBancariaCedente?.Banco?.Nome,
                pessoa_nome = x.ContaReceber.Pessoa.Nome,
                dataVencimento = x.DataVencimento.ToString("dd/MM/yyyy"),
                valorBoleto = x.ValorBoleto.ToString("C", AppDefaults.CultureInfoDefault),
                valorDesconto = x.ValorDesconto,
                status = x.Status,
                statusCssClass = EnumHelper.GetCSS(typeof(StatusCnab), x.Status),
                statusDescription = EnumHelper.GetDescription(typeof(StatusCnab), x.Status),
                statusTooltip = EnumHelper.GetTooltipHint(typeof(StatusCnab), x.Status),
                dataEmissao = x.DataEmissao.ToString("dd/MM/yyyy")
            };
        }

        public override ContentResult Form()
        {
            #region Headers

            var cfg = new ContentUI
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
                ReadyFn = "fnFormReady"
            };

            configCnab.Elements.Add(new InputHiddenUI { Id = "id" });
            configCnab.Elements.Add(new AutoCompleteUI
            {
                Id = "bancoId",
                Class = "col s12 m6 l6",
                Label = "Banco cedente",
                Required = true,
                DataUrl = @Url.Action("ContaBancariaBancoEmiteBoleto", "AutoComplete") + "?emiteBoleto=true",
                LabelId = "bancoNome"
            });
            configCnab.Elements.Add(new AutoCompleteUI
            {
                Id = "pessoaId",
                Class = "col s12 m6 l6",
                Label = "Cliente",
                Required = true,
                DataUrl = @Url.Action("Cliente", "AutoComplete"),
                LabelId = "pessoaNome",
                DataUrlPost = Url.Action("PostCliente", "Cliente")
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
                    new DataTableUIParameter { Id = "pessoaId", Required = true, Value = "PessoaId" }
                }
            };

            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1 });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", DisplayName = "Vencimento", Priority = 2, Type = "date" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "valorPrevisto", DisplayName = "Valor", Priority = 3, Type = "currency" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "descricaoParcela", DisplayName = "Parcela", Priority = 7});
            dtConfig.Columns.Add(new DataTableUIColumn
            {
                DataField = "statusContaBancaria",
                DisplayName = "Status",
                Priority = 6,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusContaBancaria))),
                RenderFn = "function(data, type, full, meta) { return fnRenderEnum(full.statusContaBancariaCssClass, full.statusContaBancariaNomeCompleto); }"
            });
            dtConfig.Columns.Add(new DataTableUIColumn { DisplayName = "Imprimir boleto", Priority = 4, Searchable = false, Orderable = false, RenderFn = "fnImprimirBoleto"});
            dtConfig.Columns.Add(new DataTableUIColumn { DisplayName = "Compartilhar", Priority = 5, Searchable = false, Orderable = false, RenderFn = "fnModalEmail" });
            #endregion

            cfg.Content.Add(dtConfig);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory() { Default = Url.Action("Index", "Cnab") },
                Header = new HtmlUIHeader()
                {
                    Title = "Boletos",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "btnGerarBoleto", Label = "GERAR BOLETO", OnClickFn = "fnNovo" },
                        new HtmlUIButton { Id = "btnGerarArqRemessa", Label = "GERAR ARQ. REMESSA", OnClickFn = "fnGerarArquivoRemessa" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnFormReadyCnab" }
            };

            var dtConfig = new DataTableUI()
            {
                Id = "dtBoletos",
                UrlGridLoad = Url.Action("GridLoad"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string> { "fnFormReadyCnab", "fnRenderEnum" },
                Options = new DataTableUIConfig()
                {
                    Select = new { style = "multi" }
                }
            };
            dtConfig.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusCnab))),
                Priority = 6,
                Width = "12%",
                RenderFn = "function(data, type, full, meta) { return fnRenderEnum(full.statusCssClass, full.statusDescription, full.statusTooltip); }"

            });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "nossoNumero", DisplayName = "Nº boleto", Priority = 6 });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "pessoa_nome", Priority = 3, DisplayName = "Cliente" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "banco_nome", Priority = 3, DisplayName = "Banco" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "dataVencimento", Priority = 4, DisplayName = "Data Vencimento", Type = "date" });
            dtConfig.Columns.Add(new DataTableUIColumn { DataField = "valorBoleto", Priority = 5, DisplayName = "Valor" });
            dtConfig.Columns.Add(new DataTableUIColumn { DisplayName = "Imprimir", Priority = 2, Searchable = false, Orderable = false, RenderFn = "fnImprimirBoleto" });

            cfg.Content.Add(dtConfig);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public ContentResult ModalConfigEmail(string email, string contaReceberId)
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Configuração para o envio de e-mail",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Enviar", OnClickFn = "fnEnviarEmail" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = "",
                    Edit = "",
                    List = ""
                },
                Id = "fly01mdlfrmModalConfigEmail",
            };

            config.Elements.Add(new InputHiddenUI { Id = "idContaReceber", Value = contaReceberId });
            config.Elements.Add(new InputTextUI { Id = "email", Class = "col s12 l12", Label = "E-mail", Value = email, Required = true, MaxLength = 50 });
            config.Elements.Add(new InputTextUI { Id = "assunto", Class = "col s12 l12", Label = "Assunto", Required = true, Readonly = false });
            config.Elements.Add(new TextAreaUI { Id = "mensagem", Class = "col s12 l12", Label = "Mensagem", Required = true, MaxLength = 150 });
            

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [HttpPost]
        public JsonResult BaixarContasReceber(List<string> IdToBaixa)
        {
            return null;
        }
    }
}