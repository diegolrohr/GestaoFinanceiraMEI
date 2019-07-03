using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Mensageria;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ValueObjects;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class ParametroTributarioBaseController<T> : BaseController<T> where T : ParametroTributarioVM
    {
        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            Dictionary<string, string> queryStringDefault = AppDefaults.GetQueryStringDefault();

            return queryStringDefault;
        }

        public ParametroTributarioVM GetParametro()
        {
            return RestHelper.ExecuteGetRequest<ParametroTributarioVM>(ResourceName);
        }

        public JsonResult CarregaParametro()
        {
            var parametroTributario = GetParametro();

            if (parametroTributario == null)
                return Json(new
                {
                    aliquotaSimplesNacional = "0",
                    aliquotaISS = "0",
                    aliquotaPISPASEP = "0",
                    aliquotaCOFINS = "0",
                    numeroRetornoNF = "1",
                    mensagemPadraoNota = "Nota Fiscal.",
                    tipoVersaoNFe = "v4",
                    tipoAmbiente = "Producao",
                    tipoModalidade = "Normal",
                    aliquotaFCP = "0",
                    tipoPresencaComprador = "Presencial",
                    horarioVerao = "Nao",
                    tipoHorario = "Brasilia",
                    versaoNFSe = "0.00",
                    usuarioWebServer = "",
                    senhaWebServer = "",
                    chaveAutenticacao = "",
                    autorizacao = "",
                    tipoTributacaoNFS = "DentroMunicipio",
                    tipoAmbienteNFS = "Producao",
                    aliquotaCSLL = "0",
                    aliquotaINSS = "0",
                    aliquotaImpostoRenda = "0",
                    incentivoCultura = false,
                    formatarCodigoISS = false,
                    tipoRegimeEspecialTributacao = "MicroEmpresaMunicipal",
                    tipoCRT = "SimplesNacional"
                }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                id = parametroTributario.Id,
                aliquotaSimplesNacional = parametroTributario.AliquotaSimplesNacional,
                aliquotaISS = parametroTributario.AliquotaISS,
                aliquotaPISPASEP = parametroTributario.AliquotaPISPASEP,
                aliquotaCOFINS = parametroTributario.AliquotaCOFINS,
                numeroRetornoNF = parametroTributario.NumeroRetornoNF,
                tipoModalidade = parametroTributario.TipoModalidade,
                tipoVersaoNFe = parametroTributario.TipoVersaoNFe,
                mensagemPadraoNota = parametroTributario.MensagemPadraoNota,
                tipoAmbiente = parametroTributario.TipoAmbiente,
                aliquotaFCP = parametroTributario.AliquotaFCP,
                tipoPresencaComprador = parametroTributario.TipoPresencaComprador,
                horarioVerao = parametroTributario.HorarioVerao,
                tipoHorario = parametroTributario.TipoHorario,
                versaoNFSe = parametroTributario.VersaoNFSe,
                usuarioWebServer = parametroTributario.UsuarioWebServer,
                senhaWebServer = parametroTributario.SenhaWebServer,
                chaveAutenticacao = parametroTributario.ChaveAutenticacao,
                autorizacao = parametroTributario.Autorizacao,
                tipoTributacaoNFS = parametroTributario.TipoTributacaoNFS,
                tipoAmbienteNFS = parametroTributario.TipoAmbienteNFS,
                aliquotaCSLL = parametroTributario.AliquotaCSLL,
                aliquotaINSS = parametroTributario.AliquotaINSS,
                aliquotaImpostoRenda = parametroTributario.AliquotaImpostoRenda,
                incentivoCultura = parametroTributario.IncentivoCultura,
                formatarCodigoISS = parametroTributario.FormatarCodigoISS,
                tipoRegimeEspecialTributacao = parametroTributario.TipoRegimeEspecialTributacao,
                tipoCRT = parametroTributario.TipoCRT,
            }, JsonRequestBehavior.AllowGet);
        }

        public override Func<T, object> GetDisplayData() { throw new NotImplementedException(); }

        public override ContentResult List()
            => Form();

        public ContentResult ModalEnvioEmail()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Enviar por e-mail para seu Contador",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Enviar", OnClickFn = "fnFormClickEnvioEmailContador" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                Id = "fly01mdlfrmEnvioEmailContador"
            };
            config.Elements.Add(new InputHiddenUI { Id = "parametroTributarioId" });
            config.Elements.Add(new InputEmailUI
            {
                Id = "emailId",
                Class = "col s12",
                Label = "E-mail do seu Contador"
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnAtualizaParametro", Type = "submit", Position = HtmlUIButtonPosition.Main });
                target.Add(new HtmlUIButton { Id = "envioEmail", Label = "Envie para seu contador", OnClickFn = "fnEnviarParametrosEmail", Type = "click", Position = HtmlUIButtonPosition.Out });
                target.Add(new HtmlUIButton { Id = "atualizaAliquota", Label = "Atualizar alíquotas", OnClickFn = "fnAtualizarAliquotas", Type = "click" });
            }
            return target;
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public override ContentResult Form() => base.Form();

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index"),
                },
                Header = new HtmlUIHeader
                {
                    Title = "Parâmetros Tributários | Nota Fiscal",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var tabs = new TabsUI
            {
                Id = "fly01tabs",
                Tabs = new List<TabsUIItem>()
                {
                    new TabsUIItem()
                    {
                        Id = "fly01frm",
                        Title = "Alíquotas Padrões"
                    },
                    new TabsUIItem()
                    {
                        Id = "fly01frm2",
                        Title = "Parâmetros de Transmissão NF-e"
                    },
                    new TabsUIItem()
                    {
                        Id = "fly01frm3",
                        Title = "Parâmetros de Transmissão NFS-e"
                    }
                }
            };

            var form1 = new FormUI
            {
                Class = "col s12",
                Elements = new List<BaseUI>(),
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            form1.Elements.Add(new InputHiddenUI { Id = "id" });

            form1.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaSimplesNacional",
                Class = "col s12 m3",
                Label = "ICMS",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form1.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaFCP",
                Class = "col s12 m3",
                Label = "Fundo de Combate à Pobreza",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form1.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaPISPASEP",
                Class = "col s12 m3",
                Label = "PIS/PASEP",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            }); ;

            form1.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaCOFINS",
                Class = "col s12 m3",
                Label = "COFINS",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form1.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaISS",
                Class = "col s12 m3",
                Label = "Imposto Sobre Serviço",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form1.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaCSLL",
                Class = "col s12 m3",
                Label = "CSLL",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form1.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaINSS",
                Class = "col s12 m3",
                Label = "INSS",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form1.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaImpostoRenda",
                Class = "col s12 m3",
                Label = "Imposto de Renda",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            var form2 = new FormUI
            {
                Class = "col s12",
                Elements = new List<BaseUI>(),
                Id = "fly01frm2"
            };

            form2.Elements.Add(new InputHiddenUI { Id = "numeroRetornoNF" });

            form2.Elements.Add(new SelectUI
            {
                Id = "tipoPresencaComprador",
                Class = "col s12 m6",
                Label = "Presença do Comprador",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoPresencaComprador)))
            });

            form2.Elements.Add(new SelectUI
            {
                Id = "tipoModalidade",
                Class = "col s12 m6",
                Label = "Modalidade",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoModalidade)))
            });

            form2.Elements.Add(new SelectUI
            {
                Id = "horarioVerao",
                Class = "col s12 m6 l3",
                Label = "Horário de Verão",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(HorarioVerao)))
            });

            form2.Elements.Add(new SelectUI
            {
                Id = "tipoHorario",
                Class = "col s12 m6 l3",
                Label = "Tipo Horário TSS",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoHorarioTSS)))
            });

            form2.Elements.Add(new SelectUI
            {
                Id = "tipoVersaoNFe",
                Class = "col s6 m6 l3",
                Label = "Versão NF-e ",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoVersaoNFe)))
            });

            form2.Elements.Add(new SelectUI
            {
                Id = "tipoAmbiente",
                Class = "col s6 m6 l3",
                Label = "Ambiente NF-e",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoAmbiente))
                .ToList().FindAll(x => "Producao,Homologacao".Contains(x.Value)))
            });

            form2.Elements.Add(new SelectUI
            {
                Id = "tipoCRT",
                Class = "col s6 m6 l3",
                Label = "CRT",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCRT)))
                //.ToList().FindAll(x => "SimplesNacional,ExcessoSublimiteDeReceitaBruta".Contains(x.Value)))               
            });

            form2.Elements.Add(new TextAreaUI { Id = "mensagemPadraoNota", Class = "col s12", Label = "Informações Adicionais", MaxLength = 1000 });

            #region NFS
            var form3 = new FormUI
            {
                Class = "col s12",
                Elements = new List<BaseUI>(),
                Id = "fly01frm3"
            };

            form3.Elements.Add(new InputCheckboxUI { Id = "incentivoCultura", Class = "col s12 m4", Label = "É Incentivador à Cultura" });
            form3.Elements.Add(new InputCheckboxUI { Id = "formatarCodigoISS", Class = "col s12 m4", Label = "Formatar Código ISS" });

            form3.Elements.Add(new InputCustommaskUI
            {
                Id = "versaoNFSe",
                Class = "col s12 m4",
                Label = "Versão NFS-e",
                MaxLength = 4,
                Value = "0.00"
            });

            form3.Elements.Add(new SelectUI
            {
                Id = "tipoTributacaoNFS",
                Class = "col s12 m4",
                Label = "Tipo Tributação NFS-e",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoTributacaoNFS)))
            });

            form3.Elements.Add(new SelectUI
            {
                Id = "tipoAmbienteNFS",
                Class = "col s12 m4",
                Label = "Ambiente NFS-e",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoAmbiente))
                .ToList().FindAll(x => "Producao,Homologacao".Contains(x.Value))
                )
            });

            form3.Elements.Add(new SelectUI
            {
                Id = "tipoRegimeEspecialTributacao",
                Class = "col s12 m4",
                Label = "Regime Especial Tributação",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoRegimeEspecialTributacao)))
            });

            form3.Elements.Add(new InputTextUI { Id = "usuarioWebServer", Class = "col s12 m3", Label = "Usuário Web Server", MaxLength = 200 });

            form3.Elements.Add(new InputPasswordUI { Id = "senhaWebServer", Class = "col s12 m3", Label = "Senha Web Server", MaxLength = 200 });

            form3.Elements.Add(new InputTextUI { Id = "chaveAutenticacao", Class = "col s12 m3", Label = "Chave de Autenticação", MaxLength = 200 });

            form3.Elements.Add(new InputTextUI { Id = "autorizacao", Class = "col s12 m3", Label = "Autorização", MaxLength = 200 });

            #endregion

            #region Helpers
            form3.Helpers.Add(new TooltipUI
            {
                Id = "versaoNFSe",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Conforme versão da lista de municípios homologados pelo TSS. Disponível no TDN ou em nossos manuais. http://tdn.totvs.com/pages/viewpage.action?pageId=239027743"
                }
            });
            form3.Helpers.Add(new TooltipUI
            {
                Id = "formatarCodigoISS",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Formatar o Código Iss da tabela padrão nos serviços com pontuação ao gerar o XML, depende da configuração esperada pela prefeitura do município. Ex: 104 = 1.04, 2502 = 25.02"
                }
            });
            form3.Helpers.Add(new TooltipUI
            {
                Id = "usuarioWebServer",
                Tooltip = new HelperUITooltip()
                {
                    Text = "De acordo com a configuração especificada pela prefeitura. Verifique, se a sua prefeitura possui ambiente de homologação para NFS"
                }
            });
            #endregion

            cfg.Content.Add(tabs);
            cfg.Content.Add(form1);
            cfg.Content.Add(form2);
            cfg.Content.Add(form3);

            return cfg;
        }

        public JsonResult EnviaEmailContador(string simplesNacional, string impostoRenda, string csll, string cofins, string pisPasep, string iss, string email, string fcp, string inss)
        {
            try
            {
                var empresa = GetDadosEmpresa();

                var ResponseError = ValidarDadosEmail(empresa, email);
                if (ResponseError != null) return ResponseError;

                MailSend(empresa, simplesNacional, impostoRenda, csll, cofins, pisPasep, iss, email, fcp, inss);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        private JsonResult ValidarDadosEmail(ManagerEmpresaVM empresa, string email)
        {
            const string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            if (string.IsNullOrEmpty(email)) return JsonResponseStatus.GetFailure("E-mail do seu contador inválido.");
            if (!Regex.IsMatch(email ?? "", pattern)) return JsonResponseStatus.GetFailure("E-mail do seu contador inválido.");
            if (string.IsNullOrEmpty(empresa.Email)) return JsonResponseStatus.GetFailure("Você ainda não configurou um email válido para sua empresa.");

            return null;
        }

        private void MailSend(ManagerEmpresaVM empresa, string simplesNacional, string impostoRenda, string csll, string cofins, string pisPasep, string iss, string email, string fcp, string inss)
        {
            var mensagemPrincipal = $"Razão Social: {empresa.RazaoSocial}".ToUpper();
            var tituloEmail = $"Este e-mail é referente aos impostos da empresa: {empresa.NomeFantasia}".ToUpper();
            var mensagemComplemento = $"CNPJ: {empresa.CNPJ}".ToUpper();
            var conteudoEmail = Mail.FormataMensagem(EmailFilesHelper.GetTemplate("Templates.ParametroTributario.html").Value, tituloEmail, mensagemPrincipal, mensagemComplemento, empresa.Email, simplesNacional, impostoRenda, csll, cofins, pisPasep, iss, fcp, inss);

            Mail.SendNoAttachment(empresa.NomeFantasia, email, tituloEmail, conteudoEmail);
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public JsonResult ImportaParametro(string id, string mensagem, double simplesNacional, double fcp, double iss, double pispasep, double cofins,
            string numeroRetorno, string modalidade, string versao, string ambiente, string tipoPresencaComprador, string horarioVerao,
            string tipoHorario, string versaoNFSe, string usuarioWebServer, string senhaWebServer, string chaveAutenticacao, string autorizacao,
              string tipoTributacaoNFS, string tipoAmbienteNFS, double csll, double inss, double impostoRenda, bool incentivoCultura, bool formatarCodigoISS, string tipoRegimeEspecialTributacao, string tipoCRT)
        {
            try
            {
                var dadosParametro = new
                {
                    id = string.IsNullOrEmpty(id) ? Guid.NewGuid() : Guid.Parse(id),
                    simplesNacional = "True",
                    aliquotaSimplesNacional = double.IsNaN(simplesNacional) ? 0 : simplesNacional,
                    aliquotaFCP = fcp,
                    aliquotaISS = double.IsNaN(iss) ? 0 : iss,
                    aliquotaPISPASEP = double.IsNaN(pispasep) ? 0 : pispasep,
                    aliquotaCOFINS = double.IsNaN(cofins) ? 0 : cofins,
                    numeroRetornoNF = numeroRetorno,
                    tipoModalidade = modalidade,
                    tipoVersaoNFe = versao,
                    mensagemPadraoNota = mensagem,
                    tipoAmbiente = ambiente,
                    tipoPresencaComprador = tipoPresencaComprador,
                    horarioVerao = horarioVerao,
                    tipoHorario = tipoHorario,
                    versaoNFSe = versaoNFSe,
                    usuarioWebServer = usuarioWebServer,
                    senhaWebServer = Base64Helper.CodificaBase64(senhaWebServer),
                    chaveAutenticacao = chaveAutenticacao,
                    autorizacao = autorizacao,
                    tipoTributacaoNFS = tipoTributacaoNFS,
                    tipoAmbienteNFS = tipoAmbienteNFS,
                    aliquotaCSLL = double.IsNaN(csll) ? 0 : csll,
                    aliquotaINSS = double.IsNaN(inss) ? 0 : inss,
                    aliquotaImpostoRenda = double.IsNaN(impostoRenda) ? 0 : impostoRenda,
                    incentivoCultura = incentivoCultura,
                    formatarCodigoISS = formatarCodigoISS,
                    tipoRegimeEspecialTributacao = tipoRegimeEspecialTributacao,
                    tipoCRT = tipoCRT
                };

                if (dadosParametro.mensagemPadraoNota.Length > 4000)
                    return JsonResponseStatus.GetFailure("Número de caracteres na Mensagem Padrão na Nota não pode ser maior que 4000 caracteres.");

                if (dadosParametro.numeroRetornoNF.Length > 20)
                    return JsonResponseStatus.GetFailure("Número de caracteres no Número de Retorno da Nota Fiscal não pode ser maior que 20 caracteres.");

                ParametroTributarioVM parametroRetorno;

                if (string.IsNullOrEmpty(id))
                {
                    parametroRetorno = RestHelper.ExecutePostRequest<ParametroTributarioVM>(ResourceName, JsonConvert.SerializeObject(dadosParametro, JsonSerializerSetting.Default));
                    id = parametroRetorno?.Id.ToString();
                }
                else
                    parametroRetorno = RestHelper.ExecutePutRequest<ParametroTributarioVM>($"{ResourceName}/{id}", JsonConvert.SerializeObject(dadosParametro, JsonSerializerSetting.Edit));

                return Json(new
                {
                    success = true,
                    id = id,
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
                    Text = "Verificamos que você não possui cadastrado sua Inscrição Estadual nos dados de sua Empresa. Por favor, insira sua inscrição estadual para realizarmos a atualização dos seus parâmetros tributários."
                }
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public JsonResult ValidaDadosEmpresa()
        {
            try
            {
                ManagerEmpresaVM empresa = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);
                if (!string.IsNullOrWhiteSpace(empresa.InscricaoEstadual))
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

        public JsonResult PostAtualizacaoIE(string inscricaoEstadual)
        {
            try
            {
                ManagerEmpresaVM empresa = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);

                var msgErrorInscricaoEstadual = string.Empty;
                if (InscricaoEstadualHelper.IsValid(empresa.Cidade?.Estado?.Sigla, inscricaoEstadual, out msgErrorInscricaoEstadual))
                {
                    empresa.InscricaoEstadual = inscricaoEstadual;
                    ApiEmpresaManager.AtualizaDadosEmpresa(empresa, SessionManager.Current.UserData.PlatformUrl);

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

        [HttpGet]
        public JsonResult ExisteParametroSalvo()
        {
            var queryString = new Dictionary<string, string> {
                    { "$select", "id" }
            };

            var response = RestHelper.ExecuteGetRequest<ParametroTributarioVM>(ResourceName, queryString);
            return Json(new { existeParametro = (response?.Id != null && response?.Id != default(Guid)) }, JsonRequestBehavior.AllowGet);
        }
    }
}