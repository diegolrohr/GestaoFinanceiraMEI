using Fly01.uiJS.Classes;
using Fly01.uiJS.Defaults;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Rest;
using Fly01.Core.Presentation.Commons;
using Fly01.uiJS.Enums;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Core.Presentation.Controllers
{
    public class CertificadoDigitalBaseController<T> : BaseController<T> where T : CertificadoDigitalVM
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
            var empresa = GetDadosEmpresa();

            try
            {
                var certificadoDigital = GetCertificado();
                if (certificadoDigital != null)
                {
                    string dtExpStr = certificadoDigital.DataExpiracao.Date.ToString("dd/MM/yyyy");

                    if (certificadoDigital.DataExpiracao.Date.CompareTo(DateTime.Now.Date) < 1)
                    {
                        return Json(new
                        {
                            success = true,
                            color = "red",
                            mainInfo = "Seu certificado digital venceu em " + dtExpStr + ".",
                            subInfo = "Atualize o certificado digital."
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else if (certificadoDigital.DataExpiracao.Date.CompareTo(DateTime.Now.AddDays(-30).Date) < 1)
                    {
                        return Json(new
                        {
                            success = true,
                            color = "totvs-blue",
                            mainInfo = "Seu certificado digital irá vencer em " + dtExpStr + ".",
                            subInfo = "Providêncie a atualização do seu certificado digital."
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            success = true,
                            color = "green",
                            mainInfo = "O certificado digital atual é válido até " + dtExpStr + "."
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new
                {
                    success = true,
                    color = "blue",
                    mainInfo = "O CNPJ (" + empresa.CNPJ + ") não possui certificado digital cadastrado.",
                    subInfo = "Envie o arquivo e informe a senha de um certificado digital válido."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                var result = new
                {
                    success = false,
                    color = "blue",
                    mainInfo = "O CNPJ (" + empresa.CNPJ + ") não possui Certificado digital cadastrado.",
                    subInfo = "Envie o arquivo e informe a senha de um certificado válido.",
                    message = error.Message
                };

                return Json(result, JsonRequestBehavior.AllowGet); ;
            }
        }

        public override Func<T, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
            => Form();

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
                target.Add(new HtmlUIButton() { Id = "save", Label = "Atualizar Certificado", OnClickFn = "fnAtualizaCertificado", Type = "submit", Position = HtmlUIButtonPosition.Main });

            return target;
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public override ContentResult Form() => base.Form();

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Certificado Digital A1",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
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

            return cfg;
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

                if (certificadoExistente == null)
                    arquivoRetorno = RestHelper.ExecutePostRequest<CertificadoDigitalVM>(ResourceName, JsonConvert.SerializeObject(arquivoCertificado, JsonSerializerSetting.Default));
                else
                    arquivoRetorno = RestHelper.ExecutePutRequest<CertificadoDigitalVM>($"{ResourceName}/{certificadoExistente.Id}", JsonConvert.SerializeObject(arquivoCertificado, JsonSerializerSetting.Default));

                return Json(new
                {
                    success = true,
                    data = arquivoRetorno,
                    message = "Certificado digital cadastrado com sucesso.",
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
