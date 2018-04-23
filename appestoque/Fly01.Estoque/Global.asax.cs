using Fly01.Core.Presentation.Application;
using System.Configuration;

namespace Fly01.Estoque
{
    public class WebApiApplication : GlobalHttpApplication
    {
        protected override string GetInstrumentationKeyAppInsights() => ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"];
    }
}
