using Fly01.Faturamento.Controllers.Base;
using Fly01.Faturamento.Entities.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Fly01.Core;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Api;

namespace Fly01.Faturamento.Controllers
{
    public class CertificadoDigitalController : BaseController<CertificadoDigitalVM>
    {
        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            Dictionary<string, string> queryStringDefault = AppDefaults.GetQueryStringDefault();

            return queryStringDefault;
        }

        private CertificadoDigitalVM GetCertificado()
        {
            var response = RestHelper.ExecuteGetRequest<ResultBase<CertificadoDigitalVM>>(ResourceName);

            if (response == null || response.Data == null)
                return null;

            return response.Data.FirstOrDefault();
        }

        public JsonResult StatusCard()
        {
            var certificadoDigital = GetCertificado();

            if (certificadoDigital == null)
                return Json(new { }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                dataExpiracaoCertificado = certificadoDigital.DataExpiracao
            }, JsonRequestBehavior.AllowGet);
        }

        public override Func<CertificadoDigitalVM, object> GetDisplayData()
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
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Certificado Digital",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton() { Id = "save", Label = "Atualizar Certificado", OnClickFn = "fnAtualizaCertificado", Type = "submit" }
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
                ReadyFn = "fnGetStatusCertificado",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputFileUI { Id = "certificado", Class = "col s12 m6", Label = "Arquivo do Certificado Digital (.pfx)", Required = true, Accept = ".pfx" });

            config.Elements.Add(new InputPasswordUI { Id = "senha", Class = "col s12 m6", Label = "Senha do Certificado", Required = true });

            cfg.Content.Add(config);

            cfg.Content.Add(new CardUI()
            {
                Class = "col s12",
                Color = "black",
                Id = "cardCertificado",
                Title = "",
                Placeholder = "Buscando informações do certificado...",
                Action = new LinkUI()
                {
                    Label = ""
                }
            });

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public JsonResult ImportaCertificado(string conteudo, string senha)
        {
            try
            {
                conteudo = conteudo?.Split(',').Last() ?? "";

                var arquivoCertificado = new
                {
                    certificado = conteudo,
                    senha = Base64Helper.CodificaBase64(senha),
                    md5 = Base64Helper.CalculaMD5Hash(conteudo)
                };

                var certificadoExistente = GetCertificado();

                CertificadoDigitalVM arquivoRetorno;

                if(certificadoExistente == null)
                    arquivoRetorno = RestHelper.ExecutePostRequest<CertificadoDigitalVM>(ResourceName, JsonConvert.SerializeObject(arquivoCertificado, JsonSerializerSetting.Default));
                else
                    arquivoRetorno = RestHelper.ExecutePutRequest<CertificadoDigitalVM>($"{ResourceName}/{certificadoExistente.Id}", JsonConvert.SerializeObject(arquivoCertificado, JsonSerializerSetting.Default));

                return Json(new
                {
                    success = true,
                    data = arquivoRetorno,
                    recordsFiltered = 1,
                    recordsTotal = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }
    }
}