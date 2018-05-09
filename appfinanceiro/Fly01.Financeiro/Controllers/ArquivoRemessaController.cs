using System;
using System.Web.Mvc;
using Fly01.Financeiro.Controllers.Base;
using Fly01.Financeiro.ViewModel;
using Fly01.uiJS.Classes;
using System.Collections.Generic;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.Controllers
{
    public class ArquivoRemessaController : BaseController<ArquivoRemessaVM>
    {
        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override Func<ArquivoRemessaVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Clientes",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" },
                        new HtmlUIButton { Id = "import", Label = "Importar clientes", OnClickFn = "fnImportarCadastro" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions") + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" });

            config.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Nº", Priority = 1, Type = "number" });
            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descricão Arquivo", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "dataExportacao", DisplayName = "Vencimento", Priority = 2, Type = "date" });
            config.Columns.Add(new DataTableUIColumn { DataField = "totalBoletos", DisplayName = "Valor Total", Priority = 5, Orderable = false, Searchable = false });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "statusArquivoRemessa",
                DisplayName = "Status",
                Priority = 0,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusArquivoRemessa)))
            });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }
    }
}