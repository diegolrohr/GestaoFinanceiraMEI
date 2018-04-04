using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Fly01.Compras.API.Startup))]
namespace Fly01.Compras.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}