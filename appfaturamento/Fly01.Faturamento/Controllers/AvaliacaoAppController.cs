using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation;

namespace Fly01.Faturamento.Controllers
{
    public class AvaliacaoAppController : AvaliacaoAppBaseController<AvaliacaoAppVM>
    {
        public AvaliacaoAppController()
            : base(ResourceHashConst.FaturamentoAvalieAplicativo) { }

        [OperationRole(ResourceKey = ResourceHashConst.FaturamentoAvalieAplicativo, PermissionValue = EPermissionValue.Read)]
        public override JsonResult Create(AvaliacaoAppVM entityVM)
        {
            entityVM.Aplicativo = "Faturamento";
            return base.Create(entityVM);
        }
    }
}