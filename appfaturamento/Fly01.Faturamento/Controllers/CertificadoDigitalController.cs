﻿using Fly01.Faturamento.ViewModel;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation;

namespace Fly01.Faturamento.Controllers
{
    public class CertificadoDigitalController : CertificadoDigitalBaseController<CertificadoDigitalVM>
    {
        public CertificadoDigitalController()
            : base(/*ResourceHashConst.FaturamentoConfiguracoesCertificadoDigital*/) { }

    }
}