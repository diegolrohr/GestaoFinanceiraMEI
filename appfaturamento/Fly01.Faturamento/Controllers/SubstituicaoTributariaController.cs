using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Controllers;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoCadastrosSubstituicaoTributaria)]
    public class SubstituicaoTributariaController : SubstituicaoTributariaBaseController<SubstituicaoTributariaVM> { }
}