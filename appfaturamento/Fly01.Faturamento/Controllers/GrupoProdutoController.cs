using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoCadastrosGrupoProdutos)]
    public class GrupoProdutoController : GrupoProdutoBaseController<GrupoProdutoVM> { }
}