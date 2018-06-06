using System.Web.Mvc;
using Fly01.Core;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.ViewModels.Presentation;

namespace Fly01.Estoque.Controllers
{
    public class AvaliacaoAppController : AvaliacaoAppBaseController<AvaliacaoAppVM>
    {
        public override JsonResult Create(AvaliacaoAppVM entityVM)
        {
            entityVM.Aplicativo = "Estoque";
            return base.Create(entityVM);
        }
    }
}