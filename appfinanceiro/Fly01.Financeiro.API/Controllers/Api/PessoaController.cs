using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Threading.Tasks;
using System.Web.Http;
using Fly01.Core.Mensageria;
using Newtonsoft.Json;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("pessoa")]
    public class PessoaController : ApiPlataformaController<Pessoa, PessoaBL>
    {
        public PessoaController()
        {
            MustProduceMessageServiceBus = true;
        }
    }
}