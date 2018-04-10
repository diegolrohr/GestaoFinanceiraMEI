using Fly01.Core.API.Application;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Fly01.Estoque.API.Startup))]
namespace Fly01.Estoque.API
{
    public partial class Startup : GlobalStartup
    {
    }
}
