using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasCadastrosCategoria)]
    public class CategoriaController : CategoriaBaseController<CategoriaVM>
    {
        public CategoriaController() 
            : base(ResourceHashConst.ComprasCadastrosCategoria) { }
    }
}