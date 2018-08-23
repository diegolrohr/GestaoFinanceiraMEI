﻿using Fly01.Core.Presentation.Application;
using System.Configuration;

namespace Fly01.OrdemServico
{
    public class WebApiApplication : GlobalHttpApplication
    {
        protected override string GetInstrumentationKeyAppInsights() => ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"];
    }
}