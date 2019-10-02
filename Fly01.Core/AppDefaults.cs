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
        public const int MaxRecordsPerPage = 10;
        public const int MaxRecordsPerPageAPI = 50;

        public static string MPNUIVersion { get; set; }
        
        public static string UrlFinanceiroApi { get; set; }

        public static string MashupClientId { get; set; }
        public static string MashupPassword { get; set; }
        public static string MashupUser { get; set; }

        public static string APICoreResourceName { get; set; }

        public static string APIEnumResourceName { get; set; }

        public static string APIDomainResourceName { get; set; }

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

        public static string GetResourceName(Type type) => type.Name.Substring(0, type.Name.Length - 2).ToLower();

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