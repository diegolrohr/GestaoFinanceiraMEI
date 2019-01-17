using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using System;
using System.Linq;
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

                foreach (var item in boletos.Data)
                {
                    try
                    {
                        //http://www.secretgeek.net/uri_enconding
                        var queryStringCodigo = new Dictionary<string, string>
                        {
                            { "urlBoleto", Uri.EscapeUriString(item.UrlBoleto)}
                        };
                        var response = RestHelper.ExecuteGetRequest<MinhaContaCodigoBarrasResponseVM>(AppDefaults.UrlApiGatewayMpn, "boletos/codigobarras", queryStringCodigo)?.Data;
                        item.CodigoBarras = response.CodigoBarras;
                        item.CodigoBarrasFormatado = response.CodigoBarrasFormatado;
                    }
                    catch (Exception)
                    {
                        item.CodigoBarras = string.Empty;
                        item.CodigoBarrasFormatado = string.Empty;
                    }
   
                }
                return Ok(boletos.Data.OrderBy(x => x.Vencimento));

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}