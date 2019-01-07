using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoCadastrosKit)]
    public class KitController : BaseController<KitVM>
    {
        public override Func<KitVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Kits",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI { Id = "fly01dt", UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 2 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }
    }
}