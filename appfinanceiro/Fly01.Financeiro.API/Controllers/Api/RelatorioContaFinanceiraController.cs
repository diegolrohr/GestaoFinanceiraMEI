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
        private Func<ContaFinanceira, ImprimirListContasVM> GetDisplayData(string tipoConta, DateTime? dataInicial, DateTime? dataFinal, DateTime? dataEmissaoInicial,
            DateTime? dataEmissaoFinal, string fornecedor, string formaPagamento, string condicaoParcelamento, string categoria, string descricao)

        {
            var filtro = "";
            filtro = descricao != null ? "Descrição: " + descricao + " - " : filtro;
            filtro = fornecedor != "" ? filtro + "Fornecedor: " + fornecedor + " - " : filtro;
            filtro = formaPagamento != "" ? filtro + "Forma Pagamento: " + formaPagamento + " - " : filtro;
            filtro = dataEmissaoInicial.HasValue ? filtro + "Data Emissão Inicial: " + dataEmissaoInicial.ToString().Substring(0,10) + " - " : filtro;
            filtro = dataEmissaoFinal.HasValue ? filtro + "Data Emissão Final: " + dataEmissaoFinal.ToString().Substring(0, 10) + " - " : filtro;
            filtro = dataInicial.HasValue ? filtro + "Data Vencimento Inicial: " + dataInicial.ToString().Substring(0, 10) + " - " : filtro;
            filtro = dataFinal.HasValue ? filtro + "Data Vencimento Final: " + dataFinal.ToString().Substring(0, 10) + " - " : filtro;
            filtro = condicaoParcelamento != "" ? filtro + "Condição Parcelamento: " + condicaoParcelamento + " - " : filtro;
            filtro = categoria != "" ? filtro + "Categoria: " + categoria + " - " : filtro;

            return x => new ImprimirListContasVM()
            {
                Id = x.Id,
                Descricao = x.Descricao.Length > 15 ? x.Descricao.Substring(0, 15) : x.Descricao,
                Categoria = x.Categoria.Descricao.Length > 15 ? x.Categoria.Descricao.Substring(0, 10) : x.Categoria.Descricao,
                Status = x.StatusContaBancaria.ToString() == "EmAberto" ? "Em Aberto" : x.StatusContaBancaria.ToString() == "BaixadoParcialmente" ? "Baixado Parcialmente" : x.StatusContaBancaria.ToString(),
                Valor = x.ValorPrevisto,
                FormaPagamento = x.FormaPagamento == null ? string.Empty : x.FormaPagamento.Descricao.Length > 13 ? x.FormaPagamento.Descricao.Substring(0, 13) : x.FormaPagamento.Descricao,
                Fornecedor = x.Pessoa == null ? string.Empty : x.Pessoa.Nome.Length > 12 ? x.Pessoa.Nome.Substring(0, 12) : x.Pessoa.Nome,
                Cliente = x.Pessoa != null ? x.Pessoa.Nome : string.Empty,
                Vencimento = x.DataVencimento,
                Titulo = tipoConta == "ContaPagar" ? "Contas a Pagar" : "Contas a Receber",
                Numero = x.Numero,
                CondicaoParcelamento = x.CondicaoParcelamento.Descricao.Length > 15 ? x.CondicaoParcelamento.Descricao.Substring(0, 15) : x.CondicaoParcelamento.Descricao,
                Parcela = x.DescricaoParcela.Length > 7 ? x.DescricaoParcela.Substring(0, 7) : x.DescricaoParcela,
                TipoConta = tipoConta,
                Emissao = x.DataEmissao,
                Filtro = filtro
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
                ((!(descricao != null)) || (x.Descricao == descricao)))
            );

            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                try
                {

                    var fornecedor = "";
                    var formaPagamento = "";
                    var condicaoParcelamento = "";
                    var categoria = "";

                    if (pessoaId.HasValue)
                        fornecedor = unitOfWork.PessoaBL.All.Where(x => x.Id == pessoaId).FirstOrDefault().Nome.ToString();
                    if (formaPagamentoId.HasValue)
                        formaPagamento = unitOfWork.FormaPagamentoBL.All.Where(x => x.Id == formaPagamentoId).FirstOrDefault().Descricao.ToString();
                    if (condicaoParcelamentoId.HasValue)
                        condicaoParcelamento = unitOfWork.CondicaoParcelamentoBL.All.Where(x => x.Id == condicaoParcelamentoId).FirstOrDefault().Descricao.ToString();
                    if (categoriaId.HasValue)
                        categoria = unitOfWork.CategoriaBL.All.Where(x => x.Id == categoriaId).FirstOrDefault().Descricao.ToString();                    

                    if (tipoConta == "ContaPagar")
                    {
                        List<ImprimirListContasVM> result = unitOfWork.ContaPagarBL
                           .AllIncluding(
                               x => x.FormaPagamento,
                               x => x.Pessoa,
                               x => x.CondicaoParcelamento,
                               x => x.Categoria
                           ).Where(filterPredicate)
                           .Take(2000)
                           .Select(GetDisplayData(tipoConta, dataInicial, dataFinal, dataEmissaoInicial, dataEmissaoFinal,
                           fornecedor, formaPagamento, condicaoParcelamento, categoria, descricao)).ToList();

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
                            x => x.CondicaoParcelamento,
                            x => x.Categoria
                        )
                        .Where(filterPredicate)
                        .Take(2000)
                        .Select(GetDisplayData(tipoConta, dataInicial, dataFinal, dataEmissaoInicial, dataEmissaoFinal,
                           fornecedor, formaPagamento, condicaoParcelamento, categoria, descricao)).ToList();

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