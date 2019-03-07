using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Financeiro.BL;
using Fly01.Financeiro.Models.ViewModel;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Web.Http;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/relatorioContaFinanceira")]
    public class RelatorioContaFinanceiraController : ApiBaseController
    {
        private Func<ContaFinanceira, ImprimirListContasVM> GetDisplayData(string tipoReport)
        {
            return x => new ImprimirListContasVM()
            {
                Id = x.Id,
                Descricao = x.Descricao,
                Categoria = x.Categoria.Descricao,
                CentroCusto = x.CentroCusto != null ? x.CentroCusto.Descricao : "",
                Status = x.StatusContaBancaria.ToString() == "EmAberto" ? "Em Aberto" : x.StatusContaBancaria.ToString() == "BaixadoParcialmente" ? "Baixado Parcialmente" : x.StatusContaBancaria.ToString(),
                Valor = x.ValorPrevisto,
                FormaPagamento = x.FormaPagamento != null ? x.FormaPagamento.Descricao : string.Empty,
                Fornecedor = x.Pessoa != null ? x.Pessoa.Nome : string.Empty,
                Cliente = x.Pessoa != null ? x.Pessoa.Nome : string.Empty,
                Vencimento = x.DataVencimento,
                Titulo = tipoReport == "ContaPagar" ? "Conta a pagar" : "Conta a receber",
                Numero = x.Numero,
                CondicaoParcelamento = x.CondicaoParcelamento.Descricao,
                Parcela = x.DescricaoParcela,
                TipoConta = tipoReport,
                Emissao = x.DataEmissao
            };
        }

        [HttpGet]
        public IHttpActionResult Get(DateTime? dataInicial,
                             DateTime? dataFinal,
                             DateTime? dataEmissaoInicial,
                             DateTime? dataEmissaoFinal,
                             Guid? pessoaId,
                             Guid? formaPagamentoId,
                             Guid? condicaoParcelamentoId,
                             Guid? categoriaId,
                             Guid? centroCustoId,
                             string tipoConta,
                             string descricao)
        {

            Func<ContaFinanceira, bool> filterPredicate = (x => (
                ((!dataInicial.HasValue) || (x.DataVencimento >= dataInicial)) &&
                ((!dataFinal.HasValue) || (x.DataVencimento <= dataFinal)) &&
                ((!dataEmissaoInicial.HasValue) || (x.DataEmissao >= dataEmissaoInicial)) &&
                ((!dataEmissaoFinal.HasValue) || (x.DataEmissao <= dataEmissaoFinal)) &&
                ((!pessoaId.HasValue) || (x.PessoaId == pessoaId)) &&
                ((!formaPagamentoId.HasValue) || (x.FormaPagamentoId == formaPagamentoId)) &&
                ((!condicaoParcelamentoId.HasValue) || (x.CondicaoParcelamentoId == condicaoParcelamentoId)) &&
                ((!categoriaId.HasValue) || (x.CategoriaId == categoriaId)) &&
                ((!(descricao != null)) || (x.Descricao == descricao)) &&
                ((!centroCustoId.HasValue) || (x.CentroCustoId == centroCustoId)))
            );

            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                if (tipoConta == "ContaPagar")
                {
                    List<ImprimirListContasVM> result = unitOfWork.ContaPagarBL
                       .AllIncluding(
                           x => x.FormaPagamento,
                           x => x.Pessoa,
                           x => x.CentroCusto,
                           x => x.CondicaoParcelamento,
                           x => x.Categoria
                       ).Where(filterPredicate)
                       .Take(2000)
                       .Select(GetDisplayData(tipoConta)).ToList();

                    return Ok(new { count = result.Count, value = result });
                }
                else
                {
                    List<ImprimirListContasVM> result = unitOfWork.ContaReceberBL
                    .AllIncluding(
                        x => x.FormaPagamento,
                        x => x.Pessoa,
                        x => x.CentroCusto,
                        x => x.CondicaoParcelamento,
                        x => x.Categoria
                    )
                    .Where(filterPredicate)
                    .Take(2000)
                    .Select(GetDisplayData(tipoConta)).ToList();

                    return Ok(new { count = result.Count, value = result });
                }
            }
        }
    }
}