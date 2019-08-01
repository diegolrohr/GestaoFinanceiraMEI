using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Financeiro.ViewModel;
using Fly01.uiJS.Classes;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(NotApply = true)]
    public class FeriadoController : BaseController<FeriadoVM>
    {
        protected override ContentUI FormJson() { throw new NotImplementedException(); }

        public override Func<FeriadoVM, object> GetDisplayData()
        {
            return x => new
            {
                Id = x.Id.ToString(),
                x.Descricao,
                x.Dia,
                x.Mes,
                x.Ano
            };
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index", "Feriado") },
                Header = new HtmlUIHeader
                {
                    Title = "Feriados",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions", "Feriado", null, Request.Url.Scheme) + "?fns="
            };

            var config = new DataTableUI { Id = "fly01dt", UrlGridLoad = Url.Action("GridLoad", "Feriado"), UrlFunctions = Url.Action("Functions", "Feriado", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "Descricao", DisplayName = "Descrição", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "Dia", DisplayName = "Dia", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "Mes", DisplayName = "Mês", Priority = 3 });
            config.Columns.Add(new DataTableUIColumn { DataField = "Ano", DisplayName = "Ano", Priority = 4 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}