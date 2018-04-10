using Fly01.Core.API.Application;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Fly01.Faturamento.API.Startup))]
namespace Fly01.Faturamento.API
{
    public partial class Startup : GlobalStartup
    {
    }
}