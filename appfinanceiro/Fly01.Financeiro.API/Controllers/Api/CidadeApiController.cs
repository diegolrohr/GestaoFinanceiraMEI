using Fly01.Financeiro.BL;
using System.Web.Http;
using Fly01.Core.Controllers.API;
using Fly01.Financeiro.Domain.Entities;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/cidadeapi")]
    public class CidadeApiController : ApiBaseController
    {
        [HttpGet]
        [Route("nomelike/{nome}")]
        public IHttpActionResult NomeLike(string nome)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                Cidade cidade = unitOfWork.CidadeBL.FindByNome(nome);

                if (cidade == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(cidade);
                }
            }
        }
    }
}