using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
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
        protected abstract Task RunServiceBus();
        protected virtual void SetAppDefaults() { }

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

            RunServiceBus();
            SetAppDefaults();
        }
    }
}