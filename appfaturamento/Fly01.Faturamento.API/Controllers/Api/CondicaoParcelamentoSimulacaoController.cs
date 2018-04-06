﻿using System;
using System.Web.Http;
using Fly01.Faturamento.BL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.API;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("condicaoparcelamentosimulacao")]
    public class CondicaoParcelamentoSimulacaoController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult Post(CondicaoParcelamentoSimulacao condicaoVM)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                if (condicaoVM.DataReferencia == null)
                    condicaoVM.DataReferencia = DateTime.Now;

                var condicao = new CondicaoParcelamento()
                {
                    CondicoesParcelamento = condicaoVM.CondicoesParcelamento,
                    QtdParcelas = int.Parse(condicaoVM.QtdParcelas.ToString())
                };
                
                return Ok( new { value = unitOfWork.CondicaoParcelamentoBL.GetPrestacoes(condicao, condicaoVM.DataReferencia, condicaoVM.ValorReferencia) } );
            }
        }
    }
}
