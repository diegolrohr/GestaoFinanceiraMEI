﻿using Fly01.Core;
using Fly01.uiJS.Classes;
using Fly01.Core.Entities.ViewModels.Commons;
using Fly01.Faturamento.Entities.ViewModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.uiJS.Defaults;
using Fly01.Core.Helpers;
using Fly01.Core.Config;
using System;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Presentation;
using Fly01.Core.Rest;

namespace Fly01.Faturamento.Controllers.Base
{
    public abstract class BaseController<T> : WebBaseController<T> where T : DomainBaseVM
    {
        protected BaseController()
        {
            ResourceName = AppDefaults.GetResourceName(typeof(T));
            APIEnumResourceName = "Fly01.Faturamento.Domain.Enums.";
            AppViewModelResourceName = "Fly01.Faturamento.Entities.ViewModel.";
            AppEntitiesResourceName = "Fly01.Faturamento.Entities";
        }

        public EmpresaVM GetDadosEmpresa()
        {
            return RestHelper.ExecuteGetRequest<EmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{SessionManager.Current.UserData.PlatformUrl}");
        }

        public ContentResult EmConstrucao(string history)
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory() { Default = history },
                Header = new HtmlUIHeader()
                {
                    Title = "Opção indisponível",
                    Buttons = new List<HtmlUIButton>()
                },
                UrlFunctions = ""
            };


            cfg.Content.Add(new FormUI()
            {
                Elements = new List<BaseUI>()
                    {
                        new LabelsetUI()
                        {
                            Class = "col s12",
                            Id = "underconstruction",
                            Name = "underconstruction",
                            Label = "O recurso está em desenvolvimento."
                        }
                    },
                Class = "col s12",

            });

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        [HttpGet]
        public virtual ActionResult Download(string fileName)
        {
            if (Session[fileName] != null)
            {
                byte[] data = Convert.FromBase64String(Session[fileName].ToString());
                Session.Remove(fileName);
                return File(data, "application/octet-stream", fileName);
            }
            else
            {
                return new HttpNotFoundResult("O arquivo solicitado não está disponível para download.");
            }
        }

        [HttpGet]
        public virtual ActionResult DownloadPDF(string fileName)
        {
            if (Session[fileName] != null)
            {
                byte[] data = Convert.FromBase64String(Session[fileName].ToString());
                Session.Remove(fileName);
                return File(data, "application/pdf", fileName);
            }
            else
            {
                return new HttpNotFoundResult("O PDF solicitado não está disponível para download.");
            }
        }

        [HttpGet]
        public virtual ActionResult DownloadXMLString(string fileName)
        {
            if (Session[fileName] != null)
            {
                byte[] data = Convert.FromBase64String(Base64Helper.CodificaBase64(Session[fileName].ToString()));
                Session.Remove(fileName);
                return File(data, "text/xml", fileName);
            }
            else
            {
                return new HttpNotFoundResult("O XML solicitado não está disponível para download.");
            }
        }

    }
}