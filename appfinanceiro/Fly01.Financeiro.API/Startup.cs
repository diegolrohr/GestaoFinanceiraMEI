using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Fly01.Financeiro.API.Startup))]
namespace Fly01.Financeiro.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}