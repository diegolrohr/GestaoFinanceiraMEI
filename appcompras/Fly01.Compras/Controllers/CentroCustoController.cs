using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasCadastrosCentroCustos)]
    public class CentroCustoController : CentroCustoBaseController<CentroCustoVM>
    {
    }
}