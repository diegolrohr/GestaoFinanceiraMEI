using Fly01.Core;
using Fly01.Core.API.Application;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.ServiceBus;
using Microsoft.OData.Edm;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.OData.Builder;

namespace Fly01.OrdemServico.API
{
    public class WebApiApplication : GlobalWebAPIApplication
    {
        protected override IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder
            {
                ContainerName = "DefaultContainer"
            };

            builder.EntitySet<Cidade>("cidade");
            builder.EntitySet<Estado>("estado");
            builder.EntitySet<GrupoProduto>("grupoproduto");
            builder.EntitySet<Ncm>("ncm");
            builder.EntitySet<Nbs>("nbs");
            builder.EntitySet<Core.Entities.Domains.Commons.OrdemServico>("ordemservico");
            builder.EntitySet<OrdemServicoItemProduto>("ordemservicoitemproduto");
            builder.EntitySet<OrdemServicoItemServico>("ordemservicoitemservico");
            builder.EntitySet<OrdemServicoManutencao>("ordemservicomanutencao");
            builder.EntitySet<ParametroOrdemServico>("parametroordemservico");
            builder.EntitySet<Pessoa>("pessoa");
            builder.EntitySet<Produto>("produto");
            builder.EntitySet<Servico>("servico");
            builder.EntitySet<UnidadeMedida>("unidademedida");
            builder.EntitySet<Iss>("iss");
            builder.EntitySet<Arquivo>("arquivo");
            builder.EntitySet<Kit>("kit");
            builder.EntitySet<KitItem>("kititem");

            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }

        protected override string GetInstrumentationKeyAppInsights() => ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"];

        protected override Task RunServiceBusApps() => Task.Factory.StartNew(() =>
        {
            SetupEnvironment.Create(RabbitConfig.VirtualHostApps);
            SetupEnvironment.Create(RabbitConfig.VirtualHostIntegracao);

            new Consumer(Assembly.Load("Fly01.OrdemServico.BL").GetType("Fly01.OrdemServico.BL.UnitOfWork")).Consume();
        });

        protected override void SetAppDefaults()
        {
            AppDefaults.UrlGateway = ConfigurationManager.AppSettings["UrlGateway"];
            AppDefaults.UrlEmissaoNfeApi = ConfigurationManager.AppSettings["UrlEmissaoNfeApi"];
            AppDefaults.UrlEstoqueApi = ConfigurationManager.AppSettings["UrlEstoqueApi"];
            AppDefaults.UrlFinanceiroApi = ConfigurationManager.AppSettings["UrlFinanceiroApi"];
            AppDefaults.UrlComprasApi = ConfigurationManager.AppSettings["UrlComprasApi"];
            AppDefaults.UrlApiGatewayMpn = ConfigurationManager.AppSettings["UrlGatewayMpn"];

            base.SetAppDefaults();
        }
    }
}