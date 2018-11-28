﻿using System;
using System.Linq;
using System.Web.Http;
using Fly01.Financeiro.BL;
using Fly01.Core.API;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("contareceberperiodo")]
    public class ContaReceberPeriodoController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime dataInicial, DateTime dataFinal)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var contasInclusao = unitOfWork.ContaReceberBL.AllWithInactiveIncluding(
                            x => x.Categoria,
                            x => x.FormaPagamento
                        ).Where(x => x.DataInclusao >= dataInicial && x.DataInclusao <= dataFinal && x.ValorPago > 0);

                var contasEdicao = unitOfWork.ContaReceberBL.AllWithInactiveIncluding(
                            x => x.Categoria,
                            x => x.FormaPagamento
                        ).Where(x => x.DataAlteracao >= dataInicial && x.DataAlteracao <= dataFinal && x.ValorPago > 0);

                var contasExclusao = unitOfWork.ContaReceberBL.AllWithInactiveIncluding(
                            x => x.Categoria,
                            x => x.FormaPagamento
                        ).Where(x => x.DataExclusao >= dataInicial && x.DataExclusao <= dataFinal);

                return Ok(
                    new
                    {
                        value = contasInclusao.Union(contasEdicao).Union(contasExclusao).ToList()
                    }
                );
            }
        }
    }
}