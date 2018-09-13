using Fly01.EmissaoNFE.Domain.ViewModelNfs;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("transmissaoNFS")]
    public class TransmissaoNFSController : ApiController
    {
        public IHttpActionResult Post(TransmissaoNFSVM entity)
        {
            return null;
        }
    }
}