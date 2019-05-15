using Fly01.Core.API.Application;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.ServiceBus;
using Microsoft.OData.Edm;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using Fly01.Core;

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
            builder.EntitySet<CertificadoDigital>("certificadodigital");
            builder.EntitySet<NotaFiscalInutilizada>("notafiscalinutilizada");
            builder.EntitySet<SerieNotaFiscal>("serienotafiscal");
            builder.EntitySet<ParametroTributario>("parametrotributario");
            builder.EntitySet<NotaFiscalEntrada>("notafiscalentrada");
            builder.EntitySet<NotaFiscalItemTributacaoEntrada>("notafiscalitemtributacaoentrada");
            builder.EntitySet<NFeEntrada>("nfeentrada");
            builder.EntitySet<NFeProdutoEntrada>("nfeprodutoentrada");
            builder.EntitySet<NotaFiscalCartaCorrecaoEntrada>("notafiscalcartacorrecaoentrada");
            builder.EntitySet<Kit>("kit");
            builder.EntitySet<KitItem>("kititem");
            builder.EntitySet<KitItem>("servico");
            builder.EntitySet<KitItem>("iss");
            builder.EntitySet<KitItem>("nbs");
            builder.EntitySet<NFeImportacao>("nfeimportacao");
            builder.EntitySet<NFeImportacaoProduto>("nfeimportacaoproduto");
            builder.EntitySet<NFeImportacaoCobranca>("nfeimportacaocobranca");
            builder.EntitySet<CentroCusto>("centrocusto");
            builder.EntitySet<AliquotaSimplesNacional>("aliquotasimplesnacional");
            builder.EntitySet<Pais>("pais");

            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }

        protected override string GetInstrumentationKeyAppInsights() => ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"];

        protected override Task RunServiceBusApps() => Task.Factory.StartNew(() =>
        {
            SetupEnvironment.Create(RabbitConfig.VirtualHostIntegracao);
            SetupEnvironment.Create(RabbitConfig.VirtualHostApps);

            new Consumer(Assembly.Load("Fly01.Compras.BL").GetType("Fly01.Compras.BL.UnitOfWork")).Consume();
        });
        protected override void SetAppDefaults()
        {
            AppDefaults.UrlGateway = ConfigurationManager.AppSettings["UrlGateway"];
            AppDefaults.UrlEmissaoNfeApi = ConfigurationManager.AppSettings["UrlEmissaoNfeApi"];
            AppDefaults.UrlEstoqueApi = ConfigurationManager.AppSettings["UrlEstoqueApi"];
            AppDefaults.UrlFinanceiroApi = ConfigurationManager.AppSettings["UrlFinanceiroApi"];
            //AppDefaults.UrlGatewayNew = $"{ConfigurationManager.AppSettings["UrlGatewayNew"]}api/";
            //AppDefaults.UrlManager = $"{AppDefaults.UrlGatewayNew}manager/";

            base.SetAppDefaults();
        }
    }
}