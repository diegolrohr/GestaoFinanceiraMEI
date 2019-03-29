using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData;
using System.Web.OData.Batch;
using System.Web.OData.Extensions;
using System.Web.OData.Query;

namespace Fly01.Core.API.Application
{
    public class ODataConfig
    {
        private static EnableQueryAttribute queryAttribute => new EnableQueryAttribute()
        {
            AllowedQueryOptions = AllowedQueryOptions.All,
            MaxTop = 500,
            PageSize = 50,
            MaxExpansionDepth = 10,
            EnableConstantParameterization = true,
            AllowedFunctions = AllowedFunctions.All,
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            EnsureStableOrdering = true,
            HandleNullPropagation = HandleNullPropagationOption.True
        };

        public static void Register(HttpConfiguration config, IEdmModel edmModel)
        {
            config.Count(QueryOptionSetting.Allowed)
                  .Filter(QueryOptionSetting.Allowed)
                  .OrderBy(QueryOptionSetting.Allowed)
                  .Expand(QueryOptionSetting.Allowed)
                  .Select(QueryOptionSetting.Allowed)
                  .MaxTop(50)
                  .AddODataQueryFilter(queryAttribute);
            config.MapHttpAttributeRoutes(new CustomDirectRouteProvider());
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "api",
                model: edmModel,
                batchHandler: new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));
        }

        public class CaseInsensitiveResolver : ODataUriResolver
        {
            public override bool EnableCaseInsensitive
            {
                get { return true; }
                set { /* Ignore value */ }
            }
        }
    }
}
