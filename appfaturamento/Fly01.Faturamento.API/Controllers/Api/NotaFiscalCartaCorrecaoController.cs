using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Faturamento.BL;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Routing;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("notafiscalcartacorrecao")]
    public class NotaFiscalCartaCorrecaoController : ApiPlataformaController<NotaFiscalCartaCorrecao, NotaFiscalCartaCorrecaoBL>
    {
    }
}
