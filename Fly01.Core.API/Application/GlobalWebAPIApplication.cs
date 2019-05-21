using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Fly01.Core.API.Application
{
    public abstract class GlobalWebAPIApplication : HttpApplication
    {
        protected abstract IEdmModel GetEdmModel();
        protected abstract string GetInstrumentationKeyAppInsights();
        protected abstract Task RunServiceBusApps();

        protected virtual void SetAppDefaults()
        {
            AppDefaults.UrlGateway = ConfigurationManager.AppSettings["UrlGateway"];
            AppDefaults.UrlEmissaoNfeApi = ConfigurationManager.AppSettings["UrlEmissaoNfeApi"];
            AppDefaults.UrlEstoqueApi = ConfigurationManager.AppSettings["UrlEstoqueApi"];
            AppDefaults.UrlFinanceiroApi = ConfigurationManager.AppSettings["UrlFinanceiroApi"];
            AppDefaults.UrlEmissaoNfeApi = ConfigurationManager.AppSettings["UrlEmissaoNfeApi"];
            AppDefaults.UrlComprasApi = ConfigurationManager.AppSettings["UrlComprasApi"];
            AppDefaults.UrlGateway = ConfigurationManager.AppSettings["UrlGateway"];
            AppDefaults.UrlGatewayNew = $"{ConfigurationManager.AppSettings["UrlGatewayNew"]}api/";
            AppDefaults.UrlManager = $"{AppDefaults.UrlGatewayNew}manager/";
            AppDefaults.UrlNotificationSocket = ConfigurationManager.AppSettings["UrlNotificationSocket"];
        }

        protected void Application_Start()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                FloatFormatHandling = FloatFormatHandling.DefaultValue,
                FloatParseHandling = FloatParseHandling.Decimal,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                
                Converters = new[] { new IsoDateTimeConverter { DateTimeStyles = System.Globalization.DateTimeStyles.AdjustToUniversal } }
            };

            GlobalFilters.Filters.Add(new AiHandleErrorAttribute());

            GlobalConfiguration.Configure(config =>
            {
                ODataConfig.Register(config, GetEdmModel());
                WebAPIConfig.Register(config);
            });

            string instrumentationKeyAppInsights = GetInstrumentationKeyAppInsights();
            if(!string.IsNullOrWhiteSpace(instrumentationKeyAppInsights))
                TelemetryConfiguration.Active.InstrumentationKey = instrumentationKeyAppInsights;

            RunServiceBusApps();
            SetAppDefaults();
        }
    }
}