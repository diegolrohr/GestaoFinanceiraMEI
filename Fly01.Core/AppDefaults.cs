using System;
using Fly01.Core.Config;
using System.Collections.Generic;
using System.Globalization;

namespace Fly01.Core
{
    public static class AppDefaults
    {
        public const string ContentTypeJson = "application/json";
        public const string ContentTypeFormUrlencoded = "application/x-www-form-urlencoded";
        public const string CreateSuccessMessage = "Inclusão realizada com sucesso.";
        public const string DeleteSuccessMessage = "Exclusão realizada com sucesso.";
        public const string EditSuccessMessage = "Alteração realizada com sucesso.";
        public const int LicenciamentoProdutoId = 17;
        public const int CookieMaxByteSize = 4000;
        public const string TenatIdHeader = "TenantId";
        public const int MaxRecordsPerPage = 10;
        public const int MaxRecordsPerPageAPI = 50;
        public const string SchedulerAzureBasicUserName = "0A2905B9-8AF8-4789-9732-914946A9C57B";
        public const string SchedulerAzureBasicPassword = "9IURl9t1";

        public const string TaskMsgStepRecuperandoRegistros = "Recuperando registros.";
        public const string TaskMsgStepMontandoArquivo = "Exportando dados para o excel.";
        public const string TaskMsgStepConcluido = "Exportação concluída.";

        public const string SeparadorSubjectEmailMensageria = "#!#";

        public static string MPNUIVersion { get; set; }

        public static string MashupClientId { get; set; }
        public static string MashupPassword { get; set; }
        public static string MashupUser { get; set; }

        public static string UrlNotificationSocket { get; set; }
        public static string UrlGateway { get; set; }
        public static string UrlGatewayNew { get; set; }
        public static string UrlManager { get; set; }
        public static string UrlManagerNew { get; set; }
        public static string UrlApiGateway { get; set; }
        public static string UrlFinanceiroApi { get; set; }
        public static string UrlFaturamentoApi { get; set; }
        public static string UrlComprasApi { get; set; }
        public static string UrlEmissaoNfeApi { get; set; }
        public static string UrlEstoqueApi { get; set; }

        public static string UrlFinanceiroWeb { get; set; }
        public static string UrlFaturamentoWeb { get; set; }
        public static string UrlComprasWeb { get; set; }
        public static string UrlEstoqueWeb { get; set; }
        public static string UrlOrdemServicoWeb { get; set; }

        public static string FinanceiroClientId { get; set; }
        public static string FaturamentoClientId { get; set; }
        public static string EstoqueClientId { get; set; }
        public static string ComprasClientId { get; set; }
        public static string OrdemServicoClientId { get; set; }

        public static string GatewayUserName { get; set; }
        public static string GatewayPassword { get; set; }
        public static string GatewayVerificationKeyPassword { get; set; }
        
        public static string UrlLoginSSO { get; set; }
        public static string UrlLogoutSSO { get; set; }
       
        public static string UrlLicenseManager { get; set; }
        
        public static string SessionKey { get; set; }
        
        public static string AppId { get; set; }

        public static string RootPathApplication { get; set; }

        public static string APICoreResourceName { get; set; }

        public static string APIEnumResourceName { get; set; }

        public static string APIDomainResourceName { get; set; }
        public static string JWTSocketAuth { get; set; }

        public static string GetRootPathApplication(string app) => string.Format(RootPathApplication, app);

        public static CultureInfo CultureInfoDefault
        {
            get
            {
                var defaultCulture = new CultureInfo("pt-BR");
                defaultCulture.NumberFormat.CurrencyPositivePattern = 2;
                defaultCulture.NumberFormat.CurrencyNegativePattern = 12;

                return defaultCulture;
            }
        }

        public static string GetResourceName(Type type) => type.Name.Substring(0, type.Name.Length - 2);

        public static Dictionary<string, string> GetQueryStringDefault(string filterField = "", string filterValue = "", int maxRecords = MaxRecordsPerPage)
        {
            if (string.IsNullOrWhiteSpace(filterField) && string.IsNullOrWhiteSpace(filterValue))
                return new Dictionary<string, string> {
                    { "$count", "true" },
                };

            return new Dictionary<string, string> {
                { "$count", "true" },
                { "$orderby", filterField },
            };
        }

        public static ProxyConfig ProxyConfig { get; set; }
    }
}