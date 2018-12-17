using Fly01.Core.Presentation;
using Fly01.Financeiro.ViewModel;
using System;
using Fly01.uiJS.Classes;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using System.Collections.Generic;
using Newtonsoft.Json;
using Fly01.Core.Defaults;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core;
using Newtonsoft.Json.Linq;
using Fly01.Core.Config;
using Fly01.Core.ViewModels;
using Fly01.Core.Notifications;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Controllers
{
    public class StoneController : BaseController<DomainBaseVM>
    {
        public override Func<DomainBaseVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        protected FormUI FormLogin()
        {
            var config = new FormUI
            {
                Id = "fly01frmLogin",
                Class = "col s12 m8 offset-m2 center",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                //ReadyFn = "fnFormReady",
                //AfterLoadFn = "fnAfterLoad"
            };

            config.Elements.Add(new StaticTextUI
            {
                Id = "textInfoStone",
                Lines = new List<LineUI>
                {
                    new LineUI()
                    {
                        Tag = "h2",
                        Class = "strong green-text",
                        Text = "Login Stone",
                    }
                }
            });

            config.Elements.Add(new StaticTextUI
            {
                Id = "textInfoBoletos",
                Class = "col s12",
                Lines = new List<LineUI>
                {
                    new LineUI()
                    {
                        Tag = "h5",
                        Class = "light",
                        Text = "Digite sua senha do portal da Stone, para ter acesso à funcionalidade de antecipação de recebíveis.",
                    }
                }
            });
            string email = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl)?.StoneEmail;

            config.Elements.Add(new InputEmailUI { Id = "email", Class = "col s12 m6 offset-m3", Label = "E-mail Stone", MaxLength = 200, Disabled = true, Value = email });

            config.Elements.Add(new InputPasswordUI { Id = "senha", Class = "col s12 m6 offset-m3", Label = "Senha Stone", MaxLength = 200, Required = true });

            config.Elements.Add(new ButtonUI
            {

                Id = "start",
                Value = "Entrar",
                ClassBtn = "green",
                OnClickFn = "fnGetToken"
            });

            return config;
        }

        protected FormUI FormSimulacao()
        {
            var config = new FormUI
            {
                Id = "formAntecipacao",
                Class = "col s12 m8 offset-m2 center",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnReadyRecebiveis",
                //AfterLoadFn = "fnAfterLoad"
            };

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorRecebivel",
                Label = "Valor",
                Class = "col s12 offset-m4 m4",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnRangeChange" }
                }

            });

            config.Elements.Add(new InputRangeUI
            {
                Id = "rangeRecebivel",
                Class = "col s12 offset-m2 m8",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "input mousemove touchmove", Function = "fnRangeChange" }
                }
            });

            config.Elements.Add(new ButtonUI
            {

                Id = "simular",
                Value = "Simular",
                Class = "col s12 offset-m4 m4",
                ClassBtn = "btn-jumbo green",
                OnClickFn = ""
            });

            return config;
        }

        public bool ValidaToken()
        {
            try
            {
                var entity = new
                {
                    Token = SessionManager.Current.UserData.StoneToken
                };

                var response = RestHelper.ExecutePostRequest<JObject>("stone/validartoken", entity, AppDefaults.GetQueryStringDefault());
                bool success = response.Value<bool>("success");

                return success;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public JsonResult GetToken(string senha)
        {
            try
            {
                string email = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl)?.StoneEmail;

                var entity = new AutenticacaoStoneVM
                {
                    Email = email,
                    Password = senha
                };

                var response = RestHelper.ExecutePostRequest<StoneTokenBaseVM>("stone/token", entity);

                SessionManager.Current.UserData.StoneToken = response.Token;

                return JsonResponseStatus.GetSuccess("");

            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);

            }
        }

        public JsonResult AntecipacaoSimular(double valor)
        {
            try
            {
                var entity = new SimularAntecipacaoStoneVM
                {
                    Token = SessionManager.Current.UserData.StoneToken,
                    Valor = valor
                };

                var response = RestHelper.ExecutePostRequest<ResponseAntecipacaoStoneVM>("stone/antecipacaosimular", entity);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetJson(error.Message);
            }
        }

        public JsonResult AntecipacaoEfetivar(double valor, int stoneBancoId, string senha)
        {
            try
            {
                GetToken(senha);

                var entity= new EfetivarAntecipacaoStoneVM
                {
                    Token = SessionManager.Current.UserData.StoneToken,
                    StoneBancoId = stoneBancoId,
                    Valor = valor
                };

                var response = RestHelper.ExecutePostRequest<ResponseAntecipacaoStoneVM>("stone/antecipacaoefetivar", entity);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public JsonResult AntecipacaoConfiguracao()
        {
            try
            {
                var entity = new StoneTokenBaseVM
                {
                    Token = SessionManager.Current.UserData.StoneToken
                };

                var response = RestHelper.ExecutePostRequest<ResponseConfiguracaoStoneVM>("stone/antecipacaoconfiguracao", entity);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public JsonResult AntecipacaoDadosBancarios()
        {
            try
            {
                var entity = new StoneTokenBaseVM
                {
                    Token = SessionManager.Current.UserData.StoneToken
                };

                var response = RestHelper.ExecutePostRequest<ResponseDadosBancariosStoneVM>("stone/antecipacaoconfiguracao", entity);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public JsonResult GetTotalAntecipar()
        {
            try
            {
                var entity = new StoneTokenBaseVM
                {
                    Token = SessionManager.Current.UserData.StoneToken
                };

                var response = RestHelper.ExecutePostRequest<ResponseConsultaTotalStoneVM>("stone/antecipacaoconsultar", entity);
                if (response == null)
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);

                return Json(new
                {
                    totalAntecipavel = response.TotalBrutoAntecipavel,
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);

            }
        }

        public override ContentResult List()
        {
            ContentUI result = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Stone - Antecipação de recebíveis",
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            if (ValidaToken()) // se tem TOKEN VALIDO
                result.Content.Add(FormSimulacao());
            else
                result.Content.Add(FormLogin());

            return Content(JsonConvert.SerializeObject(result, JsonSerializerSetting.Front), "application/json"); ;
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }
    }
}