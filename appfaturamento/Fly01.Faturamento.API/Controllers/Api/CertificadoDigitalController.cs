using Fly01.Core;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Reports;
using Fly01.Core.Rest;
using Fly01.Faturamento.BL;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [ODataRoutePrefix("certificadodigital")]
    public class CertificadoDigitalController : ApiPlataformaController<CertificadoDigital, CertificadoDigitalBL>
    {
        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<CertificadoDigital> model)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                if (!unitOfWork.CertificadoDigitalBL.All.Any())
                    return BadRequest("Nenhum certificado foi encontrado para esta plataforma.");

                if (model == null || key == default(Guid) || key == null)
                    return BadRequest(ModelState);

                var entity = Find(key);

                model.CopyChangedValues(entity);

                unitOfWork.CertificadoDigitalBL.ProcessEntity(entity);

                return await base.Put(entity.Id, model);
            }
        }

        public override async Task<IHttpActionResult> Post(CertificadoDigital entity)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    if (unitOfWork.CertificadoDigitalBL.All.Any())
                        return BadRequest("Já existe um certificado cadastrado para esta plataforma.");

                    entity = unitOfWork.CertificadoDigitalBL.ProcessEntity(entity);
                    return await base.Post(entity);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public override IHttpActionResult Get()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    var dadosEmpresa = RestHelper.ExecuteGetRequest<ManagerEmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{PlataformaUrl}");
                    var empresaCnpj = dadosEmpresa.CNPJ;
                    var empresaIE = dadosEmpresa.InscricaoEstadual;
                    var empresaUF = dadosEmpresa.Cidade != null ? (dadosEmpresa.Cidade.Estado != null ? dadosEmpresa.Cidade.Estado.Sigla : "") : "";

                    // TODO: Comparar com o certificado
                    // TODO: Excluir o certificado em faturamento.
                    //RestUtils.ExecuteDeleteRequest($"{urlFaturamentoApi}CertificadoDigital/{certificadoId}", "CertificadoDigital", null);

                    return base.Get();
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}