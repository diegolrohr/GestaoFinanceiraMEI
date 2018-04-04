using Owin;
using System.Web.Http;

namespace Fly01.Estoque.API
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            //ConfigureOAuth(app);

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
            // Configure the db context and user manager to use a single instance per request
            ///!!!! app.CreatePerOwinContext(AppDataContext.Create);


            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            //// TODO_: Colocar validação do cookie da sessão através do owin

            //OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            //{
            //    AllowInsecureHttp = true,
            //    TokenEndpointPath = new PathString("/token"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
            //    Provider = new SimpleAuthorizationServerProvider()
            //};

            //// Token Generation
            //app.Use<AuthenticationMiddleware>(); //Allows override of Invoke OWIN commands
            //app.UseOAuthAuthorizationServer(OAuthServerOptions);
            //app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}