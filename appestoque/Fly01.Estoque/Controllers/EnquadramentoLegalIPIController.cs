using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Estoque.Controllers.Base;
using System;
using System.Web.Mvc;

namespace Fly01.Estoque.Controllers
{
    public class EnquadramentoLegalIPIController : BaseController<EnquadramentoLegalIpiVM>
    {
        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override Func<EnquadramentoLegalIpiVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }
    }
}