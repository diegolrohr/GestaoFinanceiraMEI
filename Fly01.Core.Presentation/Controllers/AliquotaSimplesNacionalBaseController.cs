using Fly01.Core.Defaults;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    //$$$.modal("/AliquotaSimplesNacional/FormModal?isOnCadastroParametros=false")
    public class AliquotaSimplesNacionalBaseController<T> : BaseController<T> where T : AliquotaSimplesNacionalVM
    {
        public override Func<T, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        private JsonResult ValidarDadosEmail(string email)
        {
            const string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            if (string.IsNullOrEmpty(email)) return JsonResponseStatus.GetFailure("E-mail do seu contador inválido.");
            if (!Regex.IsMatch(email ?? "", pattern)) return JsonResponseStatus.GetFailure("E-mail do seu contador inválido.");

            return null;
        }

        [HttpPost]
        public override JsonResult Create(T entityVM)
        {
            try
            {
                if (entityVM.EnviarEmailContador)
                {
                    var ResponseError = ValidarDadosEmail(entityVM?.EmailContador);
                    if (ResponseError != null) return ResponseError;
                }

                var parametro = new ParametroTributarioVM()
                {
                    AliquotaSimplesNacional = entityVM.SimplesNacional,
                    AliquotaISS = entityVM.Iss,
                    AliquotaPISPASEP = entityVM.PisPasep,
                    AliquotaCOFINS = entityVM.Cofins,
                    NumeroRetornoNF = "1",
                    MensagemPadraoNota = "",
                    TipoVersaoNFe = "v4",
                    TipoAmbiente = "Producao",
                    TipoModalidade = "Normal",
                    AliquotaFCP = 0.0,
                    TipoPresencaComprador = "Presencial",
                    HorarioVerao = "Nao",
                    TipoHorario = "Brasilia",
                    VersaoNFSe = "0.00",
                    UsuarioWebServer = "",
                    SenhaWebServer = "",
                    ChaveAutenticacao = "",
                    Autorizacao = "",
                    TipoTributacaoNFS = "DentroMunicipio",
                    TipoAmbienteNFS = "Producao",
                    AliquotaCSLL = entityVM.Csll,
                    AliquotaINSS = 0.0,
                    AliquotaImpostoRenda = entityVM.ImpostoRenda,
                    IncentivoCultura = false,
                    FormatarCodigoISS = false,
                    TipoRegimeEspecialTributacao = "MicroEmpresaMunicipal"
                };

                var postResponse = RestHelper.ExecutePostRequest("ParametroTributario", JsonConvert.SerializeObject(parametro, JsonSerializerSetting.Default));
                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public ContentResult FormModal(bool isOnCadastroParametros = false)
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Alíquotas Simples Nacional",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                ConfirmAction = new ModalUIAction() { Label = isOnCadastroParametros ? "Aplicar" : "Salvar" },
                Action = new FormUIAction
                {
                    Create = Url.Action("Create")
                },
                ReadyFn = "fnFormReadyAliquotaSimplesNacional",
                Id = "fly01mdlfrmAliquotaSimplesNacional"
            };

            config.Elements.Add(new InputHiddenUI { Id = "isOnCadastroParametros", Value = isOnCadastroParametros.ToString() });
            config.Elements.Add(new DivElementUI { Id = "infoAliquotas", Class = "col s12 text-justify visible", Label = "Informação" });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoFaixaReceitaBruta",
                Class = "col s12",
                Label = "Faixa Receita Bruta",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoFaixaReceitaBruta)).ToList()),
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI() { DomEvent = "change", Function = "fnChangeTipoFaixaReceitaBruta" }
                }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "tipoEnquadramentoEmpresa",
                Class = "col s12",
                Required = true,
                Label = "Enquadramento Empresa",
                DataUrl = Url.Action("AliquotaSimplesNacional", "AutoComplete"),
                LabelId = "tipoEnquadramentoEmpresaDescricao",
                PreFilter = "tipoFaixaReceitaBruta",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI() { DomEvent = "autocompleteselect", Function = "fnChangeTipoEnquadramentoEmpresa" }
                }
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "simplesNacional",
                Class = "col s12 m4",
                Label = "ICMS Simples Nacional",
                Disabled = true,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "impostoRenda",
                Class = "col s12 m4",
                Label = "Imposto de Renda",
                Disabled = true,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "csll",
                Class = "col s12 m4",
                Label = "CSLL",
                Disabled = true,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "cofins",
                Class = "col s12 m4",
                Label = "COFINS",
                Disabled = true,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "pisPasep",
                Class = "col s12 m4",
                Label = "PIS/PASEP",
                Disabled = true,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "iss",
                Class = "col s12 m4",
                Label = "Imposto Sobre Serviço",
                Disabled = true,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            if (!isOnCadastroParametros)
            {
                config.Elements.Add(new InputCheckboxUI
                {
                    Id = "enviarEmailContador",
                    Class = "col s12 m4",
                    Label = "Enviar e-mail para contador",
                    DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChkEnviarEmailContador" }
                }
                });
                config.Elements.Add(new InputEmailUI { Id = "emailContador", Class = "col s12 m4", Label = "E-mail do Contador", MaxLength = 100, });

                config.Helpers.Add(new TooltipUI
                {
                    Id = "enviarEmailContador",
                    Tooltip = new HelperUITooltip()
                    {
                        Text = "Se marcar esta opção, ao salvar as alíquotas no seu cadastro de parâmetros tributários, também será enviado ao e-mail informado uma cópia das alíquotas configuradas, para você solicitar a conferência e confirmação junto ao seu contador, para evitar problemas fiscais."
                    }
                });
            }

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
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
                Class = "col s12",
                Label = "Inscrição Estadual"
            });

            config.Helpers.Add(new TooltipUI
            {
                Id = "inscricaoEstadualId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Verificamos que você não possui cadastrado sua Inscrição Estadual nos dados de sua Empresa. Por favor, insira sua inscrição estadual para realizarmos a atualização dos seus parâmetros tributários."
                }
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }


        public ContentResult FormModalWizard(bool isOnCadastroParametros = false, bool isEdit = false)
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Create"),
                    WithParams = Url.Action("Edit")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Informações Fiscais",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                //ReadyFn = "fnFormReadyAliquotaSimplesNacional",
                //Functions = new List<string> { "fnFormReadyOrcamentoItem" }
            };
            var config = new FormWizardUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List", "OrdemCompra")
                },
                ReadyFn = "fnFormReadyAliquotaSimplesNacional",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Steps = new List<FormWizardUIStep>()
                            {
                                new FormWizardUIStep()
                                {
                                    Title = "Entenda",
                                    Id = "stepEntenda",
                                    Quantity = 2,
                                },
                                new FormWizardUIStep()
                                {
                                    Title = "Informe",
                                    Id = "stepInforme",
                                    Quantity = 1,
                                },
                                new FormWizardUIStep()
                                {
                                    Title = "Selecione",
                                    Id = "stepSelecione",
                                    Quantity = 1,

                                },
                                new FormWizardUIStep()
                                {
                                    Title = "Finalize",
                                    Id = "stepFinalize",
                                    Quantity = 9,
                                },
                        },
                Rule = isEdit ? "parallel" : "linear",
                ShowStepNumbers = true
            };



            config.Elements.Add(new InputHiddenUI { Id = "isOnCadastroParametros", Value = isOnCadastroParametros.ToString() });
            config.Elements.Add(new DivElementUI { Id = "infoAliquotas", Class = "col s12 text-justify visible", Label = "Informação" });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoFaixaReceitaBruta",
                Class = "col s12",
                Label = "Faixa Receita Bruta",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoFaixaReceitaBruta)).ToList()),
                DomEvents = new List<DomEventUI>
                    {
                        new DomEventUI() { DomEvent = "change", Function = "fnChangeTipoFaixaReceitaBruta" }
                    }
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "tipoEnquadramentoEmpresa",
                Class = "col s12",
                Required = true,
                Label = "Enquadramento Empresa",
                DataUrl = Url.Action("AliquotaSimplesNacional", "AutoComplete"),
                LabelId = "tipoEnquadramentoEmpresaDescricao",
                PreFilter = "tipoFaixaReceitaBruta",
                DomEvents = new List<DomEventUI>
                    {
                        new DomEventUI() { DomEvent = "autocompleteselect", Function = "fnChangeTipoEnquadramentoEmpresa" }
                    }
            });

            config.Elements.Add(new DivElementUI { Id = "infoFinal", Class = "col s12 text-justify visible", Label = "Informação" });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "simplesNacional",
                Class = "col s12 m4",
                Label = "ICMS Simples Nacional",
                Disabled = true,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "impostoRenda",
                Class = "col s12 m4",
                Label = "Imposto de Renda",
                Disabled = true,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "csll",
                Class = "col s12 m4",
                Label = "CSLL",
                Disabled = true,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "cofins",
                Class = "col s12 m4",
                Label = "COFINS",
                Disabled = true,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "pisPasep",
                Class = "col s12 m4",
                Label = "PIS/PASEP",
                Disabled = true,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "iss",
                Class = "col s12 m4",
                Label = "Imposto Sobre Serviço",
                Disabled = true,
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            if (!isOnCadastroParametros)
            {
                config.Elements.Add(new InputCheckboxUI
                {
                    Id = "enviarEmailContador",
                    Class = "col s12 m4",
                    Label = "Enviar e-mail para contador",
                    DomEvents = new List<DomEventUI>
                    {
                        new DomEventUI { DomEvent = "change", Function = "fnChkEnviarEmailContador" }
                    }
                });
                config.Elements.Add(new InputEmailUI { Id = "emailContador", Class = "col s12 m4", Label = "E-mail do Contador", MaxLength = 100, });

                config.Helpers.Add(new TooltipUI
                {
                    Id = "enviarEmailContador",
                    Tooltip = new HelperUITooltip()
                    {
                        Text = "Se marcar esta opção, ao salvar as alíquotas no seu cadastro de parâmetros tributários, também será enviado ao e-mail informado uma cópia das alíquotas configuradas, para você solicitar a conferência e confirmação junto ao seu contador, para evitar problemas fiscais."
                    }
                });

            }
            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, uiJS.Defaults.JsonSerializerSetting.Front), "application/json");
        }

        //public ContentResult FormModalWizard(bool isOnCadastroParametros = false, bool isEdit = false)
        //{
        //    ModalUIFormWizard config = new ModalUIFormWizard()
        //    {
        //        Title = "Informações Fiscais",
        //        UrlFunctions = @Url.Action("Functions") + "?fns=",
        //        CancelAction = new ModalUIAction() { Label = "Cancelar" },
        //        Action = new FormUIAction
        //        {
        //            Create = Url.Action("Create"),
        //            Edit = @Url.Action("Edit"),
        //            Get = @Url.Action("Json") + "/",
        //            List = @Url.Action("List", "OrdemVenda")
        //        },
        //        //ReadyFn = "fnFormReadyAliquotaSimplesNacional",
        //        Id = "fly01mdlfrmAliquotaSimplesNacional",
        //        Steps = new List<FormWizardUIStep>()
        //            {
        //                new FormWizardUIStep()
        //                {
        //                    Title = "Entenda",
        //                    Id = "stepEntenda",
        //                    Quantity = 2,
        //                },
        //                new FormWizardUIStep()
        //                {
        //                    Title = "Informe",
        //                    Id = "stepInforme",
        //                    Quantity = 3,
        //                },
        //                new FormWizardUIStep()
        //                {
        //                    Title = "Selecione",
        //                    Id = "stepSelecione",
        //                    Quantity = 1,

        //                },
        //                new FormWizardUIStep()
        //                {
        //                    Title = "Finalize",
        //                    Id = "stepFinalize",
        //                    Quantity = 8,
        //                },
        //        },
        //        Rule = "linear",
        //        ShowStepNumbers = true,

        //    };

        //    config.Elements.Add(new InputHiddenUI { Id = "isOnCadastroParametros", Value = isOnCadastroParametros.ToString() });
        //    config.Elements.Add(new DivElementUI { Id = "infoAliquotas", Class = "col s12 text-justify visible", Label = "Informação" });
        //    config.Elements.Add(new SelectUI
        //    {
        //        Id = "tipoFaixaReceitaBruta",
        //        Class = "col s12",
        //        Label = "Faixa Receita Bruta",
        //        Required = true,
        //        Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoFaixaReceitaBruta)).ToList()),
        //        DomEvents = new List<DomEventUI>
        //        {
        //            new DomEventUI() { DomEvent = "change", Function = "fnChangeTipoFaixaReceitaBruta" }
        //        }
        //    });

        //    config.Elements.Add(new AutoCompleteUI
        //    {
        //        Id = "tipoEnquadramentoEmpresa",
        //        Class = "col s12",
        //        Required = true,
        //        Label = "Enquadramento Empresa",
        //        DataUrl = Url.Action("AliquotaSimplesNacional", "AutoComplete"),
        //        LabelId = "tipoEnquadramentoEmpresaDescricao",
        //        PreFilter = "tipoFaixaReceitaBruta",
        //        DomEvents = new List<DomEventUI>
        //        {
        //            new DomEventUI() { DomEvent = "autocompleteselect", Function = "fnChangeTipoEnquadramentoEmpresa" }
        //        }
        //    });


        //    config.Elements.Add(new InputCustommaskUI
        //    {
        //        Id = "simplesNacional",
        //        Class = "col s12 m4",
        //        Label = "ICMS Simples Nacional",
        //        Disabled = true,
        //        Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
        //    });
        //    config.Elements.Add(new InputCustommaskUI
        //    {
        //        Id = "impostoRenda",
        //        Class = "col s12 m4",
        //        Label = "Imposto de Renda",
        //        Disabled = true,
        //        Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
        //    });
        //    config.Elements.Add(new InputCustommaskUI
        //    {
        //        Id = "csll",
        //        Class = "col s12 m4",
        //        Label = "CSLL",
        //        Disabled = true,
        //        Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
        //    });
        //    config.Elements.Add(new InputCustommaskUI
        //    {
        //        Id = "cofins",
        //        Class = "col s12 m4",
        //        Label = "COFINS",
        //        Disabled = true,
        //        Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
        //    });
        //    config.Elements.Add(new InputCustommaskUI
        //    {
        //        Id = "pisPasep",
        //        Class = "col s12 m4",
        //        Label = "PIS/PASEP",
        //        Disabled = true,
        //        Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
        //    });
        //    config.Elements.Add(new InputCustommaskUI
        //    {
        //        Id = "iss",
        //        Class = "col s12 m4",
        //        Label = "Imposto Sobre Serviço",
        //        Disabled = true,
        //        Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
        //    });

        //    if (!isOnCadastroParametros)
        //    {
        //        config.Elements.Add(new InputCheckboxUI
        //        {
        //            Id = "enviarEmailContador",
        //            Class = "col s12 m4",
        //            Label = "Enviar e-mail para contador",
        //            DomEvents = new List<DomEventUI>
        //        {
        //            new DomEventUI { DomEvent = "change", Function = "fnChkEnviarEmailContador" }
        //        }
        //        });
        //        config.Elements.Add(new InputEmailUI { Id = "emailContador", Class = "col s12 m4", Label = "E-mail do Contador", MaxLength = 100, });

        //        config.Helpers.Add(new TooltipUI
        //        {
        //            Id = "enviarEmailContador",
        //            Tooltip = new HelperUITooltip()
        //            {
        //                Text = "Se marcar esta opção, ao salvar as alíquotas no seu cadastro de parâmetros tributários, também será enviado ao e-mail informado uma cópia das alíquotas configuradas, para você solicitar a conferência e confirmação junto ao seu contador, para evitar problemas fiscais."
        //            }
        //        });
        //    }
        //    return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");

        //}

    }
}