using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.Faturamento.Controllers.Base;
using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Faturamento.Controllers
{
    public class GrupoProdutoController : BaseController<GrupoProdutoVM>
    {
        public GrupoProdutoController()
        {
            ExpandProperties = "unidadeMedida,ncm";
        }

        public override ContentResult Form()
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
                    Title = "Cadastro de Grupo de Produto",
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
                //ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputTextUI { Id = "descricao", Class = "col s12 l6", Label = "Descrição", Required = true, MaxLength = 40 });

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
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoProduto))),
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

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override Func<GrupoProdutoVM, object> GetDisplayData()
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
            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Grupo de Produtos",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" }
            };
            var config = new DataTableUI { UrlGridLoad = Url.Action("GridLoad"), UrlFunctions = Url.Action("Functions", "GrupoProduto", null, Request.Url?.Scheme) + "?fns=" };

            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnEditar", Label = "Editar", ShowIf = "row.registroFixo == 0" });
            config.Actions.Add(new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "row.registroFixo == 0" });

            config.Columns.Add(new DataTableUIColumn { DataField = "descricao", DisplayName = "Descrição", Priority = 1 });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoProduto",
                DisplayName = "Tipo de Produto",
                Priority = 2,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoProduto))),
                RenderFn = "function(data, type, full, meta) { return fnRenderEnum(full.tipoProdutoCssClass, full.tipoProdutoValue); }"
            });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
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
    }
}