using Microsoft.Owin;
using Fly01.Core.API.Application;

[assembly: OwinStartup(typeof(Fly01.Compras.API.Startup))]
namespace Fly01.Compras.API
{
    public partial class Startup : GlobalStartup
    {
    }
}