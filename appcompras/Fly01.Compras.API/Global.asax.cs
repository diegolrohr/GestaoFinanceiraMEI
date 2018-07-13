﻿using Fly01.Compras.BL;
using Fly01.Core.API.Application;
using Fly01.Core.Entities.Domains.Commons;
using Microsoft.OData.Edm;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.OData.Builder;

namespace Fly01.Compras.API
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
            builder.EntitySet<Estado>("estado");
            builder.EntitySet<Cidade>("cidade");
            builder.EntitySet<CondicaoParcelamento>("condicaoparcelamento");
            builder.EntitySet<Produto>("produto");
            builder.EntitySet<FormaPagamento>("formapagamento");
            builder.EntitySet<UnidadeMedida>("unidademedida");
            builder.EntitySet<GrupoProduto>("grupoproduto");
            builder.EntitySet<Ncm>("ncm");
            builder.EntitySet<Categoria>("categoria");
            builder.EntitySet<OrdemCompra>("ordemcompra");
            builder.EntitySet<Pedido>("pedido");
            builder.EntitySet<PedidoItem>("pedidoitem");
            builder.EntitySet<Orcamento>("orcamento");
            builder.EntitySet<OrcamentoItem>("orcamentoitem");
            builder.EntitySet<SubstituicaoTributaria>("substituicaotributaria");
            builder.EntitySet<Cfop>("cfop");
            builder.EntitySet<GrupoTributario>("grupotributario");
            builder.EntitySet<Cest>("cest");
            builder.EntitySet<EnquadramentoLegalIPI>("enquadramentolegalipi");

            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }

        protected override string GetInstrumentationKeyAppInsights() => ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"];

        protected override Task RunServiceBus() => Task.Factory.StartNew(() => new ServiceBusBL());
        protected override Task RunServiceBus2() => Task.Factory.StartNew(() => new ServiceBusBL());
        protected override Task RunServiceBus3() => Task.Factory.StartNew(() => new ServiceBusBL());
        protected override Task RunServiceBus4() => Task.Factory.StartNew(() => new ServiceBusBL());
        protected override Task RunServiceBus5() => Task.Factory.StartNew(() => new ServiceBusBL());
    }
}