using Fly01.Core.Config;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace Fly01.Core.Rest
{
    public static class RestHelper
    {
        #region Static Properties

        public static Dictionary<string, string> DefaultHeader
        {
            get
            {
                Dictionary<string, string> header = Header;
                //header.Add("EmpresaId", "D3FC7081-7643-4CBD-9047-CC9B6F619AA7");//teste inicial
                header.Add("EmpresaId", "9DDEF5EF-7904-4039-AD15-12F0338652E1"); //validacao Sonia
                header.Add("AppUser", "diegol.rohr@gmail.com");
                header.Add("Content-Type", "application/json");
                return header;
            }
        }

        public static Dictionary<string, string> Header
        {
            get
            {
                Dictionary<string, string> header = new Dictionary<string, string>();

                return header;
            }
        }

        #endregion

        #region PUT
        public static TReturn ExecutePutRequest<TReturn>(string resource, object requestObj, Dictionary<string, string> queryString = null)
        {
            return ExecutePutRequest<TReturn>(AppDefaults.UrlFinanceiroApi, resource, requestObj, queryString);
        }

        public static string ExecutePutRequest(string resource, string requestJson, Dictionary<string, string> queryString = null)
        {
            return ExecutePutRequest(AppDefaults.UrlFinanceiroApi, resource, requestJson, queryString);
        }

        public static string ExecutePutRequest(string url, string resource, string requestJson, Dictionary<string, string> queryString = null)
        {
            HttpStatusCode statusCode;
            string statusDescription = string.Empty;
            string returnObj = string.Empty;
            DateTime beginRequest = DateTime.Now;
            try
            {
                returnObj = RestUtils.ExecutePutRequest(url, resource, requestJson, out statusCode, out statusDescription, DefaultHeader, queryString);
                return returnObj;
            }
            catch (ApiException ex)
            {
                if (queryString != null)
                {
                    resource += "?";
                    foreach (var qItem in queryString)
                    {
                        resource += qItem.Key + "=" + qItem.Value + "&";
                    }
                    resource = resource.TrimEnd('&');
                }

                throw;
            }
        }

        public static TReturn ExecutePutRequest<TReturn>(string url, string resource, object requestObj, Dictionary<string, string> queryString = null)
        {
            TReturn returnObj = default(TReturn);
            DateTime beginRequest = DateTime.Now;
            HttpStatusCode statusCode;
            string statusDescription = string.Empty;
            try
            {
                returnObj = RestUtils.ExecutePutRequest<TReturn>(url, resource, requestObj, out statusCode, out statusDescription, DefaultHeader, queryString);
                return returnObj;
            }
            catch (ApiException ex)
            {
                if (queryString != null)
                {
                    resource += "?";
                    foreach (var qItem in queryString)
                    {
                        resource += qItem.Key + "=" + qItem.Value + "&";
                    }
                    resource = resource.TrimEnd('&');
                }

                throw;
            }
        }

        public static TReturn ExecutePutRequest<TReturn>(string url, string resource, object requestObj, Dictionary<string, string> queryString, Dictionary<string, string> header, int timeout = 150)
        {
            TReturn returnObj = default(TReturn);
            DateTime beginRequest = DateTime.Now;
            HttpStatusCode statusCode;
            string statusDescription = string.Empty;
            try
            {
                returnObj = RestUtils.ExecutePutRequest<TReturn>(url, resource, requestObj, out statusCode, out statusDescription, header, queryString);
                return returnObj;
            }
            catch (ApiException ex)
            {
                if (queryString != null)
                {
                    resource += "?";
                    foreach (var qItem in queryString)
                    {
                        resource += qItem.Key + "=" + qItem.Value + "&";
                    }
                    resource = resource.TrimEnd('&');
                }

                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GET
        public static TReturn ExecuteGetRequest<TReturn>(string resource, Dictionary<string, string> queryString = null)
        {
            try
            {
                return ExecuteGetRequest<TReturn>(AppDefaults.UrlFinanceiroApi, resource, DefaultHeader, queryString);
            }
            catch (ApiException ex)
            {
                if (queryString != null)
                {
                    resource += "?";
                    foreach (var qItem in queryString)
                    {
                        resource += qItem.Key + "=" + qItem.Value + "&";
                    }
                    resource = resource.TrimEnd('&');
                }

                throw;
            }
        }

        public static TReturn ExecuteGetRequest<TReturn>(string url, string resource)
        {
            return ExecuteGetRequest<TReturn>(url, resource, null, null);
        }

        public static TReturn ExecuteGetRequest<TReturn>(string url, string resource, Dictionary<string, string> queryString = null)
        {
            try
            {
                return ExecuteGetRequest<TReturn>(url, resource, DefaultHeader, queryString);
            }
            catch (ApiException ex)
            {
                if (queryString != null)
                {
                    resource += "?";
                    foreach (var qItem in queryString)
                    {
                        resource += qItem.Key + "=" + qItem.Value + "&";
                    }
                    resource = resource.TrimEnd('&');
                }

                throw;
            }
        }

        public static TReturn ExecuteGetRequest<TReturn>(string url, string resource, Dictionary<string, string> header, Dictionary<string, string> queryString = null)
        {
            TReturn returnObj = default(TReturn);
            DateTime beginRequest = DateTime.Now;
            HttpStatusCode statusCode;
            string statusDescription = string.Empty;
            try
            {
                returnObj = RestUtils.ExecuteGetRequest<TReturn>(url, resource, out statusCode, out statusDescription, header, queryString, timeout: 600);
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static TokenDataVM ExecuteGetAuthToken(string url, string userName, string password, string platformUrl,
            string platformUser)
        {
            string encodedBody =
                string.Format("username={0}&grant_type=password&password={1}&platformurl={2}&platformuser={3}",
                    HttpUtility.HtmlEncode(userName), HttpUtility.HtmlEncode(password),
                    HttpUtility.HtmlEncode(platformUrl), HttpUtility.HtmlEncode(platformUser));

            string resource = string.Format("{0}token", url.Replace("api/", string.Empty));

            return RestUtils.ExecuteGeneratedAuthToken<TokenDataVM>(resource, encodedBody);
        }
        #endregion

        #region POST
        public static TReturn ExecutePostRequest<TReturn>(string url, string resource, object requestObj, Dictionary<string, string> queryString = null, int timeout = 150)
        {
            TReturn returnObj = default(TReturn);
            DateTime beginRequest = DateTime.Now;
            HttpStatusCode statusCode;
            string statusDescription = string.Empty;
            try
            {
                returnObj = RestUtils.ExecutePostRequest<TReturn>(url, resource, out statusCode, out statusDescription, requestObj, DefaultHeader, queryString, timeout: timeout);
                return returnObj;
            }
            catch (ApiException ex)
            {
                if (queryString != null)
                {
                    resource += "?";
                    foreach (var qItem in queryString)
                    {
                        resource += qItem.Key + "=" + qItem.Value + "&";
                    }
                    resource = resource.TrimEnd('&');
                }

                throw;
            }
        }

        public static TReturn ExecutePostRequest<TReturn>(string url, string resource, object requestObj, Dictionary<string, string> queryString, Dictionary<string, string> header, int timeout = 150)
        {
            TReturn returnObj = default(TReturn);
            DateTime beginRequest = DateTime.Now;
            HttpStatusCode statusCode;
            string statusDescription = string.Empty;
            try
            {
                returnObj = RestUtils.ExecutePostRequest<TReturn>(url, resource, out statusCode, out statusDescription, requestObj, header, queryString, timeout: timeout);
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static TReturn ExecutePostRequest<TReturn>(string resource, object requestObj, Dictionary<string, string> queryString = null, int timeout = 150)
        {
            return ExecutePostRequest<TReturn>(AppDefaults.UrlFinanceiroApi, resource, requestObj, queryString, timeout: timeout);
        }

        public static string ExecutePostRequest(string resource, string requestJson, Dictionary<string, string> queryString = null, int timeout = 150)
        {
            return ExecutePostRequest(AppDefaults.UrlFinanceiroApi, resource, requestJson, queryString, timeout: timeout);
        }

        public static string ExecutePostRequest(string url, string resource, string requestJson, Dictionary<string, string> queryString = null, int timeout = 150)
        {
            return ExecutePostRequest(url, resource, requestJson, queryString, DefaultHeader, timeout);
        }

        public static string ExecutePostRequest(string url, string resource, string requestJson, Dictionary<string, string> queryString = null, Dictionary<string, string> header = null, int timeout = 150)
        {
            HttpStatusCode statusCode;
            string statusDescription = string.Empty;
            string returnObj = string.Empty;
            DateTime beginRequest = DateTime.Now;
            try
            {
                returnObj = RestUtils.ExecutePostRequest(url, resource, out statusCode, out statusDescription, requestJson, header, queryString, timeout: timeout);
                return returnObj;
            }
            catch (ApiException ex)
            {
                if (queryString != null)
                {
                    resource += "?";
                    foreach (var qItem in queryString)
                    {
                        resource += qItem.Key + "=" + qItem.Value + "&";
                    }
                    resource = resource.TrimEnd('&');
                }

                throw;
            }
        }
        #endregion

        #region DELETE
        public static bool ExecuteDeleteRequest(string resource)
        {
            try
            {
                return ExecuteDeleteRequest(AppDefaults.UrlFinanceiroApi, resource, DefaultHeader);
            }
            catch (ApiException ex)
            {
                    throw;
            }
        }

        public static bool ExecuteDeleteRequest(string url, string resource, Dictionary<string, string> header)
        {
            HttpStatusCode statusCode;
            string statusDescription = string.Empty;
            bool returnOK = false;
            DateTime beginRequest = DateTime.Now;
            try
            {
                returnOK = RestUtils.ExecuteDeleteRequest(url, resource, out statusCode, out statusDescription, header ?? DefaultHeader);
                return returnOK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ExecuteDeleteRequest(string url, string resource, Dictionary<string, string> header, Dictionary<string, string> queryString)
        {
            var statusDescription = string.Empty;
            var beginRequest = DateTime.Now;
            try
            {
                HttpStatusCode statusCode;
                var returnOk = RestUtils.ExecuteDeleteRequest(url, resource, out statusCode, out statusDescription, header ?? DefaultHeader, queryString);
                return returnOk;
            }
            catch (ApiException ex)
            {
                throw;
            }
        }
        #endregion
    }
}