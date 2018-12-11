using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    public class Response
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        public int userId { get; set; }
        public string name { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string token { get; set; }
        public DateTime expire_at { get; set; }
        public List<int> stone_code_collection { get; set; }
        public List<string> permission_collection { get; set; }
        public string role { get; set; }
        public bool is_personified { get; set; }
        public DateTime created_at { get; set; }
    }


    [RoutePrefix("stone")]
   // [AllowAnonymous]
    public class StoneController : ApiBaseController
    {
        public IHttpActionResult Get()
        {
            return Ok("SUCESSO");
        }


        [HttpPost]
        public Response Post(AutenticacaoStone entity)
        {
            var url = "https://portalapi.stone.com.br/";
            var resource = "authenticate";
            //var resource = "stone/merchant/auth/login";
            //var url = "http://payments.bemacash.com.br/";
            try
            {

                var header = new Dictionary<string, string>()
                {
                    { "Content-Type", "application/json" }
                };

                var teste = RestHelper.ExecutePostRequest<Response>(url, resource, entity, null, header);
                return teste;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Não foi possível obter autenticação na stone. " + ex.Message);
            }
        }
    }
}