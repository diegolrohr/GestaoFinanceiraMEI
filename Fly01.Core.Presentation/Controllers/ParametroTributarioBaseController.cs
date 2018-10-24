using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using System.Linq;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.uiJS.Classes.Helpers;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;

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
            var response = RestHelper.ExecuteGetRequest<ResultBase<ParametroTributarioVM>>(ResourceName);

            if (response == null || response.Data == null)
                return null;

            return response.Data.FirstOrDefault();
        }

        public JsonResult CarregaParametro()
        {
            var parametroTributario = GetParametro();

            if (parametroTributario == null)
                return Json(new
                {
                    id = default(Guid).ToString(),
                    aliquotaSimplesNacional = "0",
                    aliquotaISS = "5",
                    aliquotaPISPASEP = "0,65",
                    aliquotaCOFINS = "2",
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
                    formatarCodigoISS = false
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
                formatarCodigoISS = parametroTributario.FormatarCodigoISS
            }, JsonRequestBehavior.AllowGet);
        }

        public override Func<T, object> GetDisplayData() { throw new NotImplementedException(); }

        public override ContentResult List() 
            => Form();

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnAtualizaParametro", Type = "submit" });

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

            var form1 = new FormUI
            {
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

            cfg.Content.Add(form1);

            var form2 = new FormUI
            {
                Class = "col s12",
                Elements = new List<BaseUI>
                {
                    new LabelSetUI { Id =  "labelSetAliquotasPadroes", Class = "col s12", Label = "Alíquotas Padrões"}
                },
                Id = "fly01frm2"
            };

            form2.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaSimplesNacional",
                Class = "col s12 m3",
                Label = "ICMS Simples Nacional",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form2.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaPISPASEP",
                Class = "col s12 m3",
                Label = "PIS/PASEP",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            }); ;

            form2.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaCOFINS",
                Class = "col s12 m3",
                Label = "COFINS",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form2.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaFCP",
                Class = "col s12 m3",
                Label = "Fundo de Combate à Pobreza",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form2.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaISS",
                Class = "col s12 m3",
                Label = "Imposto Sobre Serviço",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form2.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaCSLL",
                Class = "col s12 m3",
                Label = "CSLL",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form2.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaINSS",
                Class = "col s12 m3",
                Label = "INSS",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form2.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaImpostoRenda",
                Class = "col s12 m3",
                Label = "Imposto de Renda",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            cfg.Content.Add(form2);

            var form3 = new FormUI
            {
                Class = "col s12",
                Elements = new List<BaseUI>
                {
                    new LabelSetUI { Id =  "labelSetParametrosNFe", Class = "col s12", Label = "Parâmetros de Transmissão NF-e"}
                },
                Id = "fly01frm3"

            };

            form3.Elements.Add(new InputHiddenUI { Id = "numeroRetornoNF" });

            form3.Elements.Add(new SelectUI
            {
                Id = "tipoPresencaComprador",
                Class = "col s12 m6",
                Label = "Presença do Comprador",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoPresencaComprador)))
            });

            form3.Elements.Add(new SelectUI
            {
                Id = "tipoModalidade",
                Class = "col s12 m6",
                Label = "Modalidade",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoModalidade)))
            });

            form3.Elements.Add(new SelectUI
            {
                Id = "horarioVerao",
                Class = "col s12 m6 l3",
                Label = "Horário de Verão",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(HorarioVerao)))
            });

            form3.Elements.Add(new SelectUI
            {
                Id = "tipoHorario",
                Class = "col s12 m6 l3",
                Label = "Tipo Horário TSS",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoHorarioTSS)))
            });

            form3.Elements.Add(new SelectUI
            {
                Id = "tipoVersaoNFe",
                Class = "col s6 m6 l3",
                Label = "Versão NF-e ",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoVersaoNFe)))
            });

            form3.Elements.Add(new SelectUI
            {
                Id = "tipoAmbiente",
                Class = "col s6 m6 l3",
                Label = "Ambiente NF-e",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoAmbiente)))
            });

            form3.Elements.Add(new TextAreaUI { Id = "mensagemPadraoNota", Class = "col s12", Label = "Informações Adicionais", MaxLength = 1000 });

            #region NFS

            form3.Elements.Add(new LabelSetUI { Id = "labelSetParametrosNFSe", Class = "col s12", Label = "Parâmetros de Transmissão NFS-e" });

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

            form3.Elements.Add(new InputTextUI { Id = "usuarioWebServer", Class = "col s12 m4", Label = "Usuário Web Server", MaxLength = 200 });

            form3.Elements.Add(new InputPasswordUI { Id = "senhaWebServer", Class = "col s12 m4", Label = "Senha Web Server", MaxLength = 200 });

            form3.Elements.Add(new InputTextUI { Id = "chaveAutenticacao", Class = "col s12 m4", Label = "Chave de Autenticação", MaxLength = 200 });

            form3.Elements.Add(new InputTextUI { Id = "autorizacao", Class = "col s12 m4", Label = "Autorização", MaxLength = 200 });

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
            cfg.Content.Add(form3);

            return cfg;
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public JsonResult ImportaParametro(string mensagem, double simplesNacional, double fcp, double iss, double pispasep, double cofins,
            string numeroRetorno, string modalidade, string versao, string ambiente, string tipoPresencaComprador, string horarioVerao,
            string tipoHorario, string versaoNFSe, string usuarioWebServer, string senhaWebServer, string chaveAutenticacao, string autorizacao,
              string  tipoTributacaoNFS, string tipoAmbienteNFS, double csll, double inss, double impostoRenda, bool incentivoCultura, bool formatarCodigoISS)
        {
            try
            {
                var dadosParametro = new
                {
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
                    formatarCodigoISS = formatarCodigoISS
                };

                if (dadosParametro.mensagemPadraoNota.Length > 4000)
                    return JsonResponseStatus.GetFailure("Número de caracteres na Mensagem Padrão na Nota não pode ser maior que 4000 caracteres.");

                if (dadosParametro.numeroRetornoNF.Length > 20)
                    return JsonResponseStatus.GetFailure("Número de caracteres no Número de Retorno da Nota Fiscal não pode ser maior que 20 caracteres.");

                ParametroTributarioVM parametroRetorno;

                var existeParametro = GetParametro();

                if (existeParametro == null)
                    parametroRetorno = RestHelper.ExecutePostRequest<ParametroTributarioVM>(ResourceName, JsonConvert.SerializeObject(dadosParametro, JsonSerializerSetting.Default));
                else
                    parametroRetorno = RestHelper.ExecutePutRequest<ParametroTributarioVM>($"{ResourceName}/{existeParametro.Id}", JsonConvert.SerializeObject(dadosParametro, JsonSerializerSetting.Edit));

                return Json(new
                {
                    success = true,
                    data = parametroRetorno,
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