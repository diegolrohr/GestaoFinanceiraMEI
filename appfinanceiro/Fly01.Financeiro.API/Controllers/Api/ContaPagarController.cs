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

            return excluirRecorrencias
                ? DeleteAll(key)
                : base.Delete(key);
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
                var isChild = entity.ContaFinanceiraRepeticaoPaiId != null;
                //var isParent = unitOfWork.ContaPagarBL.All.Any(x => x.ContaFinanceiraRepeticaoPaiId == entity.Id);
                var isParent = entity.ContaFinanceiraRepeticaoPaiId == null && entity.Repetir;
                //TODO: Replicar para contas a receber

                List<ContaPagar> childs = null;

                if (isChild)
                {
                    childs = unitOfWork
                        .ContaPagarBL
                        .All
                        .Where(x => (x.ContaFinanceiraRepeticaoPaiId == entity.ContaFinanceiraRepeticaoPaiId || x.Id == entity.ContaFinanceiraRepeticaoPaiId) 
                                    && x.StatusContaBancaria == StatusContaBancaria.EmAberto && x.DataInclusao > entity.DataInclusao)
                        .ToList();
                }
                else if (isParent)
                {
                    childs = unitOfWork
                        .ContaPagarBL
                        .All
                        .Where(x => (x.ContaFinanceiraRepeticaoPaiId == entity.Id || x.Id == entity.Id) && x.StatusContaBancaria == StatusContaBancaria.EmAberto && x.DataInclusao > entity.DataInclusao)
                        .ToList();
                }

                if (childs != null)
                {
                    // Exclui filhas
                    foreach (var child in childs.Where(x => x.ContaFinanceiraRepeticaoPaiId == null))
                    {
                        Delete(child);
                        await unitOfWork.Save();
                        if (MustProduceMessageServiceBus)
                            Producer<ContaPagar>.Send(child.GetType().Name, AppUser, PlataformaUrl, child,
                                RabbitConfig.enHTTPVerb.DELETE);
                    }

                    // Exclui pai
                    var parent = childs.First(x => x.ContaFinanceiraRepeticaoPaiId != null);
                    if (parent == null) return StatusCode(HttpStatusCode.NoContent);
                    Delete(parent);
                    await unitOfWork.Save();
                    if (MustProduceMessageServiceBus)
                        Producer<ContaPagar>.Send(parent.GetType().Name, AppUser, PlataformaUrl, parent,
                            RabbitConfig.enHTTPVerb.DELETE);
                }
                else
                {
                    Delete(entity);
                    await UnitSave();
                    if (MustProduceMessageServiceBus)
                        Producer<ContaPagar>.Send(entity.GetType().Name, AppUser, PlataformaUrl, entity,
                            RabbitConfig.enHTTPVerb.DELETE);
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}