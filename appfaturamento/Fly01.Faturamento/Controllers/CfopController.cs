using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using System;
using System.Web.Mvc;

namespace Fly01.Faturamento.Controllers
{
    public class CfopController : BaseController<CfopVM>
    {
        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override Func<CfopVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }
    }
}