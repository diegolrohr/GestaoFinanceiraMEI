using Fly01.Core.API.Application;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Fly01.OrdemServico.API.Startup))]
namespace Fly01.OrdemServico.API
{
    public partial class Startup : GlobalStartup
    {
    }
}