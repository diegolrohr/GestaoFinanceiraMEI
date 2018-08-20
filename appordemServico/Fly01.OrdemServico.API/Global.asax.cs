using Fly01.Core.API.Application;
using Fly01.OrdemServico.BL;
using Microsoft.OData.Edm;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using Fly01.Core;
using Fly01.Core.Entities.Domains.Commons;
using System.Collections.Generic;
using System.Reflection;
using Fly01.Core.ServiceBus;

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

            //exemplo builder.EntitySet<Estado>("estado");
            builder.EntitySet<Pessoa>("pessoa");
            builder.EntitySet<Produto>("produto");
            builder.EntitySet<GrupoProduto>("grupoproduto");
            builder.EntitySet<Estado>("estado");
            builder.EntitySet<Cidade>("cidade");
            builder.EntitySet<ParametroOrdemServico>("parametroordemservico");

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

            base.SetAppDefaults();
        }
    }
}