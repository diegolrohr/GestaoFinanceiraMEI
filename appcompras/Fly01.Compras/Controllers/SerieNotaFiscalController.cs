using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasCadastros)]
    public class SerieNotaFiscalController : SerieNotaFiscalBaseController<SerieNotaFiscalVM>
    {
        public SerieNotaFiscalController()
            :base() { }
    }
}