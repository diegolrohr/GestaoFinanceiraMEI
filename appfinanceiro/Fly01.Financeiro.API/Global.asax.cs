﻿using Fly01.Core.API.Application;
using Fly01.Financeiro.BL;
using Fly01.Financeiro.Domain.Entities;
using Microsoft.OData.Edm;
using System.Threading.Tasks;
using System.Web.OData.Builder;

namespace Fly01.Financeiro.API
{
    public class WebApiApplication : GlobalWebAPIApplication
    {
        protected override IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.ContainerName = "DefaultContainer";
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
            builder.EntitySet<ConciliacaoBancaria>("conciliacaobancaria");
            builder.EntitySet<ConciliacaoBancariaItem>("conciliacaobancariaitem");
            builder.EntitySet<ConciliacaoBancariaItemContaFinanceira>("conciliacaobancariaitemcontafinanceira");
            builder.EntitySet<ConciliacaoBancariaTransacao>("conciliacaobancariatransacao");
            builder.EntitySet<ConciliacaoBancariaItem>("conciliacaobancariabuscarexistentes");
            builder.EntitySet<FormaPagamento>("formapagamento");
            builder.EntitySet<ContaFinanceiraRenegociacao>("contafinanceirarenegociacao");
            builder.EntitySet<Movimentacao>("movimentacao");
            builder.EntitySet<Transferencia>("transferencia");
            builder.EntitySet<MovimentacaoPorCategoria>("receitaporcategoria");
            builder.EntitySet<MovimentacaoPorCategoria>("despesaporcategoria");
            builder.EntitySet<MovimentacaoPorCategoria>("movimentacaoporcategoria");
            builder.EntitySet<ConfiguracaoNotificacao>("configuracaonotificacao");

            EntityTypeConfiguration<ConfiguracaoNotificacao> configuracaoNotificacaoCFG = builder.EntityType<ConfiguracaoNotificacao>();
            configuracaoNotificacaoCFG.Property(c => c.HoraEnvio).AsTimeOfDay();

            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }

        protected override Task RunServiceBus()
        {            
            return Task.Factory.StartNew(() => new ServiceBusBL());
        }
    }
}