﻿using Fly01.Core.Presentation;
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
using Fly01.uiJS.Enums;
using System.Linq;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroFinanceiroAntecipacaoRecebiveis, PermissionValue = EPermissionValue.Write)]
    public class StoneController : BaseController<DomainBaseVM>
    {
        public override Func<DomainBaseVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        protected FormUI FormLogin()
        {
            var config = new FormUI
            {
                Id = "fly01frmLogin",
                Class = "card-panel center col s12 m10 l8 offset-m1 offset-l2",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
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
                Class = "col s12 m10 l8 offset-m1 offset-l2",
                Label = "E-mail Stone",
                MaxLength = 200,
                Disabled = true,
                Value = email
            });
            config.Elements.Add(new InputPasswordUI
            {
                Id = "senha",
                Class = "col s12 m10 l8 offset-m1 offset-l2",
                Label = "Senha Stone",
                MaxLength = 200,
                Required = true
            });
            config.Elements.Add(new ButtonUI
            {
                Id = "start",
                Value = "Entrar",
                Class = "col s12 m10 l8 offset-m1 offset-l2",
                ClassBtn = "btn-large center",
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

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        protected void FormSimulacao(ContentUI content)
        {
            var dadosBancarios = AntecipacaoDadosBancarios();
            var total = AntecipacaoConsultar();
            var configuracao = AntecipacaoConfiguracao();
            var simulacao = new ResponseAntecipacaoStoneVM();

            if (total.TotalBrutoAntecipavel > 0)
                simulacao = AntecipacaoSimulacao(total.TotalBrutoAntecipavel);

            content.Content.Add(new DivUI
            {
                Id = "frmSim",
                Class = "col s12 m6 l8 card",
                Elements = new List<BaseUI>()
            });
            content.Content.Add(new DivUI
            {
                Id = "divInf",
                Class = "col s12 m6 l4",
                Elements = new List<BaseUI>()
            });
            content.Content.Add(new CardUI
            {
                Parent = "divInf",
                Class = "col s12 green-text",
                Color = "white",
                Title = "Total antecipável",
                Placeholder = total.TotalAntecipavelCurrency
            });
            content.Content.Add(new CardUI
            {
                Parent = "divInf",
                Class = "col s12 totvs-blue-text",
                Color = "white",
                Title = "Líquido antecipável",
                Placeholder = simulacao.LiquidoAnteciparCurrency
            });
            content.Content.Add(new CardUI
            {
                Id = "cardTaxa",
                Parent = "divInf",
                Class = "col s12 teal-text",
                Color = "white",
                Title = "Taxa Pontual",
                Placeholder = configuracao.TaxaAntecipacaoPontualCurrency
            });

            content.Content.Add(new CardUI
            {
                Id = "cardSaldoDevedor",
                Parent = "divInf",
                Class = "col s12 teal-text",
                Color = "white",
                //Title = dadosBancarios.BancoNome,
                //Placeholder = string.Format("AG: {0} / CC: {1}", dadosBancarios.Agencia, dadosBancarios.ContaComDigito)
                Title = "Saldo Devedor",
                Placeholder = total.SaldoDevedorCurrency
            });

            var config = new FormUI
            {
                Id = "formAntecipacao",
                Parent = "frmSim",
                Class = "col s12",
                UrlFunctions = Url.Action("Functions") + "?fns=fnEfetivar,",
                ReadyFn = "fnReadyRecebiveis"
            };
            config.Elements.Add(new StaticTextUI
            {
                Class = "col s12 card-content",
                Lines = new List<LineUI>
                {
                    new LineUI
                    {
                        Id = "tituloSimular",
                        Tag = "p",
                        Class = "center card-title",
                        Text = "Simular antecipação"
                    }
                }
            });
            config.Elements.Add(new InputHiddenUI
            {
                Id = "bancoStoneId",
                Value = dadosBancarios.Id.ToString()
            });
            config.Elements.Add(new InputHiddenUI
            {
                Id = "totalAntecipavel",
                Value = total.TotalBrutoAntecipavel.ToString()
            });
            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorRecebivel",
                Label = "Valor",
                Class = "col s12",
                Value = total.TotalBrutoAntecipavel.ToString(),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "input mousemove touchmove", Function = "fnRangeChange" }
                },
                Disabled = total.TotalBrutoAntecipavel == 0
            });
            config.Elements.Add(new InputRangeUI
            {
                Id = "rangeRecebivel",
                Class = "col s12",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "input mousemove touchmove", Function = "fnRangeChange" }
                }
            });
            config.Elements.Add(new ButtonUI
            {
                Id = "btnSimular",
                Value = "Simular",
                Class = "col s8 m6 l4 offset-s2 offset-m3 offset-l4 center",
                ClassBtn = "btn-large",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI
                    {
                        DomEvent = "click",
                        Function = "fnSimular",
                    }
                },
                Disabled = total.TotalBrutoAntecipavel == 0
            });

            #region Helpers
            config.Helpers.Add(new TooltipUI
            {
                Id = "cardSaldoDevedor .card-title",
                Tooltip = new HelperUITooltip()
                {
                    Text = "É o saldo dos débitos pendentes com a Stone, como Aluguel da Maquininha, Chargeback e Cancelamentos."
                }
            });
            config.Helpers.Add(new VideoHelperUI
            {
                Id = "cardTaxa .card-title",
                Video = new HelperUIVideo
                {
                    Youtube = "sUHOeyQJHtg"
                }
            });
            #endregion

            var configdt = new DataTableUI
            {
                Id = "dtSimulacao",
                Parent = "frmSim",
                Class = "col s12",
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
                        DataField = "bruto",
                        DisplayName = "Bruto",
                        Visible = true,
                        Searchable = false,
                        Orderable = false
                    },
                    new DataTableUIColumn
                    {
                        DataField = "taxa",
                        DisplayName = "Taxa",
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
                UrlGridLoad = ""
            };

            content.Content.Add(config);
            content.Content.Add(configdt);
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public ContentResult FormEfetivar(double valor, string stoneBancoId)
        {
            if (ValidaToken())
            {
                var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
                {
                    History = new ContentUIHistory
                    {
                        Default = Url.Action("EfetivarAntecipacao"),
                        WithParams = Url.Action("EfetivarAntecipacao")
                    },
                    Header = new HtmlUIHeader
                    {
                        Title = "Stone - Efetivar Antecipação"
                    },
                    UrlFunctions = Url.Action("Functions") + "?fns="
                };

                var config = new FormUI
                {
                    Id = "fly01frm",
                    Class = "col s12 m7 xl6 offset-xl1 card",
                    Action = new FormUIAction
                    {
                        Create = Url.Action("AntecipacaoEfetivar", "Stone"),
                        Get = Url.Action("Json") + "/",
                        List = Url.Action("List", "Stone")
                    },
                    UrlFunctions = Url.Action("Functions") + "?fns=",
                    ReadyFn = "fnFormReadyEfetivar"
                };
                config.Elements.Add(new InputHiddenUI { Id = "stoneBancoId", Value = stoneBancoId });
                config.Elements.Add(new InputHiddenUI { Id = "valor", Value = valor.ToString() });
                config.Elements.Add(new StaticTextUI
                {
                    Id = "textInfoStone",
                    Class = "col s12 card - content",
                    Lines = new List<LineUI>
                    {
                        new LineUI()
                        {
                            Tag = "p",
                            Class = "center card-title",
                            Text = "Dados da antecipação",
                        }
                    }
                });

                config.Elements.Add(new StaticTextUI
                {
                    Id = "textValorLiquido",
                    Class = "col s12 green-text",
                    Lines = new List<LineUI>
                    {
                        new LineUI()
                        {
                            Tag = "h5",
                            Class = "col s6 light right-align truncate",
                            Text = "Valor líquido:"
                        },
                        new LineUI()
                        {
                            Id = "valorLiquido",
                            Tag = "h5",
                            Class = "col s6 truncate",
                            Text = "R$ 00 mudar",
                        }
                    }
                });
                config.Elements.Add(new StaticTextUI
                {
                    Class = "col s12",
                    Lines = new List<LineUI>
                    {
                        new LineUI()
                        {
                            Id = "valorBruto",
                            Tag = "h6",
                            Class = "truncate col s6 light right-align",
                            Text = "Valor Bruto:"
                        },
                        new LineUI()
                        {
                            Tag = "h5",
                            Class = "col s6 truncate",
                            Text = "R$ 00 mudar",
                        }
                    }
                });
                config.Elements.Add(new InputPasswordUI
                {
                    Id = "senha",
                    Class = "col s12",
                    Label = "Senha Stone",
                    MaxLength = 200,
                    Required = true
                });

                cfg.Content.Add(config);

                return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
            }
            else
            {
                return List();
            }
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

        [OperationRole(PermissionValue = EPermissionValue.Write)]
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
                //SessionManager.Current.UserData.StoneToken = null;
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
        [OperationRole(PermissionValue = EPermissionValue.Write)]
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

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public JsonResult AntecipacaoEfetivar(EfetivarAntecipacaoStoneVM entity)
        {
            try
            {
                var result = GetToken(entity.Senha);
                var property = result.Data.GetType().GetProperty("success");
                var tokenValido = (bool)property.GetValue(result.Data, null);
                if (tokenValido)
                {
                    entity.Token = SessionManager.Current.UserData.StoneToken;
                    var response = RestHelper.ExecutePostRequest<ResponseAntecipacaoStoneVM>("stone/antecipacaoefetivar", entity);
                    return JsonResponseStatus.Get(new ErrorInfo { HasError = false }, Operation.Create);
                }
                else
                {
                    return JsonResponseStatus.GetFailure("Não foi possível obter autenticação na stone. Verifique a senha digitada e tente novamente.");
                }
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public ResponseConfiguracaoStoneVM AntecipacaoConfiguracao()
        {
            var entity = new StoneTokenBaseVM
            {
                Token = SessionManager.Current.UserData.StoneToken
            };

            return RestHelper.ExecutePostRequest<ResponseConfiguracaoStoneVM>("stone/antecipacaoconfiguracao", entity);
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

        [HttpGet]
        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public ActionResult Logout()
        {
            SessionManager.Current.UserData.StoneToken = null;
            return View("Index");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
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
                    Title = "Stone - Antecipação de recebíveis"
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            if (ValidaToken())
                FormSimulacao(result);
            else
                result.Content.Add(FormLogin());

            return Content(JsonConvert.SerializeObject(result, JsonSerializerSetting.Front), "application/json"); ;
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public virtual ActionResult EfetivarAntecipacao()
            => View("EfetivarAntecipacao");
    }
}