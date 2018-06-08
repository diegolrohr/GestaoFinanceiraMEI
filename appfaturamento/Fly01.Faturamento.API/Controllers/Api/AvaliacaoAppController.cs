using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.API;
using System.Web.Http;
using System.Configuration;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("avaliacaoapp")]
    public class AvaliacaoAppController : ApiPlataformaMongoBaseController<AvaliacaoApp>
    {
        public AvaliacaoAppController()
           : base(ConfigurationManager.AppSettings["MongoDBAvaliacaoApp"], ConfigurationManager.AppSettings["MongoCollectionNameAvaliacaoApp"]) { }
    }
}