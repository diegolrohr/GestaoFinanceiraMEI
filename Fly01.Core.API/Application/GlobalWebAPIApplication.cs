using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.OData.Edm;

namespace Fly01.Core.API.Application
{
    public abstract class GlobalWebAPIApplication : HttpApplication
    {
        protected abstract IEdmModel GetEdmModel();
        protected abstract Task RunServiceBus();

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

            GlobalConfiguration.Configure(config =>
            {
                ODataConfig.Register(config, GetEdmModel());
                WebAPIConfig.Register(config);
            });

            RunServiceBus();            
        }
    }
}