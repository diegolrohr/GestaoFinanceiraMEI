using Fly01.Compras.BL;
using System.Web.OData.Routing;
using System.Web.Http;
using System.Threading.Tasks;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.OData;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("orcamento")]
    public class OrcamentoController : ApiPlataformaController<Orcamento, OrcamentoBL>
    {

        public override async Task<IHttpActionResult> Post(Orcamento entity)
        {
            if (entity == null)
                return BadRequest(ModelState);

            ModelState.Clear();

            entity.Total = GetTotalOrcamentoItens(entity);

            Insert(entity);

            Validate(entity);

            if (!ModelState.IsValid)
                AddErrorModelState(ModelState);

            await UnitSave();

            return Created(entity);
        }

        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<Orcamento> model)
        {
            if (model == null || key == default(Guid))
                return BadRequest(ModelState);

            var entity = Find(key);

            if (entity == null || !entity.Ativo)
                return NotFound();

            ModelState.Clear();
            model.Patch(entity);

            entity.Total = GetTotalOrcamentoItens(entity);

            Update(entity);

            Validate(entity);

            if (!ModelState.IsValid)
                AddErrorModelState(ModelState);

            try
            {
                await UnitSave();

                if (MustProduceMessageServiceBus)
                    AfterSave(entity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(key))
                    return NotFound();

                throw;
            }

            return Ok();
        }

        private double GetTotalOrcamentoItens(Orcamento orcamento)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var total = unitOfWork
                                .OrcamentoItemBL
                                .All
                                .Where(x => x.OrcamentoId == orcamento.Id)
                                .ToList()
                                .Select(x => new
                                {
                                    Total = Convert.ToDouble(Math.Round((x.Quantidade * x.Valor) - x.Desconto, 2,
                                        MidpointRounding.AwayFromZero))
                                })
                                .Sum(x => x.Total);
                return total;
            }
        }
    }

}