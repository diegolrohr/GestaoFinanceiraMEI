using Fly01.Faturamento.Controllers.Base;
using Fly01.Faturamento.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fly01.Faturamento.Controllers
{
    public class EnquadramentoLegalIPIController : BaseController<EnquadramentoLegalIPIVM>
    {
        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }

        public override Func<EnquadramentoLegalIPIVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }
    }
}