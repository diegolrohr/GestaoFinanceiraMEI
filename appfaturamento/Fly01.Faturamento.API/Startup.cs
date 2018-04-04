using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Fly01.Faturamento.API.Startup))]
namespace Fly01.Faturamento.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}