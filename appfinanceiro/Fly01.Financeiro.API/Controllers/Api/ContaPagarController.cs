using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using Fly01.Financeiro.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("contapagar")]
    public class ContaPagarController : ApiPlataformaController<ContaPagar, ContaPagarBL>
    {
        public ContaPagarController()
        {
            MustProduceMessageServiceBus = true;
        }

        [EnableQuery(PageSize = 1000, MaxTop = 1000, MaxExpansionDepth = 10)]
        public override IHttpActionResult Get()
        {
            return Ok(All().AsQueryable());
        }

        [HttpDelete]
        public override Task<IHttpActionResult> Delete(Guid key)
        {
            var allUrlKeyValues = ControllerContext.Request.GetQueryNameValuePairs();
            bool excluirRecorrencias;
            bool.TryParse(allUrlKeyValues.LastOrDefault(x => x.Key.ToLower() == "excluirrecorrencias").Value, out excluirRecorrencias);

            if (excluirRecorrencias)
                return DeleteAll(key);

            return base.Delete(key);
        }

        public async Task<IHttpActionResult> DeleteAll(Guid key)
        {
            if (key == default(Guid))
                return BadRequest();

            var entity = Find(key);

            if (entity == null || !entity.Ativo)
                throw new BusinessException("Registro não encontrado ou já excluído");

            if (entity.RegistroFixo)
                throw new BusinessException("Registro não pode ser excluído (RegistroFixo)");

            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var isParent = entity.ContaFinanceiraRepeticaoPaiId == null && entity.Repetir;
                List<ContaPagar> recorrencias;

                if (isParent)
                {
                    recorrencias = unitOfWork
                        .ContaPagarBL
                        .All
                        .Where(x => (x.ContaFinanceiraRepeticaoPaiId == entity.Id || x.Id == entity.Id) &&
                                    x.StatusContaBancaria == StatusContaBancaria.EmAberto)
                        .ToList();
                }
                else
                {
                    recorrencias = unitOfWork
                        .ContaPagarBL
                        .All
                        .Where(x => (x.ContaFinanceiraRepeticaoPaiId == entity.ContaFinanceiraRepeticaoPaiId || x.Id == entity.ContaFinanceiraRepeticaoPaiId)
                                    && x.StatusContaBancaria == StatusContaBancaria.EmAberto && x.Numero >= entity.Numero)
                        .ToList();
                }

                foreach (var child in recorrencias.OrderByDescending(x => x.Numero))
                {
                    Delete(child);
                    await unitOfWork.Save();
                    if (MustProduceMessageServiceBus)
                        Producer<ContaPagar>.Send(child.GetType().Name, AppUser, PlataformaUrl, child,
                            RabbitConfig.enHTTPVerb.DELETE);
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}