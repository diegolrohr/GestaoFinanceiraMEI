using Fly01.Faturamento.BL;
using System.Web.OData.Routing;
using Fly01.Faturamento.Domain.Entities;


namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("formapagamento")]
    public class FormaPagamentoController : ApiPlataformaController<FormaPagamento, FormaPagamentoBL>
    {
        public FormaPagamentoController()
        {
            MustProduceMessageServiceBus = true;
        }
    }
}