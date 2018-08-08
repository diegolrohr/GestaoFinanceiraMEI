﻿using Fly01.Estoque.BL;
using System.Threading.Tasks;
using Fly01.Core.API.Application;
using Microsoft.OData.Edm;
using System.Web.OData.Builder;
using Fly01.Core.Entities.Domains.Commons;
using System.Configuration;
using System.Collections.Generic;

namespace Fly01.Estoque.API
{
    public class WebApiApplication : GlobalWebAPIApplication
    {
        protected override IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder
            {
                ContainerName = "DefaultContainer"
            };

            builder.EntitySet<Estado>("estado");
            builder.EntitySet<Produto>("produto");
            builder.EntitySet<Ncm>("ncm");
            builder.EntitySet<Inventario>("inventario");
            builder.EntitySet<InventarioItem>("inventarioitem");
            builder.EntitySet<TipoMovimento>("tipomovimento");
            builder.EntitySet<GrupoProduto>("grupoproduto");
            builder.EntitySet<UnidadeMedida>("unidademedida");
            builder.EntitySet<PosicaoAtual>("posicaoatual");
            builder.EntitySet<AjusteManual>("ajustemanual");
            builder.EntitySet<Produto>("produtosmaismovimentados");
            builder.EntitySet<Produto>("produtosmenosmovimentados");
            builder.EntitySet<Cest>("cest");
            builder.EntitySet<EnquadramentoLegalIPI>("enquadramentolegalipi");

            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }

        protected override string GetInstrumentationKeyAppInsights() => ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"];

        protected override Task RunServiceBus() => Task.Factory.StartNew(() => new ServiceBusBL().Consume());
    }
}