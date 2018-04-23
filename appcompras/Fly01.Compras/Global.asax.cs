using Fly01.Core.Presentation.Application;
using System.Configuration;

namespace Fly01.Compras
{
    public class WebApiApplication : GlobalHttpApplication
    {
        protected override string GetInstrumentationKeyAppInsights() => ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"];
    }
}
