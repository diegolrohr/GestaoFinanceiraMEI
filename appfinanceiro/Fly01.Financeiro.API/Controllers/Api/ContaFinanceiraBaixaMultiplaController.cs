﻿using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Core.Entities.Domains.Commons;
using System.Threading.Tasks;
using System.Web.Http;
using Fly01.Core.ServiceBus;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("contafinanceirabaixamultipla")]
    public class ContaFinanceiraBaixaMultiplaController : ApiPlataformaController<ContaFinanceiraBaixaMultipla, ContaFinanceiraBaixaMultiplaBL>
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

            if (MustProduceMessageServiceBus)
                Producer<ContaFinanceiraBaixaMultipla>.Send(entity.GetType().Name, AppUser, PlataformaUrl, entity, RabbitConfig.EnHttpVerb.POST);

            return Created(entity);
        }
    }
}