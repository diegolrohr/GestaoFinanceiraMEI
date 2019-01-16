using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.OrdemServico.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.OrdemServico.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.OrdemServico)]
    public class AgendaController : BaseController<OrdemServicoVM>
    {
        public AgendaController()
        {
            ExpandProperties = "cliente($select=id,nome,email,cpfcnpj,endereco,celular,telefone;$expand=cidade($select=nome),estado($select=sigla))";
        }

        private List<HtmlUIButton> GetButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovo" });

            return target;
        }

        public override ContentResult List()
            => Form();

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Agenda"
                }
            };
            cfg.Content.Add(new CalendarUI
            {
                Id = "calendar",
                Class = "col s10 offset-s1",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Options = new CalendarUIConfig()
                {
                    EventLimit = true,
                    Selectable = true
                },
                Callbacks = new CalendarUICallbacks()
                {
                    DayClick = "fnDayClick",
                    Select = "fnSelect"
                }
            });

            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnReadyAgenda"
            };

            cfg.Content.Add(config);
            return cfg;
        }

        public override Func<OrdemServicoVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public JsonResult GetOSAgenda(DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                    { "dataInicial", dataInicial.ToString("yyyy-MM-dd") },
                    { "dataFinal", dataFinal.ToString("yyyy-MM-dd") },
                };

                var response = RestHelper.ExecuteGetRequest<List<AgendaVM>>("agenda", queryString);

                return Json(new {
                    response,
                    sucess = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }
    }
}