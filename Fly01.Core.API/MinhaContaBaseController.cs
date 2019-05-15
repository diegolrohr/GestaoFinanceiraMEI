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
                if((entity.VencimentoFinal.Date - entity.VencimentoInicial.Date).Days > 365)
                {
                    throw new BusinessException("Período máximo de 1 ano para pesquisa. Altere o período informado.");
                }

                var queryString = new Dictionary<string, string>
                    {
                        { "codigoMaxime", entity.CodigoMaxime},
                        { "vencimentoInicial", entity.VencimentoInicial.ToString("yyyy-MM-dd")},
                        { "vencimentoFinal", entity.VencimentoFinal.ToString("yyyy-MM-dd")},
                        { "posicao", entity.Posicao}
                    };

                var boletos = RestHelper.ExecuteGetRequest<MinhaContaResponseVM>(AppDefaults.UrlGatewayNew, "boletos/consulta", queryString);
                //var boletos = new MinhaContaResponseVM();

                return Ok(boletos.Data.OrderBy(x => x.Vencimento));

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}