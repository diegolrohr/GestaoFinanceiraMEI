﻿using Fly01.Core.Defaults;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.ViewModels;
using Fly01.Faturamento.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoFaturamentoNotasFiscais)]
    public class CartaCorrecaoController : BaseController<NotaFiscalCartaCorrecaoVM>
    {
        public CartaCorrecaoController()
        {
            ExpandProperties = "notaFiscal";
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        public ActionResult Novo(Guid id)
        {
            return View(id);
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar" });
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit" });
            }

            return target;
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public ContentResult NovoForm(Guid id)
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Novo", "CartaCorrecao") + "/" + id
                },
                Header = new HtmlUIHeader
                {
                    Title = "Nova Carta de Correção",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("ListCartaCorrecao", "CartaCorrecao") + "/" + id,
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "notaFiscalId", Value = id.ToString() });
            config.Elements.Add(new TextAreaUI { Id = "mensagemCorrecao", Class = "col s12", Label = "Mensagem Carta de Correção", MaxLength = 1000 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override List<HtmlUIButton> GetListButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "notasFiscaisInutilizadas", Label = "Atualizar Status", OnClickFn = "fnNotaFiscalInutilizadaList" });
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" });
            }

            return target;
        }

        public ContentResult ListCartaCorrecao(string id)
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = $"{Url.Action("NotaFiscal", "CartaCorrecao", new { id = id })}" },
                Header = new HtmlUIHeader
                {
                    Title = "Cartas de Correção",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Content = new List<HtmlUIFunctionBase> { new DivUI { Elements = new List<BaseUI> { new InputHiddenUI { Id = "idNotaFiscal", Value = id } } } }
            };
            var config = new DataTableUI
            {
                UrlGridLoad = $"{Url.Action("GridLoad", "CartaCorrecao")}?id={id}",
                UrlFunctions = Url.Action("Functions", "CartaCorrecao", null, Request.Url.Scheme) + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" }
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnVisualizarMensagemSefaz", Label = "Mensagem SEFAZ", ShowIf = "((row.status == 'Rejeitado') || (row.status == 'RegistroENaoVinculado') || (row.status == 'FalhaTransmissao'))" },
                new DataTableUIAction { OnClickFn = "fnVisualizarCartaCorrecao", Label = "Visualizar" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Sequencial", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 2,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusCartaCorrecao))),
                RenderFn = "fnRenderEnum(full.statusCssClass, full.statusDescription)"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "mensagemCorrecao", DisplayName = "Mensagem de Correção", Priority = 3 });
            config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 4 });
            config.Columns.Add(new DataTableUIColumn { DataField = "notaFiscal_numNotaFiscal", DisplayName = "Número Nota", Priority = 5 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public ContentResult FormModal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Carta de Correção",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/"
                },
                Id = "fly01mdlfrmVisualizarCartaCorrecao"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "notafiscalId" });
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m4 l4", Label = "Data", Disabled = true });
            config.Elements.Add(new SelectUI
            {
                Id = "status",
                Class = "col s12 m4 l4",
                Label = "Status",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusCartaCorrecao))),
            });
            config.Elements.Add(new InputTextUI { Id = "numero", Class = "col s12 m4 l4", Label = "Número", Disabled = true });
            config.Elements.Add(new TextAreaUI { Id = "mensagemCorrecao", Class = "col s12", Label = "Mensagem", Disabled = true });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public ContentResult FormModalMensagemSEFAZ()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Mensagem SEFAZ",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/"
                },
                Id = "fly01mdlfrmVisualizarMensagemSEFAZ"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "notafiscalId" });
            config.Elements.Add(new TextAreaUI { Id = "mensagem", Class = "col s12", Label = "Mensagem", Disabled = true });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public ActionResult NotaFiscal(Guid id)
        {
            return View(id);
        }

        public override Func<NotaFiscalCartaCorrecaoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                mensagemCorrecao = x.MensagemCorrecao.Substring(0, x.MensagemCorrecao.Length > 35 ? 35 : x.MensagemCorrecao.Length),
                data = x.Data.ToString("dd/MM/yyyy"),
                notaFiscalId = x.NotaFiscalId,
                status = x.Status,
                statusDescription = EnumHelper.GetDescription(typeof(StatusCartaCorrecao), x.Status),
                statusCssClass = EnumHelper.GetCSS(typeof(StatusCartaCorrecao), x.Status),
                statusValue = EnumHelper.GetValue(typeof(StatusCartaCorrecao), x.Status),
                numero = x.Numero,
                notaFiscal_numNotaFiscal = x.NotaFiscal.NumNotaFiscal
            };
        }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
                filters = new Dictionary<string, string>();

            filters.Add("notaFiscalId eq", Request.QueryString["id"]);

            return base.GridLoad(filters);
        }
    }
}