using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation;

namespace Fly01.Estoque.Controllers
{
    public class AvaliacaoAppController : AvaliacaoAppBaseController<AvaliacaoAppVM>
    {
        [OperationRole(ResourceKey = ResoucerHashConst.EstoqueAvalieAplicativo, PermissionValue = EPermissionValue.Read)]
        public override JsonResult Create(AvaliacaoAppVM entityVM)
        {
            entityVM.Aplicativo = "Estoque";
            return base.Create(entityVM);
        }
    }
}