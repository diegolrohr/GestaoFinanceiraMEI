using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasCadastrosGrupoProdutos)]
    public class GrupoProdutoController : GrupoProdutoBaseController<GrupoProdutoVM> { }
}