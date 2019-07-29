using Fly01.Core.API;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;
using Fly01.Core.Mensageria;
using Fly01.Core.Entities.Domains.NoSQL;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("avaliacaoapp")]
    public class AvaliacaoAppController : ApiPlataformaMongoBaseController<AvaliacaoApp>
    {
        protected MediaClient _mediaClient;
        public AvaliacaoAppController()
            : base(ConfigurationManager.AppSettings["MongoDBAvaliacaoApp"], ConfigurationManager.AppSettings["MongoCollectionNameAvaliacaoApp"]) { }
    }
}