using Fly01.Financeiro.API.App_Start;
using Fly01.Financeiro.BL;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Fly01.Financeiro.API
{
    public class WebApiApplication : HttpApplication
    {
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
                ODataConfig.Register(config);
                WebApiConfig.Register(config);
            });
            
            Task.Factory.StartNew(() => new ServiceBusBL());
        }
    }
}