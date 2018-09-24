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
                    tipoTributacaoNFS = "recolheIss",
                    tipoAmbienteNFS = "Producao"
                }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
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
                tipoAmbienteNFS = parametroTributario.TipoAmbienteNFS
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
                Id = "aliquotaFCP",
                Class = "col s12 m3",
                Label = "Fundo de Combate à Pobreza",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form2.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaISS",
                Class = "col s12 m2",
                Label = "Imposto Sobre Serviço",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            });

            form2.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaPISPASEP",
                Class = "col s12 m2",
                Label = "PIS/PASEP",
                Data = new { inputmask = "'mask': '9{1,3}[,9{1,2}] %', 'alias': 'decimal', 'autoUnmask': true, 'suffix': ' %', 'radixPoint': ',' " }
            }); ;

            form2.Elements.Add(new InputCustommaskUI
            {
                Id = "aliquotaCOFINS",
                Class = "col s12 m2",
                Label = "COFINS",
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
                Label = "Ambiente",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoAmbiente)))
            });

            form3.Elements.Add(new TextAreaUI { Id = "mensagemPadraoNota", Class = "col s12", Label = "Informações Adicionais", MaxLength = 1000 });

            #region NFS

            form3.Elements.Add(new LabelSetUI { Id = "labelSetParametrosNFSe", Class = "col s12", Label = "Parâmetros de Transmissão NFS-e" });

            form3.Elements.Add(new InputCheckboxUI { Id = "incentivoCultura", Class = "col s12 m4", Label = "É Incentivador à Cultura" });

            form3.Elements.Add(new InputCustommaskUI
            {
                Id = "versaoNFSe",
                Class = "col s12 m2",
                Label = "Versão NFS-e",
                MaxLength = 3,
                Data = new { inputmask = "'mask':'9.99', 'showMaskOnHover': false, 'autoUnmask':true" }
            });

            form3.Elements.Add(new InputTextUI { Id = "usuarioWebServer", Class = "col s12 m6", Label = "Usuário Web Server" });

            form3.Elements.Add(new InputPasswordUI { Id = "senhaWebServer", Class = "col s12 m4", Label = "Senha Web Server" });

            form3.Elements.Add(new InputTextUI { Id = "chaveAutenticacao", Class = "col s12 m4", Label = "Chave de Autenticação" });


            form3.Elements.Add(new InputTextUI { Id = "autorizacao", Class = "col s12 m4", Label = "Autorização" });

            form3.Elements.Add(new SelectUI
            {
                Id = "tipoTributacaoNFS",
                Class = "col s6 m6",
                Required = true,
                Label = "Tipo Tributação NFS",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoTributacaoNFS)))
            });

            form3.Elements.Add(new SelectUI
            {
                Id = "tipoAmbienteNFS",
                Class = "col s12 m6",
                Label = "Ambiente NFS",
                Required = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoAmbiente)))
            });
            #endregion

            #region Helpers

            #endregion
            cfg.Content.Add(form3);

            return cfg;
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        public JsonResult ImportaParametro(string mensagem, double simplesNacional, double fcp, double iss, double pispasep, double cofins,
            string numeroRetorno, string modalidade, string versao, string ambiente, string tipoPresencaComprador, string horarioVerao,
            string tipoHorario, string versaoNFSe, string usuarioWebServer, string senhaWebServer, string chaveAutenticacao, string autorizacao,
              string  tipoTributacaoNFS, string tipoAmbienteNFS)
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
                    senhaWebServer = senhaWebServer,
                    chaveAutenticacao = chaveAutenticacao,
                    autorizacao = autorizacao,
                    tipoTributacaoNFS = tipoTributacaoNFS,
                    tipoAmbienteNFS = tipoAmbienteNFS
                };

                if (dadosParametro.mensagemPadraoNota.Length > 4000)
                    return JsonResponseStatus.GetFailure("Número de caracteres na Mensagem Padrão na Nota não pode ser maior que 4000 caracteres.");

                if (dadosParametro.numeroRetornoNF.Length > 20)
                    return JsonResponseStatus.GetFailure("Número de caracteres no Número de Retorno da Nota Fiscal não pode ser maior que 20 caracteres.");

                ParametroTributarioVM parametroRetorno;

                var existeParametro = GetParametro();

                if (existeParametro == null)
                    parametroRetorno = RestHelper.ExecutePostRequest<ParametroTributarioVM>(ResourceName, JsonConvert.SerializeObject(dadosParametro, JsonSerializerSetting.Edit));
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