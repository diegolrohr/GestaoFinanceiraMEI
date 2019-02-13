using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroCadastrosCentroCustos)]
    public class CentroCustoController : CentroCustoBaseController<CentroCustoVM>
    {
    }
}