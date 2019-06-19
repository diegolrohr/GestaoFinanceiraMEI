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
using Fly01.uiJS.Classes.Helpers;
using Fly01.Core.Config;
using Fly01.Core.ValueObjects;

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
            return RestHelper.ExecuteGetRequest<CertificadoDigitalVM>("certificadodigital");
        }

        public JsonResult StatusCard()
        {
            //var empresa = GetDadosEmpresa();

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
                            subInfo = "Atualize o certificado digital.",
                            entidadeHomologacao = certificadoDigital.EntidadeHomologacao,
                            entidadeProducao = certificadoDigital.EntidadeProducao,
                            id = certificadoDigital.Id
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else if (certificadoDigital.DataExpiracao.Date.CompareTo(DateTime.Now.AddDays(-30).Date) < 1)
                    {
                        return Json(new
                        {
                            success = true,
                            color = "totvs-blue",
                            mainInfo = "Seu certificado digital irá vencer em " + dtExpStr + ".",
                            subInfo = "Providêncie a atualização do seu certificado digital.",
                            entidadeHomologacao = certificadoDigital.EntidadeHomologacao,
                            entidadeProducao = certificadoDigital.EntidadeProducao,
                            id = certificadoDigital.Id
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            success = true,
                            color = "green",
                            mainInfo = "O certificado digital atual é válido até " + dtExpStr + ".",
                            entidadeHomologacao = certificadoDigital.EntidadeHomologacao,
                            entidadeProducao = certificadoDigital.EntidadeProducao,
                            id = certificadoDigital.Id
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new
                {
                    success = true,
                    color = "blue",
                    //mainInfo = "O CNPJ (" + empresa.CNPJ + ") não possui certificado digital cadastrado.",
                    mainInfo = "O CNPJ informado nos dados da empresa, não possui certificado digital cadastrado.",
                    subInfo = "Envie o arquivo e informe a senha de um certificado digital válido.",
                    entidadeHomologacao = "",
                    entidadeProducao = "",
                    id = "",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                var result = new
                {
                    success = false,
                    color = "blue",
                    //mainInfo = "O CNPJ (" + empresa.CNPJ + ") não possui Certificado digital cadastrado.",
                    mainInfo = "O CNPJ informado nos dados da empresa, não possui certificado digital cadastrado.",
                    subInfo = "Envie o arquivo e informe a senha de um certificado válido.",
                    entidadeHomologacao = "",
                    entidadeProducao = "",
                    id = "",
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
            {
                target.Add(new HtmlUIButton() { Id = "save", Label = "Atualizar Certificado", OnClickFn = "fnAtualizaCertificado", Type = "submit", Position = HtmlUIButtonPosition.Main });
                target.Add(new HtmlUIButton() { Id = "delete", Label = "Remover Certificado", OnClickFn = "fnRemoveCertificado", Type = "submit", Position = HtmlUIButtonPosition.Out });

            }

            return target;
        }

        //public JsonResult RemoveCertificados()
        //{
        //    try
        //    {
        //        var response = RestHelper.ExecutePostRequest("removecertificados", "", null, 150);
        //        //ExecutePutRequest<ManagerEmpresaVM>($"{AppDefaults.UrlManager}company/{SessionManager.Current.UserData.PlatformUrl}", empresa, AppDefaults.GetQueryStringDefault());
        //        return Json(new
        //        {
        //            success = true,
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public override ContentResult Form() => base.Form();

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
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
                Id = "fly01frm",
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

            config.Elements.Add(new InputTextUI { Id = "entidadeHomologacao", Class = "col s12 m6", Label = "Entidade TSS Homologação", Readonly = true });

            config.Elements.Add(new InputTextUI { Id = "entidadeProducao", Class = "col s12 m6", Label = "Entidade TSS Produção", Readonly = true });

            #region Helpers 
            config.Helpers.Add(new TooltipUI
            {
                Id = "entidadeHomologacao",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Código da empresa no TSS (Cnpj, Uf, Inscrição Estadual). Informação para suporte técnico. Necessário envio do certificado digital."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "entidadeProducao",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Código da empresa no TSS (Cnpj, Uf, Inscrição Estadual). Informação para suporte técnico. Necessário envio do certificado digital."
                }
            });
            #endregion

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

        public JsonResult ImportaCertificado(string id, string conteudo, string senha)
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

                CertificadoDigitalVM arquivoRetorno;

                if (string.IsNullOrEmpty(id))
                {
                    arquivoRetorno = RestHelper.ExecutePostRequest<CertificadoDigitalVM>(ResourceName, JsonConvert.SerializeObject(arquivoCertificado, JsonSerializerSetting.Default));
                    id = arquivoRetorno?.Id.ToString();
                }
                else
                    arquivoRetorno = RestHelper.ExecutePutRequest<CertificadoDigitalVM>($"{ResourceName}/{id}", JsonConvert.SerializeObject(arquivoCertificado, JsonSerializerSetting.Default));

                return Json(new
                {
                    success = true,
                    id = id,
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

        public JsonResult ValidaDadosEmpresa()
        {
            try
            {
                ManagerEmpresaVM empresa = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);
                if (!string.IsNullOrEmpty(empresa.InscricaoEstadual))
                {
                    return Json(new
                    {
                        success = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public ContentResult ModalAtualizaIE()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Atualizar Inscrição Estadual:",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Enviar", OnClickFn = "fnFormReadyAtualizaIE" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                Id = "fly01mdlfrmAtualizaIE"
            };
            config.Elements.Add(new InputTextUI
            {
                Id = "inscricaoEstadualId",
                Class = "col s12 m8",
                Label = "Inscrição Estadual"
            });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "chkIsento",
                Class = "col s12 m4",
                Label = "Sim, é isento de Inscrição Estadual?",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI {DomEvent = "change", Function = "fnChkIsentoInscricaoEstadual"}
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "inscricaoEstadualId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Verificamos que você não possui cadastrado sua Inscrição Estadual nos dados de sua Empresa. Por favor, insira sua inscrição estadual para realizarmos o upload do certificado digital."
                }
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public JsonResult PostAtualizacaoIE(string inscricaoEstadual)
        {
            try
            {
                ManagerEmpresaVM empresa = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);

                var msgErrorInscricaoEstadual = string.Empty;
                if (InscricaoEstadualHelper.IsValid(empresa.Cidade?.Estado?.Sigla, inscricaoEstadual, out msgErrorInscricaoEstadual))
                {
                    empresa.InscricaoEstadual = inscricaoEstadual;

                    var response = RestHelper.ExecutePutRequest<ManagerEmpresaVM>($"{AppDefaults.UrlManager}company/{SessionManager.Current.UserData.PlatformUrl}", empresa, AppDefaults.GetQueryStringDefault());

                    return Json(new
                    {
                        success = true,
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    success = false,
                    message = "Inscrição Estudal Inválida."
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
