using Fly01.Core.API.Application;
using Fly01.Financeiro.BL;
using Microsoft.OData.Edm;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using Fly01.Core;
using Fly01.Core.Entities.Domains.Commons;
using System.Collections.Generic;

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

            builder.EntitySet<Estado>("estado");
            builder.EntitySet<Cidade>("cidade");

            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }

        protected override string GetInstrumentationKeyAppInsights() => ConfigurationManager.AppSettings["InstrumentationKeyAppInsights"];

        protected override Task RunServiceBus() => Task.Factory.StartNew(() => new ServiceBusBL().Consume());

        protected override void SetAppDefaults()
        {
            AppDefaults.UrlGateway = ConfigurationManager.AppSettings["UrlGateway"];

            base.SetAppDefaults();
        }
    }
}