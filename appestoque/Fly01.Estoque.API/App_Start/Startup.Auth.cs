using Owin;
using System.Web.Http;

namespace Fly01.Estoque.API
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.Map("/api", map =>
            {
                HttpConfiguration config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();
                map.UseWebApi(config);
            });
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
        }
    }
}