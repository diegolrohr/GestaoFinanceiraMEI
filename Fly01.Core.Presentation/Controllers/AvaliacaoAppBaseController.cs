﻿using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class AvaliacaoAppBaseController<T> : WebBaseController<T> where T :DomainBaseVM
    {
        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Avalie o Aplicativo",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "save", Label = "Enviar", OnClickFn = "fnSalvarAvaliacaoApp", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="

            };
            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "menu", Value= "AvaliaçãoAPP" });
            config.Elements.Add(new InputHiddenUI { Id = "aplicativo"});
            config.Elements.Add(new InputTextUI { Id = "titulo", Class = "col s12 m12 l6", Label = "Título", Required = true, MaxLength = 45 });
            config.Elements.Add(new RatingUI { Id = "satisfacao", Class = "col s12 l3", Label = "Dê sua nota para o aplicativo" });
            config.Elements.Add(new TextareaUI { Id = "descricao", Class = "col s12 m12 24", Label = "Descrição", Required = true });

            cfg.Content.Add(config);
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public override Func<T, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            return Form();
        }

        [HttpPost]
        public override JsonResult Create(T entityVM)
        {
            try
            {
                var postResponse = RestHelper.ExecutePostRequest(ResourceName, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                T postResult = JsonConvert.DeserializeObject<T>(postResponse);
                var response = new JsonResult();
                response.Data = new { success = true, message = "Avaliação enviada com sucesso."};
                return (response);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }
    }
}