using Fly01.Core.Entities.Domains.Commons;
using Fly01.Financeiro.BL;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{

    [RoutePrefix("stoneantecipacaorecebiveis")]
    public class StoneAntecipacaoRecebiveisController : ApiPlataformaController<StoneAntecipacaoRecebiveis, StoneAntecipacaoRecebiveisBL>
    {
    }
}