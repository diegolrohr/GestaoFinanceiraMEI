using Fly01.Core;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using System.Linq;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoConfiguracoesParametrosTributarios)]
    public class ParametroTributarioController : ParametroTributarioBaseController<ParametroTributarioVM>
    {
        public ParametroTributarioController()
            : base() { }
    }
}
