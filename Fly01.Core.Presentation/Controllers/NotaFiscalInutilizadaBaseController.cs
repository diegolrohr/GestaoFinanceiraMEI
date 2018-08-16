using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json.Linq;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Core.Presentation.Controllers
{
    public class NotaFiscalInutilizadaBaseController<T> : BaseController<T> where T: NotaFiscalInutilizadaVM
    {
        public override Func<T, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                serie = x.Serie,
                numNotaFiscal = x.NumNotaFiscal,
                status = x.Status,
                statusDescription = EnumHelper.GetDescription(typeof(StatusNotaFiscal), x.Status),
                statusCssClass = EnumHelper.GetCSS(typeof(StatusNotaFiscal), x.Status),
                statusValue = EnumHelper.GetValue(typeof(StatusNotaFiscal), x.Status),
                data = x.Data.ToString("dd/MM/yyyy"),
                sefazChaveAcesso = x.SefazChaveAcesso,
                mensagem = x.Mensagem,
                recomendacao = x.Recomendacao
            };
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

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create", "NotaFiscalInutilizada"),
                    WithParams = Url.Action("Edit", "SerieNotaFiscal")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Nota Fiscal Inutilizada",
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
                    List = Url.Action("List")
                },
                //ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputCustommaskUI
            {
                Id = "serie",
                Class = "col s12 m6",
                Label = "Série",
                Required = true,
                MinLength = 1,
                MaxLength = 3,
                Data = new { inputmask = "'regex': '[0-9]*'" }
            });

            config.Elements.Add(new InputCustommaskUI
            {
                Id = "numNotaFiscal",
                Class = "col s12 m6",
                Label = "Número Nota Fiscal",
                Required = true,
                MaxLength = 8,
                Data = new { inputmask = "'regex': '[0-9]*'" }
            });

            cfg.Content.Add(config);

            return cfg;
        }

        public override List<HtmlUIButton> GetListButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "atualizarStatusInutilizada", Label = "Atualizar Status", OnClickFn = "fnAtualizarStatusInutilizada" });
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" });
            }

            return target;
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Notas Fiscais Inutilizadas",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new DataTableUI
            {
                UrlGridLoad = Url.Action("GridLoad"),
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" }
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnVisualizarRetornoSefaz", Label = "Mensagem SEFAZ", ShowIf = "(row.status != 'InutilizacaoSolicitada')" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "serie", DisplayName = "Série", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn { DataField = "numNotaFiscal", DisplayName = "Número NF", Priority = 2, Type = "numbers" });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 3,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusNotaFiscal))),
                RenderFn = "fnRenderEnum(full.statusCssClass, full.statusDescription)"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "sefazChaveAcesso", DisplayName = "Sefaz Chave Acesso", Priority = 4 });
            config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 5, Type = "date" });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpGet]
        public JsonResult AtualizaStatus()
        {
            try
            {
                var response = RestHelper.ExecuteGetRequest<JObject>("NotaFiscalInutilizadaAtualizaStatus", queryString: null);

                return Json(
                    new { success = true },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public ContentResult FormRetornoSefaz()
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
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List", "NotaFiscal")
                },
                Id = "fly01mdlfrmVisualizarRetornoSefaz"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new TextAreaUI { Id = "mensagem", Class = "col s12", Label = "Mensagem", Disabled = true });
            config.Elements.Add(new TextAreaUI { Id = "recomendacao", Class = "col s12", Label = "Recomendação", Disabled = true });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }
    }
}