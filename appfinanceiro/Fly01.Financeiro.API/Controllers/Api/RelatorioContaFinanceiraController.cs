using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Financeiro.BL;
using Fly01.Financeiro.Models.ViewModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using Fly01.Core.Notifications;

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
                Descricao = x.Descricao.Length > 15 ? x.Descricao.Substring(1,15) : x.Descricao,
                Categoria = x.Categoria.Descricao.Length > 15 ? x.Categoria.Descricao.Substring(1, 10) : x.Categoria.Descricao,
                CentroCusto = x.CentroCusto == null ? "" : x.CentroCusto.Descricao.Length > 13 ? x.CentroCusto.Descricao.Substring(1,13) : x.CentroCusto.Descricao,
                Status = x.StatusContaBancaria.ToString() == "EmAberto" ? "Em Aberto" : x.StatusContaBancaria.ToString() == "BaixadoParcialmente" ? "Baixado Parcialmente" : x.StatusContaBancaria.ToString(),
                Valor = x.ValorPrevisto,
                FormaPagamento = x.FormaPagamento == null ?  string.Empty : x.FormaPagamento.Descricao.Length > 13 ? x.FormaPagamento.Descricao.Substring(1,13) : x.FormaPagamento.Descricao,
                Fornecedor = x.Pessoa == null ? string.Empty : x.Pessoa.Nome.Length > 12 ? x.Pessoa.Nome.Substring(1,12) : x.Pessoa.Nome,
                Cliente = x.Pessoa != null ? x.Pessoa.Nome : string.Empty,
                Vencimento = x.DataVencimento,
                Titulo = tipoReport == "ContaPagar" ? "Contas a Pagar" : "Contas a Receber",
                Numero = x.Numero,
                CondicaoParcelamento = x.CondicaoParcelamento.Descricao.Length > 15 ? x.CondicaoParcelamento.Descricao.Substring(1,15) : x.CondicaoParcelamento.Descricao,
                Parcela = x.DescricaoParcela.Length > 7 ? x.DescricaoParcela.Substring(1,7) : x.DescricaoParcela,
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
                try
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

                        if (result.Count == 0)
                        {
                            throw new BusinessException("Não existem contas à serem impressas. Por gentileza revise seus filtros e imprima novamente.");
                        }

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

                        if (result.Count == 0)
                        {
                            throw new BusinessException("Não existem contas à serem impressas. Por gentileza revise seus filtros e imprima novamente.");
                        }

                        return Ok(new { count = result.Count, value = result });
                    }
                }
                catch (Exception ex)
                {
                    throw new BusinessException(ex.Message);
                }
            }
        }
    }
}