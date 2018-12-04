﻿using System;
using System.Linq;
using System.Web.Http;
using Fly01.Financeiro.BL;
using Fly01.Core.API;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("contapagarperiodo")]
    public class ContaPagarPeriodoController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime dataInicial, DateTime dataFinal, bool ignoraExclusao)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var contasInclusao = unitOfWork.ContaPagarBL.AllWithInactiveIncluding(
                            x => x.Categoria,
                            x => x.FormaPagamento
                        ).Where(x => x.DataInclusao >= dataInicial && x.DataInclusao <= dataFinal && x.ValorPago > 0);

                var contasEdicao = unitOfWork.ContaPagarBL.AllWithInactiveIncluding(
                            x => x.Categoria,
                            x => x.FormaPagamento
                        ).Where(x => x.DataAlteracao >= dataInicial && x.DataAlteracao <= dataFinal && x.ValorPago > 0);

                var contasExclusao = unitOfWork.ContaPagarBL.AllWithInactiveIncluding(
                            x => x.Categoria,
                            x => x.FormaPagamento
                        ).Where(x => !ignoraExclusao && (x.DataExclusao >= dataInicial && x.DataExclusao <= dataFinal));

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