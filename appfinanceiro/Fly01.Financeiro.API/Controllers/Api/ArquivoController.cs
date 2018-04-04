﻿using Fly01.Financeiro.BL;
using System.Web.OData.Routing;
using Fly01.Financeiro.Domain.Entities;
using System.Web.Http;
using System.Threading.Tasks;
using Fly01.Core.Notifications;
using System.Web.Http.ModelBinding;
using System.Linq;
using Fly01.Core.ValueObjects;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("arquivo")]
    public class ArquivoController : ApiPlataformaController<Arquivo, ArquivoBL>
    {
        public ArquivoController()
        {
            MustProduceMessageServiceBus = true;
        }

        public override async Task<IHttpActionResult> Post(Arquivo entity)
        {
            if (entity == null)
                return BadRequest(ModelState);

            ModelState.Clear();

            Insert(entity);

            Validate(entity);

            if (!ModelState.IsValid)
                AddErrorModelState(ModelState);

            await UnitSave();

            if (MustProduceMessageServiceBus)
                AfterSave(entity);

            return Created(entity);
        }

        private Notification Notification { get; } = new Notification();

        private void AddErrorModelState(ModelStateDictionary modelState)
        {
            modelState.ToList().ForEach(
                model => model.Value.Errors.ToList().ForEach(
                    itemErro => Notification.Errors.Add(
                        new Error(itemErro.ErrorMessage, string.Concat(char.ToLowerInvariant(model.Key[0]), model.Key.Substring(1))))));

            throw new BusinessException(Notification.Get());
        }
    }
}