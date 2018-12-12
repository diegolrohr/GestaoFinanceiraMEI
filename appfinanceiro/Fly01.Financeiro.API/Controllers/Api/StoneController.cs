﻿using Fly01.Core;
using Fly01.Core.API;
using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("stone")]
    public class StoneController : ApiBaseController
    {
        public Dictionary<string, string> GetDefaultHeader()
        {
            var header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            return header;
        }

        public Dictionary<string, string> AddAuthorizationHeader(Dictionary<string, string> header)
        {
            //TODO token
            header.Add("Authorization", String.Format("Bearer {0}", "StoneToken"));
            return header;
        }

        public Dictionary<string, string> AddStoneCodeHeader(Dictionary<string, string> header)
        {
            ManagerEmpresaVM response = ApiEmpresaManager.GetEmpresa(PlataformaUrl);

            header.Add("StoneCode", response?.StoneCode);
            return header;
        }


        public IHttpActionResult Get()
        {
            return Ok("SUCESSO");
        }

        [HttpPost]
        [Route("token")]
        public string GetToken(AutenticacaoStone entity)
        {
            var resource = "authenticate";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var authenticate = RestHelper.ExecutePostRequest<ResponseAutenticacaoStone>(AppDefaults.UrlStone, resource, entity, null, GetDefaultHeader());
                return authenticate.Token;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Não foi possível obter autenticação na stone. " + ex.Message);
            }
        }
    }
}