using Fly01.Core.API.Application;
using Microsoft.OData.Edm;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.OData.Builder;

namespace Fly01.EmissaoNFE.API
{
    public class WebApiApplication : GlobalWebAPIApplication
    {
        protected override IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder
            {
                ContainerName = "DefaultContainer"
            };

            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }

        protected override string GetInstrumentationKeyAppInsights() => ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"];

        protected override Task RunServiceBus() => Task.Factory.StartNew(() => { });
        protected override Task RunServiceBus2() => Task.Factory.StartNew(() => { });
        protected override Task RunServiceBus3() => Task.Factory.StartNew(() => { });
        protected override Task RunServiceBus4() => Task.Factory.StartNew(() => { });
        protected override Task RunServiceBus5() => Task.Factory.StartNew(() => { });
    }
}
