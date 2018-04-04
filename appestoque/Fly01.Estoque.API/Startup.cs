using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Fly01.Estoque.API.Startup))]

namespace Fly01.Estoque.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
