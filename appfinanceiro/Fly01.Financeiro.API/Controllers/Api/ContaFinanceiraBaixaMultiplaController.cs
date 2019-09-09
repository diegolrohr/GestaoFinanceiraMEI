using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("contafinanceirabaixamultipla")]
    public class ContaFinanceiraBaixaMultiplaController : ApiEmpresaController<ContaFinanceiraBaixaMultipla, ContaFinanceiraBaixaMultiplaBL>
    {
        public override async Task<IHttpActionResult> Post(ContaFinanceiraBaixaMultipla entity)
        {
            if (entity == null)
                return BadRequest(ModelState);

            ModelState.Clear();

            Insert(entity);
            
            Validate(entity);

            if (!ModelState.IsValid)
                AddErrorModelState(ModelState);

            foreach (var baixa in UnitOfWork.ContaFinanceiraBaixaMultiplaBL.GeraBaixas(entity))
            {
                UnitOfWork.ContaFinanceiraBaixaBL.Insert(baixa);
                await UnitSave();
            }

            return Created(entity);
        }
    }
}