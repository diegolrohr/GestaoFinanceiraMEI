﻿using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoConfiguracoesParametrosTributarios)]
    public class ParametroTributarioController : ParametroTributarioBaseController<ParametroTributarioVM>
    {    
        public ParametroTributarioController()
            : base() { }
    }
}