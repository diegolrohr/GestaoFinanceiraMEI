using Fly01.Core;
using Fly01.Core.Controllers.Web;
using Fly01.uiJS.Classes;
using Fly01.Core.VM;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core.Helpers;
using Fly01.Core.Config;

namespace Fly01.Estoque.Controllers.Base
{
    public abstract class BaseController<T> : WebBaseController<T> where T : DomainBaseVM
    {
        public const string EstoqueAPIEnumResourceName = "Fly01.Estoque.Domain.Enums.";

        protected BaseController()
        {
            ResourceName = AppDefaults.GetResourceName(typeof(T));
            APIEnumResourceName = EstoqueAPIEnumResourceName;
            AppViewModelResourceName = "Fly01.Estoque.Entities.ViewModel.";
            AppEntitiesResourceName = "Fly01.Estoque.Entities";
        }

        public EmpresaVM GetDadosEmpresa()
        {
            var urlGateway = AppDefaults.UrlGateway
                .Replace("financeiro/", string.Empty)
                .Replace("faturamento/", string.Empty)
                .Replace("estoque/", string.Empty)
                .Replace("compras/", string.Empty);

            return RestHelper.ExecuteGetRequest<EmpresaVM>(urlGateway, $"Empresa/{SessionManager.Current.UserData.PlatformUrl}");
        }


        public ContentResult EmConstrucao(string history)
        {
            var config = new ContentUI
            {
                Header = new HtmlUIHeader
                {
                    Title = "Opção indisponível",
                    Buttons = new List<HtmlUIButton>()
                }
            };

            config.Content.Add(new FormUI
            {
                Elements = new List<BaseUI>{
                        new LabelsetUI
                        {
                            Class = "col s12",
                            Id = "underconstruction",
                            Name = "underconstruction",
                            Label = "O recurso está em desenvolvimento."
                        }
                    },
                Class = "col s12"
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Default), "application/json");
        }
    }
}