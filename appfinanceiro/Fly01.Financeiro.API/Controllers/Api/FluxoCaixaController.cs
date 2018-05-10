using Fly01.Financeiro.BL;

using System;
using System.Web.Http;
using Fly01.Core.API;
using System.Linq;
using Fly01.Core.Helpers;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/fluxocaixa")]
    public class FluxoCaixaController : ApiBaseController
    {
        [HttpGet]
        [Route("saldos")]
        public IHttpActionResult GetSaldos()
        {
            //#1 Saldo de Todas as Contas (Consolidado) + AReceber e APagar (hoje)
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(new { value = unitOfWork.FluxoCaixaBL.GetSaldos() });
            }
        }

        [HttpGet]
        [Route("projecao")]
        public IHttpActionResult GetProjecao(DateTime dataInicial, DateTime dataFinal)
        {
            //#2 Projeções do Fluxo de Caixa
            if (dataInicial > dataFinal)
                return BadRequest("Data inicial não pode ser superior a data final.");

            //if ((DateTime.Now.Date > dataInicial.Date) || (DateTime.Now.Date > dataFinal.Date))
            //    return BadRequest("O período informado deve ser superior ao dia de hoje.");

            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(new { value = unitOfWork.FluxoCaixaBL.GetProjecao(dataInicial, dataFinal) });
            }
        }

        [HttpGet]
        [Route("projecaodetalhe")]
        public IHttpActionResult GetProjecaoDetalhe(DateTime dataInicial, DateTime dataFinal, int pageNo, int pageSize)
        {
            //#3 Projeções do Fluxo de Caixa paginação
            if (dataInicial > dataFinal)
                return BadRequest("Data inicial não pode ser superior a data final.");

            //if ((DateTime.Now.Date > dataInicial.Date) || (DateTime.Now.Date > dataFinal.Date))
            //    return BadRequest("O período informado deve ser superior ao dia de hoje.");

            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var projecao = unitOfWork.FluxoCaixaBL.GetProjecao(dataInicial, dataFinal);
                int skipRecords = (pageNo - 1) * pageSize;

                return Ok(new PagedResult<FluxoCaixaProjecao>(projecao.Skip(skipRecords).Take(pageSize), pageNo, pageSize, projecao.Count));
            }
        }

        [HttpGet]
        [Route("projecaoNextDays")]
        public IHttpActionResult GetProjecaoNextDays(DateTime dataInicial, DateTime dataFinal)
        {
            if (dataInicial.Date > dataFinal.Date)
                return BadRequest("Data inicial não pode ser superior a data final.");

            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.FluxoCaixaBL.GetAllNextDays(dataInicial, dataFinal));
            }
        }
    }
}