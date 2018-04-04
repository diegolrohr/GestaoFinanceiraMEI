using Fly01.Estoque.Controllers.Base;
using Fly01.Estoque.Entities.ViewModel;
using System;
using System.Web.Mvc;

namespace Fly01.Estoque.Controllers
{
    public class CestController : BaseController<CestVM>
    {
        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override Func<CestVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }
    }
}