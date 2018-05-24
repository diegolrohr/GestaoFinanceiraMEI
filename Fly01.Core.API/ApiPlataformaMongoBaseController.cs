using Fly01.Core.Entities.Domains;
using Fly01.Core.Notifications;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fly01.Core.API
{
    public class ApiPlataformaMongoBaseController<T> : ApiBaseController where T : PlataformaBase, new()
    {
        private string MongoDBName { get; set; }
        private string MongoCollectionName { get; set; }

        public ApiPlataformaMongoBaseController(string mongoDBName, string mongoCollectionName)
        {
            MongoDBName = mongoDBName;
            MongoCollectionName = mongoCollectionName;
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> Post(T entity)
        {
            entity.Id = Guid.NewGuid();
            entity.DataInclusao = DateTime.Now;
            entity.Ativo = true;
            entity.UsuarioInclusao = AppUser;
            entity.PlataformaId = PlataformaUrl;
            
            var mongoHelper = new LogMongoHelper<T>(MongoDBName);
            var collection = mongoHelper.GetCollection(MongoCollectionName);

            await collection.InsertOneAsync(entity);

            return Ok(new { entity.Id });
        }
    }
}
