using System;
using System.Web.Http;
using System.Reflection;

namespace Fly01.IntegracaoServiceBus.API.Controllers
{
    public class ConsumerController : ApiController
    {
        public IHttpActionResult GetMessage()
        {
            var integracaoConsumer = new IntegracaoConsumer();

            integracaoConsumer.Consume();

            return Ok();
        }
    }
}