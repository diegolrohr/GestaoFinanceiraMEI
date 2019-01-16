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
using Fly01.uiJS.Enums;
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

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo", OnClickFn = "fnNovaOS", Position = HtmlUIButtonPosition.Main });

            return target;
        }

        public override ContentResult List()
            => Form();

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index"),
                    WithParams = Url.Action("Edit")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Agenda",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                SidebarUrl = Url.Action("Sidebar", "Home")
            };
            cfg.Content.Add(new DivUI
            {
                Class = "col s12",
                Id = "legenda",
            });
            cfg.Content.Add(new CalendarUI
            {
                Id = "calendar",
                Class = "col s10 offset-s1",
                Options = new CalendarUIConfig()
                {
                    EventLimit = true
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

                return Json(new
                {
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