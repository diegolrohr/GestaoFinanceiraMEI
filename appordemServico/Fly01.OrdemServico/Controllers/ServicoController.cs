﻿using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.OrdemServico.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.OrdemServico.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoCadastrosServicos)]
    public class ServicoController : BaseController<ServicoVM>
    {
        protected Func<ServicoVM, object> GetDisplayDataSelect { get; set; }
        protected string SelectProperties { get; set; }

        public ServicoController()
        {
            ExpandProperties = "nbs($select=id,descricao)";
            SelectProperties = "id,codigoServico,descricao,registroFixo";

            GetDisplayDataSelect = x => new
            {
                id = x.Id,
                codigoServico = x.CodigoServico,
                descricao = x.Descricao,
                registroFixo = x.RegistroFixo
            };
        }

        public override Func<ServicoVM, object> GetDisplayData()
        {
            return GetDisplayDataSelect;
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("$expand", ExpandProperties);
            customFilters.AddParam("$select", SelectProperties);

            return customFilters;
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Serviços",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "codigoServico", DisplayName = "Código", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 2 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "cancel", Label = "Cancelar", OnClickFn = "fnCancelar", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "saveNew", Label = "Salvar e Novo", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvar", Type = "submit", Position = HtmlUIButtonPosition.Main });
            }

            return target;
        }

        protected override ContentUI FormJson()
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
                    Title = "Dados do Serviço",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = Url.Action("List"),
                    Form = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputTextUI { Id = "codigoServico", Class = "col l3 m3 s12", Label = "Código", Required = true });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col l9 m9 s12", Label = "Descrição", Required = true });

            config.Elements.Add(new InputHiddenUI { Id = "tipoPagamentoImpostoISS" });
            config.Elements.Add(new InputHiddenUI { Id = "tipoTributacaoISS" });

            config.Elements.Add(new InputCurrencyUI { Id = "valorServico", Class = "col l3 m3 s12", Label = "Valor Servico", Required = true });

            config.Elements.Add(new TextAreaUI { Id = "observacao", Class = "col l12 m12 s12", Label = "Observação", MaxLength = 200 });

            cfg.Content.Add(config);

            return cfg;
        }

        public ContentResult FormModal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar Serviço",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                },
                Id = "fly01mdlfrmProduto",
                UrlFunctions = Url.Action("Functions") + "?fns=",
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputTextUI { Id = "codigoServico", Class = "col l6 m6 s12", Label = "Código", Required = true });
            config.Elements.Add(new InputCurrencyUI { Id = "valorServico", Class = "col l6 m6 s12", Label = "Valor Servico", Required = true });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col l12 m12 s12", Label = "Descrição", Required = true });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }
}