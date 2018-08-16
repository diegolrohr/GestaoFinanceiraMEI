using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasConfiguracoesNotasFiscaisInutilizadas)]
    public class NotaFiscalInutilizadaController : NotaFiscalInutilizadaBaseController<NotaFiscalInutilizadaVM>
    {
        public NotaFiscalInutilizadaController()
            : base() { }
    }
}