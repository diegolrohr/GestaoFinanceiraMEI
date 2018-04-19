using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Faturamento.Controllers.Base;
using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using Fly01.Faturamento.ViewModel;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.API;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Faturamento.Controllers
{
    public class FormaPagamentoController : BaseController<FormaPagamentoVM>
    {
        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("$select", "id,descricao,tipoFormaPagamento");

            return customFilters;
        }

        public override Func<FormaPagamentoVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id,
                descricao = x.Descricao,
                tipoFormaPagamento = EnumHelper.SubtitleDataAnotation("TipoFormaPagamento", x.TipoFormaPagamento).Value
            };
        }

        public override ContentResult List()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory() { Default = Url.Action("Index", "FormaPagamento") },
                Header = new HtmlUIHeader()
                {
                    Title = "Forma de Pagamento",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new DataTableUI() { UrlGridLoad = Url.Action("GridLoad", "FormaPagamento"), UrlFunctions = Url.Action("Functions", "FormaPagamento", null, Request.Url.Scheme) + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir" });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoFormaPagamento",
                DisplayName = "Tipo",
                Priority = 2,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoFormaPagamento", true, false))
            });

            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1 });

            cfg.Content.Add(config);
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create", "FormaPagamento"),
                    WithParams = Url.Action("Edit", "FormaPagamento"),
                },
                Header = new HtmlUIHeader
                {
                    Title = "Dados da forma de pagamento",
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
                    List = @Url.Action("List")
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoFormaPagamento",
                Class = "col s4 l4",
                Label = "Tipo",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoFormaPagamento", true, false))
            });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s8 l8", Label = "Descrição", Required = true });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public ContentResult FormModal()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar Forma de Pagamento",
                UrlFunctions = @Url.Action("Functions", "PedidoItem", null, Request.Url.Scheme) + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                },
                Id = "fly01mdlfrmFormaPagamento",
            };
            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoFormaPagamento",
                Class = "col s4 l4",
                Label = "Tipo",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoFormaPagamento", true, false))
            });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s8 l8", Label = "Descrição", Required = true });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

    }
}