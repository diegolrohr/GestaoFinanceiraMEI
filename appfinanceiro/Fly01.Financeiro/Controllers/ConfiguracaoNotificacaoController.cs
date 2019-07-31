using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Financeiro.ViewModel;
using Fly01.Core.Helpers;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System.Globalization;
using Fly01.uiJS.Classes.Helpers;
using Fly01.Core.Rest;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroConfiguracoesNotificacoes)]
    public class ConfiguracaoNotificacaoController : BaseController<ConfiguracaoNotificacaoVM>
    {
        public ContentResult GetData()
        {
            var entity = RestHelper.ExecuteGetRequest<ConfiguracaoNotificacaoVM>(ResourceName);

            if (entity == null)
            {
                entity = new ConfiguracaoNotificacaoVM()
                {
                    Id = Guid.NewGuid(),
                    ContasAPagar = true,
                    ContasAReceber = true
                };
            }

            return Content(JsonConvert.SerializeObject(entity, JsonSerializerSetting.Front), "application/json");
        }

        [HttpPost]
        public override JsonResult Edit(ConfiguracaoNotificacaoVM entityVM)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                return new JsonResult() { Data = new { success = false, message = message } };
            }

            entityVM.Id = default(Guid);

            var response = RestHelper.ExecuteGetRequest<ConfiguracaoNotificacaoVM>(ResourceName);

            if (response == null)
            {
                //inclusao
                return base.Create(entityVM);
            }
            else
            {
                //alteração
                entityVM.Id = response.Id;
                entityVM.CopyProperties<ConfiguracaoNotificacaoVM>(response);
                return base.Edit(response);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public override ContentResult Form() => base.Form();

        protected override ContentUI FormJson()
        {
            if (!UserCanRead)
                return new ContentUIBase(Url.Action("Sidebar", "Home"));

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Envio de notificações via SMS/E-mail",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = Url.Action("Edit"),
                    Edit = Url.Action("Edit"),
                    Get = Url.Action("GetData") + "/"
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "notificaViaSMS",
                Class = "col s12 m6",
                Label = "Notificar via SMS",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeCheckButtonSMS" }
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "notificaViaSMS",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Os agendamentos são processados 24h após a configuração."
                }
            });


            config.Elements.Add(new InputCheckboxUI
            {
                Id = "notificaViaEmail",
                Class = "col s12 m6 ",
                Label = "Notificar via E-Mail",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnChangeCheckButton" }
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "notificaViaEmail",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Os agendamentos são processados 24h após a configuração."
                }
            });


            config.Elements.Add(new InputHiddenUI { Id = "id", Value = Guid.NewGuid().ToString() });
            config.Elements.Add(new InputHiddenUI { Id = "platformUrlId" });
            config.Elements.Add(new InputHiddenUI { Id = "contasAPagar", Value = "true" });
            config.Elements.Add(new InputHiddenUI { Id = "contasAReceber", Value = "true" });

            config.Elements.Add(new SelectUI
            {
                Id = "diaSemana",
                Class = "col s12 m6",
                Label = "Dia da semana",
                Required = true,
                Options = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
                .Select(x => new SelectOptionUI { Id = x.ToString(), Label = DateTimeFormatInfo.CurrentInfo.GetDayName(x).ToUpper().First().ToString() + DateTimeFormatInfo.CurrentInfo.GetDayName(x).ToString().Substring(1), Value = ((int)x).ToString() })
                .ToList()
            });

            config.Elements.Add(new InputTimeUI { Id = "horaEnvio", Class = "col s12 m6", Label = "Horário de envio", Required = true, });
            config.Elements.Add(new InputTelUI { Id = "contatoDestino", Class = "col s12 m6", Label = "Celular destino", Required = true, MaxLength = 20 });
            config.Elements.Add(new InputEmailUI { Id = "emailDestino", Class = "col s12 m6", Label = "E-mail destino", Required = true, MaxLength = 70 });
            config.Elements.Add(new StaticTextUI
            {
                Id = "textInfo",
                Class = "col s12",
                Lines = new List<LineUI>
                {
                    new LineUI()
                    {
                        Tag = "h6",
                        Text = "Configuração para recebimento de notificações via SMS/E-mail, de contas a pagar/receber dos próximos 7 dias e saldo final projetado.",
                    }
                }
            });


            cfg.Content.Add(config);

            return cfg;
        }

        public override Func<ConfiguracaoNotificacaoVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}