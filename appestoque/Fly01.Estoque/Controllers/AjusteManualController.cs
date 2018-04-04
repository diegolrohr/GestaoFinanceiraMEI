﻿using Fly01.Estoque.Controllers.Base;
using Fly01.Estoque.Entities.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core;
using Fly01.Core.Api;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Estoque.Controllers
{
    public class AjusteManualController : BaseController<AjusteManualVM>
    {

        public override Func<AjusteManualVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            return Form();
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
                    Title = "Ajuste manual",
                    Buttons = new List<HtmlUIButton>
                    {
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
                    List = @Url.Action("Form")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>
                {
                    "fnEntradaSaida",
                    "fnChangeTipoProduto",
                    "fnChangeGrupoProduto",
                    "fnChangeNCM"
                }
            };

            config.Elements.Add(new SelectUI()
            {
                Id = "tipoEntradaSaida",
                Class = "col l6 m6 s12",
                Label = "Entrada / Saída",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoEntradaSaida", true, false)),
                DomEvents = new List<DomEventUI> { new DomEventUI { DomEvent = "change", Function = "fnChangeEntradaSaida" } }

            });

            config.Elements.Add(new AutocompleteUI
            {
                Id = "tipoMovimentoId",
                Class = "col l6 m6 s12",
                Label = "Tipo de Movimento",
                Required = true,
                DataUrl = @Url.Action("TipoMovimento", "AutoComplete"),
                DataUrlPost = @Url.Action("NovoTipoMovimento"),
                LabelId = "tipoMovimentoDescricao",
                PreFilter = "tipoEntradaSaida"
            });

            config.Elements.Add(new AutocompleteUI
            {
                Id = "produtoId",
                Class = "col l12 m12 s12",
                Label = "Produto",
                Required = true,
                DataUrl = @Url.Action("Produto", "AutoComplete"),
                LabelId = "produtoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "Produto"),
                DataPostField = "descricao",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "blur", Function = "fnChangeProduto" }
                }
            });

            config.Elements.Add(new InputTextUI { Id = "codigoProduto", Class = "col l4 m4 s12", Label = "Código", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "codigoBarras", Class = "col l4 m4 s12", Label = "Código de barras", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "saldoProduto", Class = "col l4 m4 s12", Label = "Saldo atual", Value = "0", Disabled = true });

            config.Elements.Add(new InputNumbersUI
            {
                Id = "quantidade",
                Class = "col l6 m6 s12",
                Label = "Quantidade",
                Value = "0",
                Required = true,
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeQuantidade" }
                }
            });

            config.Elements.Add(new InputNumbersUI { Id = "novoEstoque", Class = "col l6 m6 s12", Label = "Novo estoque", Value = "0", Disabled = true });

            config.Elements.Add(new TextareaUI { Id = "observacao", Class = "col l12 m12 s12", Label = "Observação", MaxLength = 200 });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        [HttpPost]
        public JsonResult NovoTipoMovimento(string term)
        {
            try
            {
                var tipoProduto = Request.QueryString["tipo"];

                var entity = new TipoMovimentoVM
                {
                    Descricao = term,
                    TipoEntradaSaida = tipoProduto
                };

                var resourceName = AppDefaults.GetResourceName(typeof(TipoMovimentoVM));
                var data = RestHelper.ExecutePostRequest<TipoMovimentoVM>(resourceName, entity, AppDefaults.GetQueryStringDefault());

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }

        }
    }
}