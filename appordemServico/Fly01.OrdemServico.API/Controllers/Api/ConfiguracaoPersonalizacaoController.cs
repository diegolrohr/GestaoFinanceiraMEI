using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.OrdemServico.BL;

namespace Fly01.OrdemServico.API.Controllers.Api
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