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
using System.Linq;
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
            
            cfg.Content.Add(new CalendarUI
            {
                Id = "calendar",
                Class = "col s12",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Header = new CalendarUIHeader
                {
                    Left = "prev,next today",
                    Center = "title",
                    Right = "month,agendaWeek,agendaDay"
                },
                Options = new CalendarUIConfig()
                {
                    EventLimit = true,
                    Selectable = true,
                    Editable = false,
                    MinTime = "00:00:00",
                    MaxTime = "23:00:00"
                
                },
                Callbacks = new CalendarUICallbacks()
                {
                    DayClick = "fnDayClick",
                    Select = "fnSelect"
                },
                UrlData = Url.Action("GetOSAgenda")
            });
            cfg.Content.Add(new DivUI
            {
                Id = "legenda",
                Class = "col s12 ",
                Elements = new List<BaseUI>
                {
                    new StaticTextUI
                    {
                        Id = "leg1",
                        Class = "col s12 center",
                        Lines = EnumHelper.GetDataEnumValues(typeof(StatusOrdemServico)).OrderByDescending(x => x.Description).Select(x => new LineUI()
                        {
                            Tag = "span",
                            Class = $"badge cool {x.CssClass}",
                            Text = x.Description
                        }).ToList()
                    }
                }
            });
            return cfg;
        }

        public override Func<OrdemServicoVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public JsonResult GetOSAgenda(DateTime initialDate, DateTime finalDate)
        {
            try
            {
                Dictionary<string, string> queryString = new Dictionary<string, string>
                {
                    { "dataInicial", initialDate.ToString("yyyy-MM-dd") },
                    { "dataFinal", finalDate.ToString("yyyy-MM-dd") },
                };

                var response = RestHelper.ExecuteGetRequest<List<AgendaVM>>("agenda", queryString);

                return Json(new
                {
                    events = response.Select(x => new
                    {
                        className = x.ClassName,
                        title = x.Title,
                        start = x.Start,
                        end   = x.End,
                        url   = x.Url
                    }),
                    success = true
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