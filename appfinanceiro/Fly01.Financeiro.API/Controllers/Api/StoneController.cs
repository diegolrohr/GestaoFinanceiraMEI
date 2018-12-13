using Fly01.Core;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/stone")]
    public class StoneController : ApiBaseController
    {
        public Dictionary<string, string> GetDefaultHeader()
        {
            var header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            return header;
        }

        public void AddAuthorizationHeader(Dictionary<string, string> header, string token)
        {
            header.Add("Authorization", String.Format("Bearer {0}", token));
        }

        public void AddStoneCodeHeader(Dictionary<string, string> header)
        {
            ManagerEmpresaVM response = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            header.Add("StoneCode", response?.StoneCode);
        }

        public IHttpActionResult Get()
        {
            return Ok("SUCESSO");
        }

        [HttpPost]
        [Route("token")]
        public IHttpActionResult GetToken(AutenticacaoStone entity)
        {
            var resource = "authenticate";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var authenticate = RestHelper.ExecutePostRequest<ResponseAutenticacaoStone>(AppDefaults.UrlStone, resource, entity, null, GetDefaultHeader());
                return Ok(new { token = authenticate.Token });
            }
            catch (Exception ex)
            {
                throw new BusinessException("Não foi possível obter autenticação na stone. " + ex.Message);
            }
        }

        [HttpPost]
        [Route("validartoken")]
        public IHttpActionResult ValidarToken(ResponseAutenticacaoStone entity)
        {

            var resource = "authenticate/validate";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var authenticate = RestHelper.ExecutePostRequest<JObject>(AppDefaults.UrlStone, resource, entity, null, GetDefaultHeader());
                return Ok(new { success = true });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}