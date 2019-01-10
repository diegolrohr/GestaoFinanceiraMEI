using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
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
                Class = "col s12"
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
    }
}