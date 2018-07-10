using Fly01.Faturamento.ViewModel;
using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core;
using System.Linq;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation;
using Fly01.uiJS.Classes.Helpers;

namespace Fly01.Faturamento.Controllers
{
    public class ParametroTributarioController : BaseController<ParametroTributarioVM>
    {
        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            Dictionary<string, string> queryStringDefault = AppDefaults.GetQueryStringDefault();

            return queryStringDefault;
        }

        private ParametroTributarioVM GetParametro()
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
                    tipoHorario = "Brasilia"
                }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                registroSimplificadoMT = parametroTributario.RegistroSimplificadoMT,
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
                tipoHorario = parametroTributario.TipoHorario
            }, JsonRequestBehavior.AllowGet);
        }

        public override Func<ParametroTributarioVM, object> GetDisplayData() { throw new NotImplementedException(); }

        public override ContentResult List() { return Form(); }

        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index"),
                },
                Header = new HtmlUIHeader
                {
                    Title = "Parâmetros Tributários | Nota Fiscal",
                    Buttons = new List<HtmlUIButton>
                    {
                        new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnAtualizaParametro", Type = "submit" }
                    }
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

            form1.Elements.Add(new InputCheckboxUI { Id = "registroSimplificadoMT", Class = "col s12", Label = "Registro Simplificado de MT" });

            cfg.Content.Add(form1);

            var form2 = new FormUI
            {
                Class = "col s12",
                Elements = new List<BaseUI>
                {
                    new LabelSetUI { Id =  "sss", Class = "col s12", Label = "Alíquotas Padrões"}
                }

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
                    new LabelSetUI { Id =  "sss", Class = "col s12", Label = "Parâmetros de Transmissão"}
                }

            };

            //form3.Elements.Add(new InputCustommaskUI
            //{
            //    Id = "numeroRetornoNF",
            //    Class = "col s12 m3",
            //    Label = "Número de Retorno da NF",
            //    MaxLength = 20,
            //    Data = new { inputmask = "'regex': '[0-9]*'" }
            //});
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
                Label = "Versão NFe ",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoVersaoNFe)))
            });

            form3.Elements.Add(new SelectUI
            {
                Id = "tipoAmbiente",
                Class = "col s6 m6 l3",
                Label = "Ambiente",
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoAmbiente)))
            });

            form3.Elements.Add(new InputTextUI { Id = "mensagemPadraoNota", Class = "col s12", Label = "Mensagem Padrão na Nota", MaxLength = 1000 });

            #region NFS
            ////Paramentro NFS
            //config.Elements.Add(new LabelSetUI { Id = "simulatorLabel", Class = "col s12", Label = "Parâmentros NF Serviço" });

            //config.Elements.Add(new InputCheckboxUI { Id = "incentivoCultura", Class = "col s12", Label = "Incentivo à Cultura" });

            //config.Elements.Add(new SelectUI
            //{
            //    Id = "tipoRegimeEspecialTrib",
            //    Class = "col s12 m6",
            //    Label = "Regime Especial Tributário",
            //    Required = true,
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoRegimeEspecialTrib", true, false))
            //});

            //config.Elements.Add(new SelectUI
            //{
            //    Id = "tipoMensagemNFSE",
            //    Class = "col s12 m6",
            //    Label = "Tipo Mensagem NFS-e",
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoMensagemNFSE", true, false))
            //});

            //config.Elements.Add(new SelectUI
            //{
            //    Id = "tipoLayoutNFSE",
            //    Class = "col s12",
            //    Label = "Tipo Layout NFS-e",
            //    Required = true,
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoLayoutNFSE", true, false))
            //});

            //config.Elements.Add(new InputCheckboxUI { Id = "novoModeloUnicoXMLTSS", Class = "col s12 m6", Label = "Novo Modelo Único XML TSS" });

            //config.Elements.Add(new InputCustommaskUI
            //{
            //    Id = "versao",
            //    Class = "col s6 m3",
            //    Label = "Versão",
            //    MaxLength = 3,
            //    Data = new { inputmask = "'mask':'9.99', 'showMaskOnHover': false, 'autoUnmask':true" }
            //});


            //config.Elements.Add(new InputCustommaskUI
            //{
            //    Id = "siafi",
            //    Class = "col s6 m3",
            //    Label = "SIAFI",
            //    MaxLength = 4,
            //    Data = new { inputmask = "'regex': '[0-9]*'" }
            //});

            //config.Elements.Add(new SelectUI
            //{
            //    Id = "tipoAmbienteNFS",
            //    Class = "col s12 m6",
            //    Label = "Tipo Ambiente NFS",
            //    Required = true,
            //    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase("TipoAmbienteNFS", true, false))
            //});

            ////config.Elements.Add(new InputTextUI { Id = "aEDFe", Class = "col s12 m6", Label = "AEDFe"});

            //config.Elements.Add(new InputTextUI { Id = "usuario", Class = "col s12 m4", Label = "Usuário" });

            //config.Elements.Add(new InputPasswordUI { Id = "senha", Class = "col s12 m4", Label = "Senha" });

            //config.Elements.Add(new InputTextUI { Id = "chaveAutenticacao", Class = "col s12 m4", Label = "Chave de Autenticacao" });

            #endregion


            #region Helpers 
            form3.Helpers.Add(new TooltipUI
            {
                Id = "mensagemPadraoNota",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Informe | entre as palavras, para exibir quebra de linha(enter) na impressão da DANFE. Exemplo: TextoLinha1 | TextoLinha2 | TextoLinha3."
                }
            });
            #endregion
            cfg.Content.Add(form3);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Default), "application/json");
        }

        public JsonResult ImportaParametro(string mensagem, bool registro, double simplesNacional, double fcp, double iss, double pispasep, double cofins, string numeroRetorno, string modalidade, string versao, string ambiente, string tipoPresencaComprador, string horarioVerao, string tipoHorario)
        {
            try
            {
                var dadosParametro = new
                {
                    simplesNacional = "True",
                    registroSimplificadoMT = registro == true ? "True" : "False",
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
                    tipoHorario = tipoHorario
                };

                if (dadosParametro.mensagemPadraoNota.Length > 200)
                    return JsonResponseStatus.GetFailure("Número de caracteres na Mensagem Padrão na Nota não pode ser maior que 200");

                if (dadosParametro.numeroRetornoNF.Length > 20)
                    return JsonResponseStatus.GetFailure("Número de caracteres no Número de Retorno da Nota Fiscal não pode ser maior que 20");

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