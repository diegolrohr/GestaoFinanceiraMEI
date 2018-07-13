using System;
using Fly01.Core.API.Application;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Financeiro.BL;
using Microsoft.OData.Edm;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using Fly01.Core;

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
            builder.EntitySet<Arquivo>("arquivo");
            builder.EntitySet<ContaBancaria>("contabancaria");
            builder.EntitySet<Feriado>("feriado");
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
            builder.EntitySet<ConfiguracaoNotificacaoFinanceiro>("configuracaonotificacao");
            builder.EntityType<ConfiguracaoNotificacaoFinanceiro>().Property(c => c.HoraEnvio).AsTimeOfDay();
            builder.EntitySet<Cnab>("cnab");
            builder.EntitySet<ArquivoRemessa>("arquivoremessa");

            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }

        protected override string GetInstrumentationKeyAppInsights() => ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"];

        protected override Task RunServiceBus() => Task.Factory.StartNew(() => new ServiceBusBL());
        protected override Task RunServiceBus2() => Task.Factory.StartNew(() => new ServiceBusBL());
        protected override Task RunServiceBus3() => Task.Factory.StartNew(() => new ServiceBusBL());
        protected override Task RunServiceBus4() => Task.Factory.StartNew(() => new ServiceBusBL());
        protected override Task RunServiceBus5() => Task.Factory.StartNew(() => new ServiceBusBL());

        protected override void SetAppDefaults()
        {
            AppDefaults.UrlGateway = ConfigurationManager.AppSettings["UrlGateway"];

            base.SetAppDefaults();
        }
    }
}