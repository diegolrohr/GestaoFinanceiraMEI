using Fly01.Core.Notifications;
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
                                
                var boletos = RestHelper.ExecuteGetRequest<MinhaContaResponseVM>(AppDefaults.UrlApiGatewayMpn, "boletos/consulta", queryString);

                return Ok(boletos.Data);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}