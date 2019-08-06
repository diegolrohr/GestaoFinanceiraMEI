using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class GrupoProdutoBaseController<T> : BaseController<T> where T : GrupoProdutoVM
    {
        public GrupoProdutoBaseController()
        {
            ExpandProperties = "unidadeMedida,ncm";
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var queryString = base.GetQueryStringDefaultGridLoad();
            queryString.Add("$select", "id,descricao,tipoProduto,registroFixo");
            return queryString;
        }

        private List<HtmlUIButton> GetButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" });

            return target;
        }

        public override Func<T, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                descricao = x.Descricao,
                tipoProduto = x.TipoProduto,
                tipoProdutoDescription = EnumHelper.GetDescription(typeof(TipoProduto), x.TipoProduto),
                tipoProdutoCssClass = EnumHelper.GetCSS(typeof(TipoProduto), x.TipoProduto),
                tipoProdutoValue = EnumHelper.GetValue(typeof(TipoProduto), x.TipoProduto),
                registroFixo = x.RegistroFixo
            };
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Grupo de Produtos",
                    Buttons = new List<HtmlUIButton>(GetButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" }
            };

            var config = new DataTableUI { Id = "fly01dt", UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions", "GrupoProduto", null, Request.Url?.Scheme) + "?fns=" };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoProduto",
                DisplayName = "Tipo de Produto",
                Priority = 2,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoProduto))),
                RenderFn = "fnRenderEnum(full.tipoProdutoCssClass, full.tipoProdutoValue)"
            });

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
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create"),
                    WithParams = Url.Action("Edit")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Cadastro de Grupo de Produto",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Id = "fly01frm",
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
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 l6", Label = "Descrição", Required = true, MaxLength = 60 });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "ncmId",
                Class = "col s12 l6",
                Label = "NCM",
                DataUrl = @Url.Action("Ncm", "AutoComplete"),
                LabelId = "ncmDescricao",
                DomEvents = new List<DomEventUI>()
                {
                    new DomEventUI() { DomEvent = "autocompleteselect", Function = "fnChangeNCM" }
                }
            });

            config.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaIpi",
                Class = "col s6 l3",
                Label = "Alíquota Ipi",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoProduto",
                Class = "col s6 l3",
                Label = "Tipo Produto",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoProduto)).ToList().FindAll(x => "ProdutoFinal,Insumo,Outros".Contains(x.Value)).OrderByDescending(x => x.Label)),
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "unidadeMedidaId",
                Class = "col s12 l6",
                Label = "Unidade de Medida",
                DataUrl = @Url.Action("UnidadeMedida", "AutoComplete"),
                LabelId = "unidadeMedidaDescricao"
            });

            cfg.Content.Add(config);

            return cfg;
        }

        public JsonResult GetAliquotaIPI(string ncmId)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(NcmVM));

            var queryString = AppDefaults.GetQueryStringDefault();
            queryString.AddParam("$filter", $"id eq {ncmId}");
            queryString.AddParam("$select", "aliquotaIPI");

            var NCM = RestHelper.ExecuteGetRequest<ResultBase<NcmVM>>(resourceName, queryString).Data.FirstOrDefault();
            return Json(
                new
                {
                    aliquotaIpi = NCM != null ? NCM.AliquotaIPI : 0.0
                },
                JsonRequestBehavior.AllowGet
            );
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }
    }
}