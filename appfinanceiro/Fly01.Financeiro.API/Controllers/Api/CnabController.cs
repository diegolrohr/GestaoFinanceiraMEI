﻿using Fly01.Financeiro.BL;
using System;
using System.Web.Http;
using Fly01.Core.API;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/cnab")]
    public class CnabController : ApiBaseController
    {
        [HttpGet]
        [Route("imprimeBoleto")]
        public IHttpActionResult ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                return Ok(unitOfWork.CnabBL.GeraBoletos(contaReceberId, contaBancariaId, DateTime.Now, 0));
            }
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var data = unitOfWork.CnabBL.Get();
                return Ok(new { value = data });
            }
        }

    }
}