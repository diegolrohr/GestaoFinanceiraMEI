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
using Fly01.uiJS.Enums;
using System.Linq;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroFinanceiroAntecipacaoRecebiveis, PermissionValue = EPermissionValue.Write)]
    public class StoneController : BaseController<DomainBaseVM>
    {
        #region Autenticação

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
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnReadyLogin"
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

        protected bool ValidaToken()
        {
            try
            {
                var entity = new
                {
                    SessionManager.Current.UserData.Stone.Token
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

        protected StoneTokenBaseVM GetToken(string senha)
        {
            string email = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl)?.StoneEmail;

            var entity = new AutenticacaoStoneVM
            {
                Email = email,
                Password = senha
            };            
            return RestHelper.ExecutePostRequest<StoneTokenBaseVM>("stone/token", entity);            
        }

        public JsonResult Login(string senha)
        {
            try
            {
                SessionManager.Current.UserData.Stone.Token = GetToken(senha).Token;
                AtualizaDadosStone();
                return JsonResponseStatus.GetSuccess("");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        protected void AtualizaDadosStone()
        {
            var entity = new StoneTokenBaseVM
            {
                Token = SessionManager.Current.UserData.Stone.Token
            };

            SessionManager.Current.UserData.Stone.DadosBancarios =
                RestHelper.ExecutePostRequest<StoneDadosBancariosVM>("stone/dadosbancarios", entity);

            SessionManager.Current.UserData.Stone.AntecipacaoConfiguracao =
                RestHelper.ExecutePostRequest<StoneConfiguracaoVM>("stone/antecipacaoconfiguracao", entity);

            SessionManager.Current.UserData.Stone.AntecipacaoTotais =
                RestHelper.ExecutePostRequest<StoneTotaisVM>("stone/antecipacaoconsultar", entity);

            if (SessionManager.Current.UserData.Stone.AntecipacaoTotais.TotalBrutoAntecipavel > 0)
            {
                SessionManager.Current.UserData.Stone.Simulacoes = new List<StoneAntecipacaoVM>();
                AntecipacaoSimulacao(SessionManager.Current.UserData.Stone.AntecipacaoTotais.TotalBrutoAntecipavel);
                SessionManager.Current.UserData.Stone.AntecipacaoTotais.TotalLiquidoAntecipavel =
                    SessionManager.Current.UserData.Stone.Simulacoes.FirstOrDefault().LiquidoAntecipar;
            }
        }

        public ContentResult Logout()
        {
            SessionManager.Current.UserData.Stone = new StoneDataVM();
            return List();
        }

        #endregion

        #region Antecipação

        #region Simulação
        protected List<HtmlUIFunctionBase> FormSimulacao()
        {
            var content = new List<HtmlUIFunctionBase>();
            content.Add(new DivUI
            {
                Id = "frmSim",
                Class = "col s12 l8 card",
                Elements = new List<BaseUI>()
            });
            content.Add(new DivUI
            {
                Id = "divInf",
                Class = "col s12 l4",
                Elements = new List<BaseUI>()
            });
            content.Add(new CardUI
            {
                Parent = "divInf",
                Class = "col s12 green-text",
                Color = "white",
                Title = "Total antecipável",
                Placeholder = SessionManager.Current.UserData.Stone.AntecipacaoTotais.TotalBrutoAntecipavelCurrency
            });
            content.Add(new CardUI
            {
                Parent = "divInf",
                Class = "col s12 green-text",
                Color = "white",
                Title = "Líquido antecipável",
                Placeholder = SessionManager.Current.UserData.Stone.AntecipacaoTotais.TotalLiquidoAntecipavelCurrency
            });
            content.Add(new CardUI
            {
                Id = "cardTaxa",
                Parent = "divInf",
                Class = "col s12 green-text",
                Color = "white",
                Title = "Taxa pontual",
                Placeholder = SessionManager.Current.UserData.Stone.AntecipacaoConfiguracao.TaxaAntecipacaoPontualPercent
            });

            var config = new FormUI
            {
                Id = "formAntecipacao",
                Parent = "frmSim",
                Class = "col s12",
                UrlFunctions = Url.Action("Functions") + "?fns=fnEfetivar,fnExcluir,fnAtualizarDadosStone,",

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
            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorRecebivel",
                Label = "Valor",
                Class = "col s12",
                Value = SessionManager.Current.UserData.Stone.AntecipacaoTotais.TotalBrutoAntecipavel.ToString(),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "input mousemove touchmove", Function = "fnRangeChange" }
                },
                Disabled = SessionManager.Current.UserData.Stone.AntecipacaoTotais.TotalBrutoAntecipavel == 0
            });
            config.Elements.Add(new InputRangeUI
            {
                Id = "rangeRecebivel",
                Class = "col s12",
                Min = 0,
                Max = SessionManager.Current.UserData.Stone.AntecipacaoTotais.TotalBrutoAntecipavel,
                Value = SessionManager.Current.UserData.Stone.AntecipacaoTotais.TotalBrutoAntecipavel.ToString(),
                Step = 0.01,
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
                Disabled = SessionManager.Current.UserData.Stone.AntecipacaoTotais.TotalBrutoAntecipavel == 0
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
                    },
                    new DataTableUIAction
                    {
                        OnClickFn = "fnExcluir",
                        Label = "Excluir"
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
                        Orderable = false,
                        Priority = 2
                    },
                    new DataTableUIColumn
                    {
                        DataField = "taxa",
                        DisplayName = "Taxa",
                        Visible = true,
                        Searchable = false,
                        Orderable = false,
                        Priority = 3
                    },
                    new DataTableUIColumn
                    {
                        DataField = "liquido",
                        DisplayName = "Líquido",
                        Visible = true,
                        Searchable = false,
                        Orderable = false,
                        Priority = 1
                    },
                },
                UrlGridLoad = Url.Action("GridLoadSimulacoes")
            };

            content.Add(config);
            content.Add(configdt);

            return content;
        }

        protected void AntecipacaoSimulacao(double valor)
        {
            if (valor > 0 && !SessionManager.Current.UserData.Stone.Simulacoes.Any(x => Math.Round(x.BrutoAntecipar, 2) == Math.Round(valor, 2)))
            {
                var entity = new StoneAntecipacaoSimularVM
                {
                    Token = SessionManager.Current.UserData.Stone.Token,
                    Valor = valor
                };
                var response = RestHelper.ExecutePostRequest<StoneAntecipacaoVM>("stone/antecipacaosimular", entity);
                SessionManager.Current.UserData.Stone.Simulacoes.Add(response);
            }
        }

        [HttpPost]
        public virtual JsonResult DeleteSimulacao(Guid id)
        {
            try
            {
                SessionManager.Current.UserData.Stone.Simulacoes.Remove(
                    SessionManager.Current.UserData.Stone.Simulacoes.Where(x => x.Id == id).FirstOrDefault()
                );

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Delete);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpGet]
        public ContentResult AntecipacaoSimular(double valor)
        {
            try
            {
                AntecipacaoSimulacao(valor);
                return Content(JsonConvert.SerializeObject(new { success = true }), "application/json");
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return Content(JsonConvert.SerializeObject(JsonResponseStatus.GetFailure(error.Message)), "application/json");
            }
        }

        public JsonResult GridLoadSimulacoes(Dictionary<string, string> filters = null)
        {
            try
            {
                return Json(new
                {
                    recordsTotal = SessionManager.Current.UserData.Stone.Simulacoes.Count(),
                    recordsFiltered = SessionManager.Current.UserData.Stone.Simulacoes.Count(),
                    data = SessionManager.Current.UserData.Stone.Simulacoes.Select(x => new
                    {
                        id = x.Id,
                        bruto = x.BrutoAnteciparCurrency,
                        taxa = x.TaxaPontualPercent,
                        liquido = x.LiquidoAnteciparCurrency
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    queryStringFilter = string.Empty,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new { },
                    success = false,
                    message = string.Format("Ocorreu um erro ao carregar dados: {0}", ex.Message)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Efetivação
        public ContentResult FormEfetivar(Guid? id)
        {
            if (ValidaToken())
            {
                if (!SessionManager.Current.UserData.Stone.Simulacoes.Any(x => x.Id == id))
                    return List();

                var simulacao = SessionManager.Current.UserData.Stone.Simulacoes.FirstOrDefault(x => x.Id == id);

                var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
                {
                    History = new ContentUIHistory
                    {
                        Default = Url.Action("AntecipacaoEfetivar") + "/" + id.ToString()
                    },
                    Header = new HtmlUIHeader
                    {
                        Title = "Stone - Efetivar antecipação"
                    },
                    UrlFunctions = Url.Action("Functions") + "?fns="
                };

                var config = new FormUI
                {
                    Id = "fly01frm",
                    Class = "col s12 xl6 offset-xl3 card",
                    Action = new FormUIAction
                    {
                        Create = Url.Action("AntecipacaoEfetivar", "Stone"),
                        Get = Url.Action("Json") + "/",
                        List = Url.Action("List", "Stone")
                    },
                    UrlFunctions = Url.Action("Functions") + "?fns="
                };
                config.Elements.Add(new InputHiddenUI { Id = "id", Value = simulacao.Id.ToString() });
                config.Elements.Add(new StaticTextUI
                {
                    Id = "textInfoStone",
                    Class = "col s12 card-content",
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
                            Text = simulacao.LiquidoAnteciparCurrency,
                        }
                    }
                });
                config.Elements.Add(new StaticTextUI
                {
                    Class = "col s12",
                    Lines = new List<LineUI>
                    {
                        new LineUI(){ Tag = "h6", Class = "truncate col s6 light right-align", Text = "Taxa:" },
                        new LineUI(){ Tag = "h6", Class = "truncate col s6", Text = simulacao.TaxaPontualPercent },
                        new LineUI(){ Tag = "h6", Class = "truncate col s6 light right-align", Text = "Valor Bruto:" },
                        new LineUI(){ Tag = "h6", Class = "truncate col s6", Text = simulacao.BrutoAnteciparCurrency },
                        new LineUI(){ Tag = "h6", Class = "truncate col s6 light right-align", Text = "Data Pagamento:" },
                        new LineUI(){ Tag = "h6", Class = "truncate col s6", Text = simulacao.Data.ToString("dd/MM/yyyy")},
                        new LineUI(){ Tag = "h6", Class = "truncate col s6 light right-align", Text = "Banco:" },
                        new LineUI(){ Tag = "h6", Class = "truncate col s6", Text = SessionManager.Current.UserData.Stone.DadosBancarios.BancoComCodigo },
                        new LineUI(){ Tag = "h6", Class = "truncate col s6 light right-align", Text = "Agência:" },
                        new LineUI(){ Tag = "h6", Class = "truncate col s6", Text = SessionManager.Current.UserData.Stone.DadosBancarios.Agencia },
                        new LineUI(){ Tag = "h6", Class = "truncate col s6 light right-align", Text = "Conta:" },
                        new LineUI(){ Tag = "h6", Class = "truncate col s6", Text = SessionManager.Current.UserData.Stone.DadosBancarios.ContaComDigito }
                    }
                });
                config.Elements.Add(new StaticTextUI
                {
                    Id = "textInfoStone",
                    Class = "col s12 card-content",
                    Lines = new List<LineUI>
                    {
                        new LineUI()
                        {
                            Tag = "h6",
                            Class = "col s12 center",
                            Text = "Digite sua senha para confirmar e realizar a antecipação:",
                        }
                    }
                });
                config.Elements.Add(new InputPasswordUI
                {
                    Id = "senha",
                    Class = "col s12 m8 l6 offset-m2 offset-l3",
                    Label = "Senha Stone",
                    MaxLength = 200,
                    Required = true
                });
                config.Elements.Add(new ButtonUI
                {
                    Id = "btnCancelar",
                    Class = "col s6 right-align",
                    Value = "Cancelar",
                    ClassBtn = "btn-secondary btn-narrow",
                    DomEvents = new List<DomEventUI>
                    {
                        new DomEventUI
                        {
                            DomEvent = "click", Function = "fnCancelar"
                        }
                    }
                });
                config.Elements.Add(new ButtonUI
                {
                    Id = "btnEfetivar",
                    Class = "col s6",
                    Value = "Efetivar",
                    ClassBtn = "btn-narrow",
                    DomEvents = new List<DomEventUI>
                    {
                        new DomEventUI
                        {
                            DomEvent = "click", Function = "fnSalvar"
                        }
                    }
                });

                cfg.Content.Add(config);

                return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
            }
            else
            {
                return List();
            }
        }

        [HttpPost]
        public JsonResult AntecipacaoEfetivar(StoneAntecipacaoEfetivarPostVM entity)
        {
            try
            {
                var result = GetToken(entity.Senha);
                
                if (!string.IsNullOrWhiteSpace(result.Token))
                {
                    entity.StoneBancoId = SessionManager.Current.UserData.Stone.DadosBancarios.Id;
                    entity.Valor = SessionManager.Current.UserData.Stone.Simulacoes.FirstOrDefault(x => x.Id == entity.Id).BrutoAntecipar;
                    entity.Token = SessionManager.Current.UserData.Stone.Token;
                    var response = RestHelper.ExecutePostRequest<StoneAntecipacaoEfetivarVM>("stone/antecipacaoefetivar", entity);
                    AtualizaDadosStone();
                    return JsonResponseStatus.GetSuccess("Antecipação realizada com sucesso.");
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

        public virtual ActionResult AntecipacaoEfetivar(Guid? id)
            => View("AntecipacaoEfetivar", id);

        #endregion

        #endregion

        public ContentResult ListAndUpdate()
        {
            if (ValidaToken())
                AtualizaDadosStone();
            return List();
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
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "btnLogout", Label = "Sair", OnClickFn = "fnLogout" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
            };

            if (ValidaToken())
                result.Content.AddRange(FormSimulacao());
            else
                result.Content.Add(FormLogin());

            return Content(JsonConvert.SerializeObject(result, JsonSerializerSetting.Front), "application/json"); ;
        }

        #region NotImplemented        
        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }
        public override Func<DomainBaseVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}