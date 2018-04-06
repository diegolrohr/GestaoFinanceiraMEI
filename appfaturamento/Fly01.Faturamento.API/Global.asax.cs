using Fly01.Faturamento.BL;
using Fly01.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Configuration;

namespace Fly01.Faturamento.API
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            SetAppDefaults();
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

        private void SetAppDefaults()
        {
            AppDefaults.UrlGateway = ConfigurationManager.AppSettings["UrlS1Gateway"];
            AppDefaults.UrlEstoqueApi = ConfigurationManager.AppSettings["UrlEstoqueApi"];
            AppDefaults.UrlEmissaoNfeApi = ConfigurationManager.AppSettings["UrlEmissaoNfeApi"];
            AppDefaults.SessionKey = ConfigurationManager.AppSettings["SessionKey"];
        }
    }
}