using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Estoque.BL;

namespace Fly01.Estoque.API.Controllers.Api
{
    [ODataRoutePrefix("configuracaopersonalizacao")]
    public class ConfiguracaoPersonalizacaoController : ApiPlataformaController<ConfiguracaoPersonalizacao, ConfiguracaoPersonalizacaoBL>
    {
        public ConfiguracaoPersonalizacaoController()
        {
            MustProduceMessageServiceBus = true;
        }
    }
}