using System.Web.Http;
using System.Web.OData.Extensions;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Fly01.EmissaoNFE.API.Startup))]
namespace Fly01.EmissaoNFE.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}