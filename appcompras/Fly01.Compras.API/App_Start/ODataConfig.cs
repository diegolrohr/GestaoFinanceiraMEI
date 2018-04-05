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
    {  //http://stackoverflow.com/questions/26676879/webapi-and-odatacontroller-return-406-not-acceptable
        //https://blogs.msdn.microsoft.com/davidhardin/2014/12/17/web-api-odata-v4-lessons-learned/

        //build functions
        //https://blogs.msdn.microsoft.com/odatateam/2014/12/08/tutorial-sample-functions-actions-in-web-api-v2-2-for-odata-v4-0-type-scenario/

        //functions
        //https://damienbod.com/2014/06/13/web-api-and-odata-v4-queries-functions-and-attribute-routing-part-2/

        //batch
        //https://aspnetwebstack.codeplex.com/wikipage?title=Web%20API%20Request%20Batching
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes(new CustomDirectRouteProvider());

            //config.MapODataServiceRoute(
            //    routeName: "ODataRoute",
            //    routePrefix: "api",
            //    model: GetEdmModel(config));

            //config.EnableEnumPrefixFree(enumPrefixFree: true);
            config.EnableCors();

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "api",
                model: GetEdmModel(config),
                batchHandler: new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));

            //http://odata.github.io/WebApi/#13-04-DependencyInjection
            //config.EnableDependencyInjection(builder =>
            //{
            //    //builder.AddService( AddService<ODataUriResolver>(ServiceLifetime.Singleton, sp => new CaseInsensitiveResolver());
            //    builder.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(ODataUriResolver), sp => new CaseInsensitiveResolver());
            //});

            //config.EnsureInitialized();
        }

        private static IEdmModel GetEdmModel(HttpConfiguration config)
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            //builder.Namespace = "Fly01.BelezaEstetica.API";
            builder.ContainerName = "DefaultContainer";

            //GET
            //var function = builder.Function("parcelamentosimulacao");
            //function.Parameter<CondicaoParcelamentoSimulacao>("simulacao");
            //function.ReturnsCollection<CondicaoParcelamentoParcela>();

            //POST
            //var parcelamentoSimulacaoFunction = builder.Action("parcelamentosimulacao");
            //parcelamentoSimulacaoFunction.Parameter<double>("valorreferencia");
            //parcelamentoSimulacaoFunction.Parameter<string>("condicoesparcelamento");
            //parcelamentoSimulacaoFunction.Parameter<Date>("datareferencia");
            //parcelamentoSimulacaoFunction.Parameter<int?>("qtdparcelas");
            //parcelamentoSimulacaoFunction.ReturnsCollection<CondicaoParcelamentoParcela>();

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
            ////  Per Collection Stadium, returns single entity
            //FunctionConfiguration getClienteByEmail = cliente.EntityType.Collection.Function("GetBy");
            //getClienteByEmail.Parameter<string>("Email");
            //getClienteByEmail.ReturnsFromEntitySet<Cliente>("cliente");

            // Global Function
            //builder.Function("GlobalFunction").ReturnsCollectionFromEntitySet<Cliente>("cliente");

            //config.EnableCaseInsensitive(caseInsensitive: true);
            //config.EnableUnqualifiedNameCall(unqualifiedNameCall: true);
            //config.EnableEnumPrefixFree(enumPrefixFree: true);

            // All the property names in the generated Edm Model will become camel case if EnableLowerCamelCase() is called.

            //config.SetTimeZoneInfo(TimeZoneInfo.Utc);

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