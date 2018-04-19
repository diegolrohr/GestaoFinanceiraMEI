using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Faturamento.Controllers.Base;
using Fly01.Faturamento.ViewModel;
using System;
using System.Web.Mvc;

namespace Fly01.Faturamento.Controllers
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