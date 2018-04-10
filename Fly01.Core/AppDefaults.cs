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

        public static string MashupClientId { get; set; }
        public static string MashupPassword { get; set; }
        public static string MashupUser { get; set; }

        public static string UrlGateway { get; set; }
        public static string UrlApiGateway { get; set; }
        public static string UrlManager { get; set; }
        public static string UrlFinanceiroApi { get; set; }
        public static string UrlFaturamentoApi { get; set; }
        public static string UrlComprasApi { get; set; }
        public static string UrlEmissaoNfeApi { get; set; }
        public static string UrlEstoqueApi { get; set; }

        public static string GatewayUserName { get; set; }
        public static string GatewayPassword { get; set; }
        public static string GatewayVerificationKeyPassword { get; set; }
        
        public static string UrlLoginSSO { get; set; }
        public static string UrlLogoutSSO { get; set; }
       
        public static string UrlLicenseManager { get; set; }
        
        public static string SessionKey { get; set; }
        
        public static string AppId { get; set; }

        public static string RootPathApplication { get; set; }

        public static string GetRootPathApplication(string app)
        {
            return string.Format(RootPathApplication, app);
        }

        public static CultureInfo CultureInfoDefault
        {
            get
            {
                return new CultureInfo("pt-BR");
            }
        }

        public static string GetResourceName(Type type)
        {
            /*
             * Por convensão foi definido que todos os resources serão iguais ao nome das classes, retirando
             * apenas o sufixo VM.
             * Ex.: AccountsPayableVM -> AccountsPayable
             * **/
            return type.Name.Substring(0, type.Name.Length - 2);
        }

        public static Dictionary<string, string> GetQueryStringDefault(string filterField = "", string filterValue = "", int maxRecords = MaxRecordsPerPage)
        {
            if (string.IsNullOrWhiteSpace(filterField) && string.IsNullOrWhiteSpace(filterValue))
                return new Dictionary<string, string> {
                    { "$count", "true" },
                };

            return new Dictionary<string, string> {
                //{ filterField, filterValue },
                { "$count", "true" },
                { "$orderby", filterField },
            };
        }

        public static ProxyConfig ProxyConfig { get; set; }
    }
}