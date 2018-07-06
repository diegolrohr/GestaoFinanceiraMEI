using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation;

namespace Fly01.Estoque.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.EstoqueCadastrosGrupoProdutos)]
    public class GrupoProdutoController : GrupoProdutoBaseController<GrupoProdutoVM>
    {
    }
}