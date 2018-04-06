using Fly01.Estoque.Domain.Entities;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Batch;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using Fly01.Core.API;

namespace Fly01.Estoque.API
{
    public class ODataConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes(new CustomDirectRouteProvider());

            config.EnableCors();

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "api",
                model: GetEdmModel(config),
                batchHandler: new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));
        }

        private static IEdmModel GetEdmModel(HttpConfiguration config)
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            builder.ContainerName = "DefaultContainer";

            var queryAttribute = new EnableQueryAttribute()
            {
                AllowedQueryOptions = AllowedQueryOptions.All,
                MaxTop = 50,
                PageSize = 50,
                MaxExpansionDepth = 10,
                EnableConstantParameterization = true,
                AllowedFunctions = AllowedFunctions.All,
                AllowedArithmeticOperators = AllowedArithmeticOperators.All,
                AllowedLogicalOperators = AllowedLogicalOperators.All,
                EnsureStableOrdering = true,
                HandleNullPropagation = HandleNullPropagationOption.True
            };
            config.Count(QueryOptionSetting.Allowed)
                  .Filter(QueryOptionSetting.Allowed)
                  .OrderBy(QueryOptionSetting.Allowed)
                  .Expand(QueryOptionSetting.Allowed)
                  .Select(QueryOptionSetting.Allowed)
                  .MaxTop(50)
                  .AddODataQueryFilter(queryAttribute);

            builder.EntitySet<Estado>("estado");
            builder.EntitySet<Produto>("produto");
            builder.EntitySet<NCM>("ncm");
            builder.EntitySet<Inventario>("inventario");
            builder.EntitySet<InventarioItem>("inventarioitem");
            builder.EntitySet<TipoMovimento>("tipomovimento");
            builder.EntitySet<GrupoProduto>("grupoproduto");
            builder.EntitySet<UnidadeMedida>("unidademedida");
            builder.EntitySet<PosicaoAtual>("posicaoatual");
            builder.EntitySet<AjusteManual>("ajustemanual");
            builder.EntitySet<Produto>("produtosmaismovimentados");
            builder.EntitySet<Produto>("produtosmenosmovimentados");
            builder.EntitySet<Cest>("cest");
            builder.EntitySet<EnquadramentoLegalIPI>("enquadramentolegalipi");

            builder.EnableLowerCamelCase();

            return builder.GetEdmModel();
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