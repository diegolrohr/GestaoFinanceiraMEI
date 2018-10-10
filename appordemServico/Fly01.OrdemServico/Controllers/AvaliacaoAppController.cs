using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation;

namespace Fly01.OrdemServico.Controllers
{
    public class AvaliacaoAppController : AvaliacaoAppBaseController<AvaliacaoAppVM>
    {
        public AvaliacaoAppController()
            : base(ResourceHashConst.OrdemServicoAvalieAplicativo) { }

        [OperationRole(ResourceKey = ResourceHashConst.OrdemServicoAvalieAplicativo, PermissionValue = EPermissionValue.Read)]
        public override JsonResult Create(AvaliacaoAppVM entityVM)
        {
            entityVM.Aplicativo = "OrdemServico";
            return base.Create(entityVM);
        }
    }
}