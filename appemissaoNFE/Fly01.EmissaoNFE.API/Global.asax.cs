using Fly01.Core.API.Application;
using Microsoft.OData.Edm;
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

        protected override Task RunServiceBus()
        {
            return Task.Factory.StartNew(() => { });
        }
    }
}
