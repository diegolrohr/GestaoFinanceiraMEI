﻿using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using System;
using System.Web.Mvc;

namespace Fly01.Estoque.Controllers
{
    [OperationRole(NotApply = true)]
    public class CestController : BaseController<CestVM>
    {
        protected override ContentUI FormJson() { throw new NotImplementedException(); }

        public override Func<CestVM, object> GetDisplayData() { throw new NotImplementedException(); }

        public override ContentResult List() { throw new NotImplementedException(); }
    }
}