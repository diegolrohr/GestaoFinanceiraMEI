﻿using System.Web.Mvc;
using Fly01.Core;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.ViewModels.Presentation;

namespace Fly01.Faturamento.Controllers
{
    public class AvaliacaoAppController : AvaliacaoAppBaseController<AvaliacaoAppVM>
    {
        public AvaliacaoAppController()
        {
            ResourceName = AppDefaults.GetResourceName(typeof(AvaliacaoAppVM));
        }

        public override JsonResult Create(AvaliacaoAppVM entityVM)
        {
            entityVM.Aplicativo = "Faturamento";
            return base.Create(entityVM);
        }
    }
}