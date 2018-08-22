
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels;
using Fly01.OrdemServico.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using System;
using System.Web.Mvc;

namespace Fly01.OrdemServico.Controllers
{
    public class ParametroOrdemServicoController : BaseController<ParametroOrdemServicoVM>
    {

        public override ContentResult List()
            => Form();

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public override ContentResult Form() => base.Form();

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Parâmetros da Ordem de Serviço"
                }

            };

            var config = new FormUI
            {
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                }
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });

            config.Elements.Add(new InputNumbersUI { Id = "diasEntrega", Class = "col s12 m3", Label = "Nº de dias sugeridos para a previsão de entrega", Required = true });

            cfg.Content.Add(config);

            return cfg;
        }

        public override Func<ParametroOrdemServicoVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }
    }
}