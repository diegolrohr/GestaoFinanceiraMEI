using Fly01.Compras.Domain.Entities;
using Fly01.Core.API;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Batch;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Query;

namespace Fly01.Compras.API
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

            builder.EntitySet<Pessoa>("pessoa");
            builder.EntitySet<Arquivo>("arquivo");
            builder.EntitySet<Estado>("estado");
            builder.EntitySet<Cidade>("cidade");
            builder.EntitySet<CondicaoParcelamento>("condicaoparcelamento");
            builder.EntitySet<Produto>("produto");
            builder.EntitySet<FormaPagamento>("formapagamento");
            builder.EntitySet<UnidadeMedida>("unidademedida");
            builder.EntitySet<GrupoProduto>("grupoproduto");
            builder.EntitySet<NCM>("ncm");
            builder.EntitySet<Categoria>("categoria");
            builder.EntitySet<OrdemCompra>("ordemcompra");
            builder.EntitySet<Pedido>("pedido");
            builder.EntitySet<PedidoItem>("pedidoitem");
            builder.EntitySet<Orcamento>("orcamento");
            builder.EntitySet<OrcamentoItem>("orcamentoitem");
            builder.EntitySet<SubstituicaoTributaria>("substituicaotributaria");
            builder.EntitySet<Cfop>("cfop");
            builder.EntitySet<GrupoTributario>("grupotributario");
            builder.EntitySet<Cest>("cest");
            builder.EntitySet<EnquadramentoLegalIPI>("enquadramentolegalipi");

            builder.EnableLowerCamelCase();
            var edmModel = builder.GetEdmModel();
            return edmModel;
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