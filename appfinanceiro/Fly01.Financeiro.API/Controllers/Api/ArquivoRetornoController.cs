using Fly01.Core.API;
using System.Web.Http;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Financeiro.BL;
using Fly01.Core.Rest;
using System.Collections.Generic;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/arquivoRetorno")]
    public class ArquivoRetornoController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(ArquivoRetornoCnab entity)
        {
            var cnabs = new List<Cnab>();

            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var dadosCedente = unitOfWork.CnabBL.GetDadosCedente(entity.ContaBancariaId);
                var dadosEmpresa = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
                cnabs = ArquivoRetornoBL.ImportarArquivoRetorno(entity, dadosCedente, unitOfWork.CnabBL, dadosEmpresa);
            }
            return Ok(cnabs);
        }
    }
}