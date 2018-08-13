using Fly01.Core.API.Application;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Fly01.Financeiro.API.Startup))]
namespace Fly01.Financeiro.API
{
    public partial class Startup : GlobalStartup
    {
    }
}