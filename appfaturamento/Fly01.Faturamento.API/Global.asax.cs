using System.Threading.Tasks;
using Fly01.Core.API.Application;
using Microsoft.OData.Edm;
using System.Web.OData.Builder;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core;
using System.Configuration;
using Fly01.Core.ServiceBus;
using System.Reflection;

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
            builder.EntitySet<Nbs>("nbs");
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
            builder.EntitySet<NotaFiscalInutilizada>("notafiscalinutilizada");
            builder.EntitySet<NotaFiscalItemTributacao>("notafiscalitemtributacao");
            builder.EntitySet<EnquadramentoLegalIPI>("enquadramentolegalipi");
            builder.EntitySet<NotaFiscalCartaCorrecao>("notafiscalcartacorrecao");
            builder.EntitySet<Iss>("iss");
            builder.EntitySet<Kit>("kit");
            builder.EntitySet<KitItem>("kititem");
            builder.EntitySet<CentroCusto>("centrocusto");

            builder.EnableLowerCamelCase();

            return builder.GetEdmModel();
        }

        protected override string GetInstrumentationKeyAppInsights() => ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"];

        protected override Task RunServiceBusApps() => Task.Factory.StartNew(() =>
        {
            SetupEnvironment.Create(RabbitConfig.VirtualHostApps);
            SetupEnvironment.Create(RabbitConfig.VirtualHostIntegracao);

            new Consumer(Assembly.Load("Fly01.Faturamento.BL").GetType("Fly01.Faturamento.BL.UnitOfWork")).Consume();
        });

        protected override void SetAppDefaults()
        {
            AppDefaults.UrlEmissaoNfeApi = ConfigurationManager.AppSettings["UrlEmissaoNfeApi"];
            AppDefaults.UrlGateway = ConfigurationManager.AppSettings["UrlGateway"];
            AppDefaults.UrlEstoqueApi = ConfigurationManager.AppSettings["UrlEstoqueApi"];
            AppDefaults.UrlFinanceiroApi = ConfigurationManager.AppSettings["UrlFinanceiroApi"];
            AppDefaults.UrlGatewayNew = $"{ConfigurationManager.AppSettings["UrlGatewayNew"]}api/";
            AppDefaults.UrlManager = $"{AppDefaults.UrlGatewayNew}manager/";

            base.SetAppDefaults();
        }
    }
}