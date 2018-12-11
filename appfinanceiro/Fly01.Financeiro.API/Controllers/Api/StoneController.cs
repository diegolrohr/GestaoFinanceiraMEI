using Fly01.Core.API;
using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using System;
using System.Web.Http;
using RestSharp;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("stone")]
    public class StoneController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok("SUCESSO");
        }

        //[HttpPost]
        //public IHttpActionResult UserAutentication(AutenticacaoStone autenticacao)
        //{
        //    var url = "https://portalapi.stone.com.br/authenticate";
        //    try
        //    {
        //        var teste = "";
        //        RestClient client = new RestClient(url);
        //        RestRequest request = CreateJsonRequest(teste);
        //        //client.para

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException("Não foi possível obter autenticação na stone. " + ex.Message);
        //    }
        //}

        private static RestRequest CreateJsonRequest(string resource)
        {
            RestRequest request = new RestRequest();
            request.Resource = resource;
            request.RequestFormat = DataFormat.Json;
            return request;
        }
    }
}