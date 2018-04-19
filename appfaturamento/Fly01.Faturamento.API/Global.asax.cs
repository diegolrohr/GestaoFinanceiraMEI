﻿using Fly01.Faturamento.BL;
using System.Threading.Tasks;
using Fly01.Core.API.Application;
using Microsoft.OData.Edm;
using System.Web.OData.Builder;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core;
using System.Configuration;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.API
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
            builder.EntitySet<Produto>("produto");
            builder.EntitySet<UnidadeMedida>("unidademedida");
            builder.EntitySet<GrupoProduto>("grupoproduto");
            builder.EntitySet<Ncm>("ncm");
            builder.EntitySet<Cfop>("cfop");
            builder.EntitySet<GrupoTributario>("grupotributario");
            builder.EntitySet<ParametroTributario>("parametrotributario");
            builder.EntitySet<OrdemVenda>("ordemvenda");
            builder.EntitySet<OrdemVendaProduto>("ordemvendaproduto");
            builder.EntitySet<OrdemVendaServico>("ordemvendaservico");
            builder.EntitySet<Servico>("servico");
            builder.EntitySet<NBS>("nbs");
            builder.EntitySet<CondicaoParcelamento>("condicaoparcelamento");
            builder.EntitySet<FormaPagamento>("formapagamento");
            builder.EntitySet<Categoria>("categoria");
            builder.EntitySet<Cest>("cest");
            builder.EntitySet<SubstituicaoTributaria>("substituicaotributaria");
            builder.EntitySet<SerieNotaFiscal>("serienotafiscal");
            builder.EntitySet<NotaFiscal>("notafiscal");
            builder.EntitySet<NFe>("nfe");
            builder.EntitySet<NFSe>("nfse");
            builder.EntitySet<NFSeServico>("nfseservico");
            builder.EntitySet<NFeProduto>("nfeproduto");
            builder.EntitySet<CertificadoDigital>("certificadodigital");
            builder.EntitySet<SerieNotaFiscal>("serienotafiscalinutilizada");
            builder.EntitySet<NotaFiscalItemTributacao>("notafiscalitemtributacao");
            builder.EntitySet<EnquadramentoLegalIPI>("enquadramentolegalipi");

            builder.EnableLowerCamelCase();
            
            return builder.GetEdmModel();
        }

        protected override Task RunServiceBus()
        {
           return Task.Factory.StartNew(() => new ServiceBusBL());
        }

        protected override void SetAppDefaults()
        {
            AppDefaults.UrlEmissaoNfeApi = ConfigurationManager.AppSettings["UrlEmissaoNfeApi"];
            AppDefaults.UrlGateway = ConfigurationManager.AppSettings["UrlGateway"];
            AppDefaults.UrlEstoqueApi = ConfigurationManager.AppSettings["UrlEstoqueApi"];

            base.SetAppDefaults();
        }
    }
}