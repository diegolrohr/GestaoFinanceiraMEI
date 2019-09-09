using Fly01.Core.API.Application;
using Microsoft.OData.Edm;
using System.Web.OData.Builder;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API
{
    public class WebApiApplication : GlobalWebAPIApplication
    {
        protected override IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder
            {
                ContainerName = "DefaultContainer"
            };

            builder.EntitySet<Pessoa>("pessoa");
            builder.EntitySet<ContaBancaria>("contabancaria");
            builder.EntitySet<Categoria>("categoria");
            builder.EntitySet<Banco>("banco");
            builder.EntitySet<ContaPagar>("contapagar");
            builder.EntitySet<ContaReceber>("contareceber");
            builder.EntitySet<Estado>("estado");
            builder.EntitySet<Cidade>("cidade");
            builder.EntitySet<CondicaoParcelamento>("condicaoparcelamento");
            builder.EntitySet<ContaFinanceiraBaixa>("contafinanceirabaixa");
            builder.EntitySet<ContaFinanceiraBaixaMultipla>("contafinanceirabaixamultipla");
            builder.EntitySet<ConciliacaoBancaria>("conciliacaobancaria");
            builder.EntitySet<ConciliacaoBancariaItem>("conciliacaobancariaitem");
            builder.EntitySet<ConciliacaoBancariaItemContaFinanceira>("conciliacaobancariaitemcontafinanceira");
            builder.EntitySet<ConciliacaoBancariaTransacao>("conciliacaobancariatransacao");
            builder.EntitySet<ConciliacaoBancariaItem>("conciliacaobancariabuscarexistentes");
            builder.EntitySet<FormaPagamento>("formapagamento");
            builder.EntitySet<MovimentacaoFinanceira>("movimentacao");
            builder.EntitySet<TransferenciaFinanceira>("transferencia");
            builder.EntitySet<MovimentacaoFinanceiraPorCategoria>("receitaporcategoria");
            builder.EntitySet<MovimentacaoFinanceiraPorCategoria>("despesaporcategoria");
            builder.EntitySet<MovimentacaoFinanceiraPorCategoria>("movimentacaoporcategoria");

            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }
    }
}