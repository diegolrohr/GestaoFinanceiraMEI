using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Fly01.Core.API
{
    public class MinhaContaBaseController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(MinhaContaConfiguracaoVM entity)
        {
            try
            {
                var queryString = new Dictionary<string, string>
                    {
                        { "codigoMaxime", entity.CodigoMaxime},
                        { "vencimentoInicial", entity.VencimentoInicial},
                        { "vencimentoFinal", entity.VencimentoFinal},
                        { "posicao", entity.Posicao}
                    };

                var url = "http://gateway-homolog.totvsmpn.com.br/api/boletos/";
                var autenticacao = RestHelper.ExecuteGetRequest<MinhaContaResponse>(url, "consulta", queryString);

                return Ok(autenticacao.Data);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}