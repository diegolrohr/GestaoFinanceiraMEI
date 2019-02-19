using Fly01.Core.API;
using System.Web.Http;
using Fly01.Financeiro.BL;
using System;
using System.Linq;
using System.Collections.Generic;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/relatorioContaFinanceira")]
    public class RelatorioContaFinanceiraController : ApiBaseController
    {
        [HttpGet]
        public IHttpActionResult Get(DateTime? dataInicial,
                                     DateTime? dataFinal,
                                     DateTime? dataEmissaoInicial,
                                     DateTime? dataEmissaoFinal,
                                     Guid? clienteId,
                                     Guid? formaPagamentoId,
                                     Guid? condicaoParcelamentoId,
                                     Guid? categoriaId,
                                     Guid? centroCustoId,
                                     string tipoConta)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                dynamic result;

                if (tipoConta == "ContaPagar")
                {
                     result = unitOfWork.ContaPagarBL.All.Where(x =>
                        (dataInicial == null || x.DataVencimento >= dataInicial) &&  
                        ( dataFinal == null || x.DataVencimento <= dataFinal) &&
                        (dataEmissaoInicial == null || x.DataEmissao >= dataEmissaoInicial ) &&
                        (dataEmissaoFinal == null || x.DataEmissao <= dataEmissaoFinal)&&
                        (clienteId == null || x.PessoaId == clienteId) &&
                        (formaPagamentoId  == null || x.FormaPagamentoId == formaPagamentoId) &&
                        (condicaoParcelamentoId == null || x.CondicaoParcelamentoId == condicaoParcelamentoId) &&
                        (categoriaId  == null || x.CategoriaId == categoriaId) &&
                        (centroCustoId == null || x.CentroCustoId == centroCustoId)).ToList();
                }
                else
                {
                    result = unitOfWork.ContaReceberBL.All.Where(x =>
                        (dataInicial == null || x.DataVencimento >= dataInicial) &&
                        (dataFinal == null || x.DataVencimento <= dataFinal) &&
                        (dataEmissaoInicial == null || x.DataEmissao >= dataEmissaoInicial) &&
                        (dataEmissaoFinal == null || x.DataEmissao <= dataEmissaoFinal) &&
                        (clienteId == null || x.PessoaId == clienteId) &&
                        (formaPagamentoId == null || x.FormaPagamentoId == formaPagamentoId) &&
                        (condicaoParcelamentoId == null || x.CondicaoParcelamentoId == condicaoParcelamentoId) &&
                        (categoriaId == null || x.CategoriaId == categoriaId) &&
                        (centroCustoId == null || x.CentroCustoId == centroCustoId)).ToList();
                }

                return Ok(new { value = result });
            }
        }
    }
}