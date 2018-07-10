using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using System;
using System.Web.Mvc;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(NotApply = true)]
    public class NCMController : BaseController<NcmVM>
    {
        public override ContentResult Form() { throw new NotImplementedException(); }

        public override Func<NcmVM, object> GetDisplayData() { throw new NotImplementedException(); }

        public override ContentResult List() { throw new NotImplementedException(); }
    }
}