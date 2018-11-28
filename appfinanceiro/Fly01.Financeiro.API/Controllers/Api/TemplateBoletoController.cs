using Fly01.Core.Entities.Domains.Commons;
using Fly01.Financeiro.BL;
using System.Web.OData.Routing;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("templateboleto")]
    public class TemplateBoletoController : ApiPlataformaController<TemplateBoleto, TemplateBoletoBL>
    {
    }
}