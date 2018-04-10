using Fly01.Core.API.Application;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Fly01.EmissaoNFE.API.Startup))]
namespace Fly01.EmissaoNFE.API
{
    public partial class Startup : GlobalStartup
    {
    }
}