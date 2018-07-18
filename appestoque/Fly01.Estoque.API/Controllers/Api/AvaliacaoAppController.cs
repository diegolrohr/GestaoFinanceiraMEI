using Fly01.Core.API;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;
using Fly01.Core.Mensageria;
using Fly01.Core.Entities.Domains.NoSQL;

namespace Fly01.Estoque.API.Controllers.Api
{
    [RoutePrefix("avaliacaoapp")]
    public class AvaliacaoAppController : ApiPlataformaMongoBaseController<AvaliacaoApp>
    {
        public AvaliacaoAppController()
           : base(ConfigurationManager.AppSettings["MongoDBAvaliacaoApp"], ConfigurationManager.AppSettings["MongoCollectionNameAvaliacaoApp"]) { }

        public override Task<IHttpActionResult> Post(AvaliacaoApp entity)
        {
            SlackClient.PostNotificacaoAvaliacaoApp(entity.Id, Request.RequestUri.AbsoluteUri.Split('.')[1], entity.Descricao, PlataformaUrl);

            return base.Post(entity);
        }
    }
}