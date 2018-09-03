using Fly01.Core.Entities.Domains.Commons;
using Fly01.Compras.BL;
using System.Web.OData.Routing;

namespace Fly01.Compras.API.Controllers.Api
{
    [ODataRoutePrefix("notafiscalcartacorrecaoentrada")]
    public class NotaFiscalCartaCorrecaoEntradaController : ApiPlataformaController<NotaFiscalCartaCorrecaoEntrada, NotaFiscalCartaCorrecaoEntradaBL>
    {
    }
}
