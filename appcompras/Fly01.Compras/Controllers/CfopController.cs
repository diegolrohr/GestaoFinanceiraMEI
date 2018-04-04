using Fly01.Compras.Controllers.Base;
using Fly01.Compras.Entities.ViewModel;
using System;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
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