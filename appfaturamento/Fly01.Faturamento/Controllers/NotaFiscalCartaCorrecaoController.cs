using Fly01.Core.Defaults;
using Fly01.Core.Presentation;
using Fly01.Faturamento.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Faturamento.Controllers
{
    public class NotaFiscalCartaCorrecaoController : BaseController<NotaFiscalCartaCorrecaoVM>
    {
        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create", "NotaFiscalCartaCorrecao"),
                    WithParams = Url.Action("Edit", "NotaFiscalCartaCorrecao")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Nova Carta de Correção",
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
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = Url.Action("List")
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "notaFiscalId", Value = "618B82AF-5219-4C14-8AEF-73931FA9D0B2" });
            config.Elements.Add(new TextAreaUI { Id = "mensagemCorrecao", Class = "col s12", Label = "Mensagem Carta de Correção", MaxLength = 1000 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override Func<NotaFiscalCartaCorrecaoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                mensagemCorrecao = x.MensagemCorrecao,
                data = x.Data.ToString("dd/MM/yyyy"),
                notaFiscalId = x.NotaFiscalId
            };
        }
        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index", "NotaFiscalCartaCorrecao" )  },
                Header = new HtmlUIHeader
                {
                    Title = "Cartas de Correção",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "notasFiscaisInutilizadas", Label = "Atualizar Status", OnClickFn = "fnNotaFiscalInutilizadaList" },
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" },
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI {UrlGridLoad = Url.Action("GridLoad", "NotaFiscalCartaCorrecao"), UrlFunctions = Url.Action("Functions", "NotaFiscalCartaCorrecao", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Visualizar" });
            
            config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 1 });

            //config.Columns.Add(new DataTableUIColumn
            //{
            //    DataField = "tipoOperacaoSerieNotaFiscal",
            //    DisplayName = "Operação da Série NF",
            //    Priority = 3,
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoOperacaoSerieNotaFiscal)))
            //});

            config.Columns.Add(new DataTableUIColumn { DataField = "mensagemCorrecao", DisplayName = "Mensagem de Correção", Priority = 2 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }
    }
}