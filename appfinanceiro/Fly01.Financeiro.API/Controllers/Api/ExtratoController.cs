using Fly01.Financeiro.BL;
using Fly01.Core.Helpers;
using System;
using System.Web.Http;
using Fly01.Core.API;
using Fly01.Financeiro.Domain.Entities;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/extrato")]
    public class ExtratoController : ApiBaseController
    {
        [HttpGet]
        [Route("saldos")]
        public IHttpActionResult GetSaldos()
        {
            //#1 (Lista de Contas com saldos)
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(new { value = unitOfWork.ExtratoBL.GetSaldos() });
            }
        }

        [HttpGet]
        [Route("historicosaldos")]
        public IHttpActionResult GetHistoricoSaldos(DateTime dataInicial, DateTime dataFinal, Guid? contaBancariaId)
        {
            //#2 (Gráfico de acompanhamento de saldos)
            if(dataInicial > dataFinal)
                return BadRequest("Data inicial não pode ser superior a data final.");
            
            if ((dataInicial > DateTime.Now.Date) || (dataFinal > DateTime.Now.Date))
                return BadRequest("O período informado não pode ser superior ao dia de hoje.");
            
            if ((dataFinal - dataInicial).TotalDays > 60)
                return BadRequest("O período informado não deve ultrapassar 60 dias");

            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(new { value = unitOfWork.ExtratoBL.GetHistoricoSaldos(dataInicial, dataFinal, contaBancariaId) });
            }
        }

        [HttpGet]
        [Route("extratodetalhe")]
        public IHttpActionResult GetExtratoDetalhe(DateTime dataInicial, DateTime dataFinal, Guid? contaBancariaId, int pageNo = 1, int pageSize = 20)
        {
            //#3 (Grid com o detalhamento de contas do extrato)
            if (dataInicial > dataFinal)
                return BadRequest("Data inicial não pode ser superior a data final.");

            if ((dataInicial > DateTime.Now.Date) || (dataFinal > DateTime.Now.Date))
                return BadRequest("O período informado não pode ser superior ao dia de hoje.");

            if ((dataFinal - dataInicial).TotalDays > 60)
                return BadRequest("O período informado não deve ultrapassar 60 dias");

            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                int skipRecords = (pageNo - 1) * pageSize;

                int totalRecords = unitOfWork.ExtratoBL.GetExtratoDetalheCount(dataInicial, dataFinal, contaBancariaId);
                var items = unitOfWork.ExtratoBL.GetExtratoDetalhe(dataInicial, dataFinal, contaBancariaId, skipRecords, pageSize);

                return Ok(new PagedResult<ExtratoDetalhe>(items, pageNo, pageSize, totalRecords));
            }
        }
    }
}