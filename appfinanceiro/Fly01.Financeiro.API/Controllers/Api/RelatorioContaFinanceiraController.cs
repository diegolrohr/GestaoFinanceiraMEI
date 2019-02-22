using Fly01.Core.API;
using System.Web.Http;
using Fly01.Financeiro.BL;
using System;
using System.Linq;
using System.Collections.Generic;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Financeiro.Models.ViewModel;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;

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
                    result = unitOfWork.ContaPagarBL
                       .AllIncluding(
                           x => x.FormaPagamento,
                           x => x.Pessoa,
                           x => x.CentroCusto,
                           x => x.CondicaoParcelamento,
                           x => x.Categoria
                       )
                       .Where(x =>
                           (dataInicial == null || x.DataVencimento >= dataInicial) &&
                           (dataFinal == null || x.DataVencimento <= dataFinal) &&
                           (dataEmissaoInicial == null || x.DataEmissao >= dataEmissaoInicial) &&
                           (dataEmissaoFinal == null || x.DataEmissao <= dataEmissaoFinal) &&
                           (clienteId == null || x.PessoaId == clienteId) &&
                           (formaPagamentoId == null || x.FormaPagamentoId == formaPagamentoId) &&
                           (condicaoParcelamentoId == null || x.CondicaoParcelamentoId == condicaoParcelamentoId) &&
                           (categoriaId == null || x.CategoriaId == categoriaId) &&
                           (centroCustoId == null || x.CentroCustoId == centroCustoId)
                       )
                       .Select(x => new ImprimirListContasVM()
                       {
                           Id = x.Id,
                           Descricao = x.Descricao,
                           Categoria = x.Categoria.Descricao,
                           CentroCusto = x.CentroCusto != null ? x.CentroCusto.Descricao : "",
                           //Status = EnumHelper.GetValue(typeof(StatusContaBancaria), x.StatusContaBancaria.ToString()),
                           Status = x.StatusContaBancaria.ToString() == "EmAberto" ? "Em Aberto" : x.StatusContaBancaria.ToString() == "BaixadoParcialmente" ? "Baixado Parcialmente" : x.StatusContaBancaria.ToString(),
                           Valor = x.ValorPrevisto.ToString(),
                           FormaPagamento = x.FormaPagamento != null ? x.FormaPagamento.Descricao : string.Empty,
                           Fornecedor = x.Pessoa != null ? x.Pessoa.Nome : string.Empty,
                           Cliente = x.Pessoa != null ? x.Pessoa.Nome : string.Empty,
                           Vencimento = x.DataVencimento,
                           Titulo = "Contas a Pagar",
                           Numero = x.Numero,
                           CondicaoParcelamento = x.CondicaoParcelamento.Descricao,
                           Parcela = x.DescricaoParcela,
                           TipoConta = "ContaPagar"
                       }).ToList();
                }
                else
                {
                    result = unitOfWork.ContaReceberBL
                        .AllIncluding(
                            x => x.FormaPagamento,
                            x => x.Pessoa,
                            x => x.CentroCusto,
                            x => x.CondicaoParcelamento,
                            x => x.Categoria
                        )
                        .Where(x =>
                            (dataInicial == null || x.DataVencimento >= dataInicial) &&
                            (dataFinal == null || x.DataVencimento <= dataFinal) &&
                            (dataEmissaoInicial == null || x.DataEmissao >= dataEmissaoInicial) &&
                            (dataEmissaoFinal == null || x.DataEmissao <= dataEmissaoFinal) &&
                            (clienteId == null || x.PessoaId == clienteId) &&
                            (formaPagamentoId == null || x.FormaPagamentoId == formaPagamentoId) &&
                            (condicaoParcelamentoId == null || x.CondicaoParcelamentoId == condicaoParcelamentoId) &&
                            (categoriaId == null || x.CategoriaId == categoriaId) &&
                            (centroCustoId == null || x.CentroCustoId == centroCustoId)
                        )
                        .Select(x => new ImprimirListContasVM()
                        {
                            Id = x.Id,
                            Descricao = x.Descricao,
                            Categoria = x.Categoria.Descricao,
                            CentroCusto = x.CentroCusto != null ? x.CentroCusto.Descricao : "",
                            Status = x.StatusContaBancaria.ToString() == "EmAberto" ? "Em Aberto" : x.StatusContaBancaria.ToString() == "BaixadoParcialmente" ? "Baixado Parcialmente" : x.StatusContaBancaria.ToString(),
                            Valor = x.ValorPrevisto.ToString(),
                            FormaPagamento = x.FormaPagamento != null ? x.FormaPagamento.Descricao : string.Empty,
                            Fornecedor = x.Pessoa != null ? x.Pessoa.Nome : string.Empty,
                            Cliente = x.Pessoa != null ? x.Pessoa.Nome : string.Empty,
                            Vencimento = x.DataVencimento,
                            Titulo = "Contas a Receber",
                            Numero = x.Numero,
                            CondicaoParcelamento = x.CondicaoParcelamento.Descricao,
                            Parcela = x.DescricaoParcela,
                            TipoConta = "ContaReceber"
                        }).ToList();
                }

                return Ok(new { value = result });
            }
        }
    }
}