using Fly01.Financeiro.BL;
using System;
using System.Web.Http;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/cnab")]
    public class CnabController : ApiBaseController
    {
        [HttpGet]
        [Route("ImprimeBoleto")]
        public IHttpActionResult ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var data = unitOfWork.CnabBL.GetDadosBoleto(contaReceberId, contaBancariaId);

                return Ok(data);
            }
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var data = unitOfWork.CnabBL.GetCnab();

                return Ok(new { value = data });
            }
        }

        [HttpGet]
        [Route("GetCnab")]
        public IHttpActionResult GetCnab(Guid Id)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var data = unitOfWork.CnabBL.GetCnab(Id);

                return Ok(data);
            }
        }

        [HttpGet]
        [Route("GetContasReceberArquivo")]
        public IHttpActionResult GetContasReceber(Guid IdArquivoRemessa)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var data = unitOfWork.CnabBL.GetContasReceberArquivo(IdArquivoRemessa);

                return Ok(new { value = data });
            }
        }

        [HttpPost]
        [ActionName("Post")]
        public async Task<IHttpActionResult> PostAsync(Cnab entity)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    unitOfWork.CnabBL.Insert(entity);
                    await unitOfWork.Save();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        [HttpPut]
        [Route("AtualizaIdArquivoRemessa")]
        public async Task<IHttpActionResult> AtualizaIdArquivoRemessa(List<Guid> ids, Guid idArquivoRemessa)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                foreach (var id in ids)
                {
                    var model = unitOfWork.CnabBL.Find(id);
                    model.ArquivoRemessaId = idArquivoRemessa;

                    unitOfWork.CnabBL.Update(model);

                    await unitOfWork.Save();
                }

                return Ok();
            }
        }
    }
}