﻿using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using System;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
{
    [OperationRole(NotApply = true)]
    public class NCMController : BaseController<NcmVM>
    {
        protected override ContentUI FormJson() { throw new NotImplementedException(); }

        public override Func<NcmVM, object> GetDisplayData() { throw new NotImplementedException(); }

        public override ContentResult List() { throw new NotImplementedException(); }
    }
}