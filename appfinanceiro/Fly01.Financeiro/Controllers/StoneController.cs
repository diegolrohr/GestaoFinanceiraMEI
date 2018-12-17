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
using Fly01.uiJS.Classes.Helpers;

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
                Class = "card-panel center col s12 offset-m2 offset-l3 m8 l6",
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
                        Tag = "h3",
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
                        Tag = "p",
                        Class = "center light",
                        Text = "Digite sua senha do portal da Stone, para ter acesso à funcionalidade de antecipação de recebíveis.",
                    }
                }
            });
            string email = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl)?.StoneEmail;

            config.Elements.Add(new InputEmailUI
            {
                Id = "email",
                Class = "col s8",
                Label = "E-mail Stone",
                MaxLength = 200,
                Disabled = true,
                Value = email
            });
            config.Elements.Add(new InputPasswordUI
            {
                Id = "senha",
                Class = "col s8",
                Label = "Senha Stone",
                MaxLength = 200,
                Required = true
            });
            config.Elements.Add(new ButtonUI
            {
                Id = "start",
                Value = "Entrar",
                Class = "col s4",
                ClassBtn = "green btn-large",
                OnClickFn = "fnGetToken"
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "email",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Para alterar o e-mail você precisa ir em dados da Empresa no campo 'E-mail Stone'."
                }
            });

            return config;
        }

        protected void FormSimulacao(ContentUI result)
        {
            var dadosBancarios = AntecipacaoDadosBancarios();
            var total = AntecipacaoConsultar();
            var simulacao = AntecipacaoSimulacao(total.TotalBrutoAntecipavel);

            var config = new FormUI
            {
                Id = "formAntecipacao",
                Class = "col s12 center card green",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            config.Elements.Add(new StaticTextUI
            {
                Class = "col s12 m4 white-text card-content",
                Lines = new List<LineUI>
                {
                    new LineUI
                    {
                        Tag = "span",
                        Class = "card-title",
                        Text = "Total Antecipável"
                    },
                    new LineUI
                    {
                        Id = "totalAntecipavel",
                        Tag = "h5",
                        Class = "",
                        Text = total?.TotalAntecipavelCurrency ?? "R$ 0,00"
                    }
                }
            });
            config.Elements.Add(new StaticTextUI
            {
                Class = "col s12 m4 teal white-text card-content",
                Lines = new List<LineUI>
                {
                    new LineUI
                    {
                        Tag = "span",
                        Class = "card-title",
                        Text = "Líquido Antecipável"
                    },
                    new LineUI
                    {
                        Id = "liquidoAntecipavel",
                        Tag = "h5",
                        Class = "",
                        Text = simulacao?.LiquidoAnteciparCurrency ?? "R$ 0,00"
                    }
                }
            });
            config.Elements.Add(new InputHiddenUI
            {
                Id = "stoneBancoId"
            });

            config.Elements.Add(new StaticTextUI
            {
                Class = "col s12 m4 white-text card-content",
                Lines = new List<LineUI>
                {
                    new LineUI
                    {
                        Tag = "span",
                        Class = "card-title",
                        Text = "Conta Bancária"
                    },
                    new LineUI
                    {
                        Id = "banco",
                        Tag = "h6",
                        Class = "",
                        Text = dadosBancarios?.BancoNome ?? "Banco"
                    },
                    new LineUI
                    {
                        Id = "agencia",
                        Tag = "h6",
                        Class = "",
                        Text = "Ag. " + dadosBancarios?.AgenciaComDigito ?? "Ag. 0000"
                    },
                    new LineUI
                    {
                        Id = "conta",
                        Tag = "h6",
                        Class = "",
                        Text = "Cc. " + dadosBancarios?.ContaComDigito ?? "Cc. 0000-0"
                    }
                }
            });

            var config2 = new FormUI
            {
                Id = "formAntecipacao2",
                Class = "col s12 m4 card",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnReadyRecebiveis"
            };
            config2.Elements.Add(new StaticTextUI
            {
                Class = "col s12 card-content",
                Lines = new List<LineUI>
                {
                    new LineUI
                    {
                        Tag = "span",
                        Class = "green-text card-title",
                        Text = "Simular antecipação"
                    }
                }
            });

            config2.Elements.Add(new InputCurrencyUI
            {
                Id = "valorRecebivel",
                Label = "Valor",
                Class = "col s12 green-text",
                Value = total.TotalBrutoAntecipavel.ToString(),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnRangeChange" }
                }

            });
            config2.Elements.Add(new InputRangeUI
            {
                Id = "rangeRecebivel",
                Class = "col s12 green-text",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "input mousemove touchmove", Function = "fnRangeChange" }
                }
            });

            config2.Elements.Add(new ButtonUI
            {
                Value = "Simular",
                Class = "col s12 m6 offset-m3",
                ClassBtn = "btn-large green",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI
                    {
                        DomEvent = "click",
                        Function = "fnSimular"
                    }
                }
            });

            var configdt = new DataTableUI
            {
                Id = "dtSimulacao",
                Class = "col s12 m8",
                Actions =
                {
                    new DataTableUIAction
                    {
                        OnClickFn = "fnEfetivar",
                        Label = "Efetivar"
                    }
                },
                Columns =
                {
                    new DataTableUIColumn
                    {
                        DataField = "capital",
                        DisplayName = "Capital",
                        Visible = true,
                        Searchable = false,
                        Orderable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "liquido",
                        DisplayName = "Líquido",
                        Visible = true,
                        Searchable = false,
                        Orderable = false
                    },
                },
                UrlGridLoad = " "
            };

            result.Content.Add(config);
            result.Content.Add(config2);
            result.Content.Add(configdt);
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

        protected ResponseAntecipacaoStoneVM AntecipacaoSimulacao(double valor)
        {
            var entity = new SimularAntecipacaoStoneVM
            {
                Token = SessionManager.Current.UserData.StoneToken,
                Valor = valor
            };
            return RestHelper.ExecutePostRequest<ResponseAntecipacaoStoneVM>("stone/antecipacaosimular", entity);
        }

        [HttpGet]
        public ContentResult AntecipacaoSimular(double valor)
        {
            try
            {
                var response = AntecipacaoSimulacao(valor);
                return Content(JsonConvert.SerializeObject(response), "application/json");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return Content(JsonConvert.SerializeObject(JsonResponseStatus.GetFailure(error.Message)), "application/json");
            }
        }

        public ContentResult AntecipacaoEfetivar(double valor, int stoneBancoId, string senha)
        {
            try
            {
                GetToken(senha);

                var entity = new EfetivarAntecipacaoStoneVM
                {
                    Token = SessionManager.Current.UserData.StoneToken,
                    StoneBancoId = stoneBancoId,
                    Valor = valor
                };

                var response = RestHelper.ExecutePostRequest<ResponseAntecipacaoStoneVM>("stone/antecipacaoefetivar", entity);
                return Content(JsonConvert.SerializeObject(response), "application/json");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return Content(JsonConvert.SerializeObject(JsonResponseStatus.GetFailure(error.Message)), "application/json");
            }
        }

        [HttpGet]
        public ContentResult AntecipacaoConfiguracao()
        {
            try
            {
                var entity = new StoneTokenBaseVM
                {
                    Token = SessionManager.Current.UserData.StoneToken
                };

                var response = RestHelper.ExecutePostRequest<ResponseConfiguracaoStoneVM>("stone/antecipacaoconfiguracao", entity);
                return Content(JsonConvert.SerializeObject(response), "application/json");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return Content(JsonConvert.SerializeObject(JsonResponseStatus.GetFailure(error.Message)), "application/json");
            }
        }

        protected ResponseDadosBancariosStoneVM AntecipacaoDadosBancarios()
        {
            var entity = new StoneTokenBaseVM
            {
                Token = SessionManager.Current.UserData.StoneToken
            };

            return RestHelper.ExecutePostRequest<ResponseDadosBancariosStoneVM>("stone/dadosbancarios", entity);
        }

        [HttpGet]
        protected ResponseConsultaTotalStoneVM AntecipacaoConsultar()
        {
            var entity = new StoneTokenBaseVM
            {
                Token = SessionManager.Current.UserData.StoneToken
            };

            return RestHelper.ExecutePostRequest<ResponseConsultaTotalStoneVM>("stone/antecipacaoconsultar", entity);
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

            if (ValidaToken())// se tem TOKEN VALIDO
                FormSimulacao(result);
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