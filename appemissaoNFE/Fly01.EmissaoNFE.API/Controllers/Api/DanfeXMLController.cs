using Fly01.EmissaoNFE.API.Model;
using Fly01.EmissaoNFE.BL;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;
using System;
using System.Web.Http;

namespace Fly01.EmissaoNFE.API.Controllers.Api
{
    [RoutePrefix("danfeXML")]
    public class DanfeXMLController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(DanfeVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                try
                {
                    var xml = entity.EntidadeAmbiente == TipoAmbiente.Homologacao ? DanfeXMLHelper.Homologacao(entity) : DanfeXMLHelper.Producao(entity);
                    //tenta recuperar invertendo o ambiente, pode ser que foi emitido em outro ambiente do atual configurado
                    //posteriormente começou a se salvar as configuração de cada transmissão
                    if (xml == null || (xml != null && string.IsNullOrEmpty(xml?.XML)))
                    {
                        xml = entity.EntidadeAmbiente == TipoAmbiente.Homologacao ? DanfeXMLHelper.Producao(entity) : DanfeXMLHelper.Homologacao(entity);
                    }

                    return Ok(xml);
                }
                catch (Exception ex)
                {
                    if (unitOfWork.EntidadeBL.TSSException(ex))
                    {
                        unitOfWork.EntidadeBL.EmissaoNFeException(ex, entity);
                    }

                    return InternalServerError(ex);
                }
            }
        }
    }

    public static class DanfeXMLHelper
    {
        public static XMLVM Producao(DanfeVM entity)
        {
            var response = new NFESBRAProd.NFESBRA().RETORNAFAIXA(AppDefault.Token, entity.Producao, entity.DanfeId, entity.DanfeId, "");

            var xml = new XMLVM()
            {
                XML = response.NOTAS != null ? response.NOTAS.Length > 0 ? response.NOTAS[0].NFE.XML : "" : ""
            };

            return xml;
        }

        public static XMLVM Homologacao(DanfeVM entity)
        {
            var response = new NFESBRA.NFESBRA().RETORNAFAIXA(AppDefault.Token, entity.Homologacao, entity.DanfeId, entity.DanfeId, "");

            var xml = new XMLVM()
            {
                XML = response.NOTAS != null ? response.NOTAS.Length > 0 ? response.NOTAS[0].NFE.XML : "" : ""
            };

            return xml;
        }
    }
}