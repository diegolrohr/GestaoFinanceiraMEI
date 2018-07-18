﻿using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoConfiguracoesCertificadoDigital)]
    public class CertificadoDigitalController : CertificadoDigitalBaseController<CertificadoDigitalVM>
    {
        public CertificadoDigitalController()
            : base() { }
    }
}