﻿using Fly01.Core.API;
using System.Web.Http;
using System.Configuration;
using Fly01.Core.Mensageria;
using System.Threading.Tasks;
using Fly01.Core.Entities.Domains.NoSQL;

namespace Fly01.Compras.API.Controllers.Api
{
    [RoutePrefix("avaliacaoapp")]
    public class AvaliacaoAppController : ApiPlataformaMongoBaseController<AvaliacaoApp>
    {
        protected MediaClient _mediaClient; 
        public AvaliacaoAppController()
           : base(ConfigurationManager.AppSettings["MongoDBAvaliacaoApp"], ConfigurationManager.AppSettings["MongoCollectionNameAvaliacaoApp"]) { }

        public override Task<IHttpActionResult> Post(AvaliacaoApp entity)
        {
            _mediaClient = new MediaClient();
            _mediaClient.PostNotificacaoAvaliacaoApp(entity.Id, Request.RequestUri.AbsoluteUri.Split('.')[1], entity.Descricao, PlataformaUrl);

            return base.Post(entity);
        }
    }
}