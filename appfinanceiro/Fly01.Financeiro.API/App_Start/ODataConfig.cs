using Fly01.Financeiro.Domain.Entities;
using Fly01.Core.API;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Batch;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Query;

namespace Fly01.Financeiro.API.App_Start
{
    public class ODataConfig
    {
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
            builder.EntitySet<ContaBancaria>("contabancaria");
            builder.EntitySet<Feriado>("feriado");
            builder.EntitySet<Categoria>("categoria");
            builder.EntitySet<Banco>("banco");
            builder.EntitySet<ContaPagar>("contapagar");
            builder.EntitySet<ContaReceber>("contareceber");
            builder.EntitySet<Estado>("estado");
            builder.EntitySet<Cidade>("cidade");
            builder.EntitySet<CondicaoParcelamento>("condicaoparcelamento");
            builder.EntitySet<ContaFinanceiraBaixa>("contafinanceirabaixa");
            //builder.EntitySet<DemonstrativoResultadoExercicio>("demonstrativoresultadoexercicio");
            builder.EntitySet<ConciliacaoBancaria>("conciliacaobancaria");
            builder.EntitySet<ConciliacaoBancariaItem>("conciliacaobancariaitem");
            builder.EntitySet<ConciliacaoBancariaItemContaFinanceira>("conciliacaobancariaitemcontafinanceira");
            builder.EntitySet<ConciliacaoBancariaTransacao>("conciliacaobancariatransacao");
            builder.EntitySet<ConciliacaoBancariaItem>("conciliacaobancariabuscarexistentes");
            builder.EntitySet<FormaPagamento>("formapagamento");
            builder.EntitySet<ContaFinanceiraRenegociacao>("contafinanceirarenegociacao");
            builder.EntitySet<Movimentacao>("movimentacao");
            builder.EntitySet<Transferencia>("transferencia");
            builder.EntitySet<MovimentacaoPorCategoria>("receitaporcategoria");
            builder.EntitySet<MovimentacaoPorCategoria>("despesaporcategoria");
            builder.EntitySet<MovimentacaoPorCategoria>("movimentacaoporcategoria");
            builder.EntitySet<ConfiguracaoNotificacao>("configuracaonotificacao");

            EntityTypeConfiguration<ConfiguracaoNotificacao> configuracaoNotificacaoCFG = builder.EntityType<ConfiguracaoNotificacao>();
            configuracaoNotificacaoCFG.Property(c => c.HoraEnvio).AsTimeOfDay();

            
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