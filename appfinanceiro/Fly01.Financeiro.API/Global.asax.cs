using Fly01.Core.API.Application;
using Microsoft.OData.Edm;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using Fly01.Core;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.ServiceBus;
using System.Reflection;

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
            builder.EntitySet<ContaFinanceiraRenegociacao>("contafinanceirarenegociacao");
            builder.EntitySet<MovimentacaoFinanceira>("movimentacao");
            builder.EntitySet<TransferenciaFinanceira>("transferencia");
            builder.EntitySet<MovimentacaoFinanceiraPorCategoria>("receitaporcategoria");
            builder.EntitySet<MovimentacaoFinanceiraPorCategoria>("despesaporcategoria");
            builder.EntitySet<MovimentacaoFinanceiraPorCategoria>("movimentacaoporcategoria");
            builder.EntitySet<Pais>("pais");
            builder.EntitySet<ConfiguracaoPersonalizacao>("configuracaopersonalizacao");

            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }

        protected override string GetInstrumentationKeyAppInsights() => ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"];

        protected override Task RunServiceBusApps() => Task.Factory.StartNew(() =>
        {
            SetupEnvironment.Create(RabbitConfig.VirtualHostApps);
            SetupEnvironment.Create(RabbitConfig.VirtualHostIntegracao);

            new Consumer(Assembly.Load("Fly01.Financeiro.BL").GetType("Fly01.Financeiro.BL.UnitOfWork")).Consume();
        });
    }
}