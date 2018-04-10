using System.Web.Http;
using Owin;

namespace Fly01.Core.API.Application
{
    public partial class GlobalStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.Map("/api", map =>
            {
                HttpConfiguration config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();
                map.UseWebApi(config);
            });
        }
    }
}
