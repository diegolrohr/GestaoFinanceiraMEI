using Fly01.Core.Defaults;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    //$$$.modal("/AliquotaSimplesNacional/FormModal?idParametro=00000000-0000-0000-0000-000000000000")
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

        public override JsonResult Create(T entityVM)
        {
            //salvar novo parametro tributario
            throw new NotImplementedException();
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
                Id = "ipi",
                Class = "col s12 m4",
                Label = "IPI",
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
    }
}