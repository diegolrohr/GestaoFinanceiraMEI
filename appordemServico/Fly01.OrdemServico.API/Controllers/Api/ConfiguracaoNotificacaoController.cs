using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Web.Http;
using System.Linq;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("configuracaonotificacao")]
    public class ConfiguracaoNotificacaoController : ApiPlataformaController<ConfiguracaoNotificacaoFinanceiro, ConfiguracaoNotificacaoBL>
    {
        public override IHttpActionResult Get()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var response = unitOfWork.ConfiguracaoNotificacaoBL.All.FirstOrDefault();
                if(response == null)
                    return Ok();
                else
                    return Ok(response);
            }
        }
    }
}