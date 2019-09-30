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
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Static Properties
        public static Dictionary<string, string> HeaderTenantIdCompany
        {
            get
            {
                Dictionary<string, string> header = Header;
                header.Add(AppDefaults.TenatIdHeader, TenantId.Split(',')[0]);
                return header;
            }
        }

        public static string TenantId
        {
            get
            {
                return "01,01";
            }
        }

        public static Dictionary<string, string> DefaultHeader
        {
            get
            {
                Dictionary<string, string> header = Header;
                header.Add(AppDefaults.TenatIdHeader, TenantId);
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

        public static Dictionary<string, string> DefaultHeaderWithoutSession(string accessToken)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add(RestUtils.AUTH_HEADER, String.Format("Bearer {0}", accessToken));
            header.Add(AppDefaults.TenatIdHeader, TenantId);

            return header;
        }

        #endregion

        #region PUT
        public static TReturn ExecutePutRequest<TReturn>(string resource, object requestObj, Dictionary<string, string> queryString = null)
        {
            return ExecutePutRequest<TReturn>(AppDefaults.UrlApiGateway, resource, requestObj, queryString);
        }

        public static string ExecutePutRequest(string resource, string requestJson, Dictionary<string, string> queryString = null)
        {
            return ExecutePutRequest(AppDefaults.UrlApiGateway, resource, requestJson, queryString);
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

                LogLevel logLevel = ((int)ex.StatusCode) >= 500 ? LogLevel.Error : LogLevel.Warn;
                var logEvent = new LogEventInfo(logLevel, "", "Put");
                logEvent.Properties["method"] = "PUT";
                logEvent.Properties["resource"] = url + resource;
                logEvent.Properties["requestJson"] = requestJson;
                logEvent.Properties["statusCode"] = ex.StatusCode;
                logEvent.Properties["exception"] = string.Concat(ex.GetType().FullName, " ", ex.Message);
                logger.Log(logEvent);

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

                LogLevel logLevel = ((int)ex.StatusCode) >= 500 ? LogLevel.Error : LogLevel.Warn;
                var logEvent = new LogEventInfo(logLevel, "", "Put");
                logEvent.Properties["method"] = "PUT";
                logEvent.Properties["resource"] = url + resource;
                logEvent.Properties["requestJson"] = (requestObj == null ? string.Empty : JsonConvert.SerializeObject(requestObj));
                logEvent.Properties["statusCode"] = ex.StatusCode;
                logEvent.Properties["exception"] = string.Concat(ex.GetType().FullName, " ", ex.Message);
                logger.Log(logEvent);

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

                LogLevel logLevel = ((int)ex.StatusCode) >= 500 ? LogLevel.Error : LogLevel.Warn;
                var logEvent = new LogEventInfo(logLevel, "", "Put");
                logEvent.Properties["method"] = "PUT";
                logEvent.Properties["resource"] = url + resource;
                logEvent.Properties["requestJson"] = (requestObj == null ? string.Empty : JsonConvert.SerializeObject(requestObj));
                logEvent.Properties["statusCode"] = ex.StatusCode;
                logEvent.Properties["exception"] = string.Concat(ex.GetType().FullName, " ", ex.Message);
                logger.Log(logEvent);

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
                return ExecuteGetRequest<TReturn>(AppDefaults.UrlApiGateway, resource, DefaultHeader, queryString);
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

                LogLevel logLevel = ((int)ex.StatusCode) >= 500 ? LogLevel.Error : LogLevel.Warn;
                var logEvent = new LogEventInfo(logLevel, "", "Get");
                logEvent.Properties["method"] = "GET";
                logEvent.Properties["resource"] = AppDefaults.UrlApiGateway + resource;
                logEvent.Properties["requestJson"] = "";
                logEvent.Properties["statusCode"] = ex.StatusCode;
                logEvent.Properties["exception"] = string.Concat(ex.GetType().FullName, " ", ex.Message);
                logger.Log(logEvent);

                throw;
            }
        }

        public static TReturn ExecuteGetRequestWithoutSession<TReturn>(string resource, Dictionary<string, string> queryString = null, string acessToken = "")
        {
            return ExecuteGetRequest<TReturn>(AppDefaults.UrlApiGateway, resource, DefaultHeaderWithoutSession(acessToken), queryString);
        }

        public static TReturn ExecuteGetRequest<TReturn>(string url, string resource)
        {
            return ExecuteGetRequest<TReturn>(url, resource, null, null);
        }

        public static TReturn ExecuteGetRequestTenantIdCompany<TReturn>(string resource)
        {
            return ExecuteGetRequest<TReturn>(AppDefaults.UrlApiGateway, resource, HeaderTenantIdCompany, null);
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

                LogLevel logLevel = ((int)ex.StatusCode) >= 500 ? LogLevel.Error : LogLevel.Warn;
                var logEvent = new LogEventInfo(logLevel, "", "Get");
                logEvent.Properties["method"] = "GET";
                logEvent.Properties["resource"] = url + resource;
                logEvent.Properties["requestJson"] = "";
                logEvent.Properties["statusCode"] = ex.StatusCode;
                logEvent.Properties["exception"] = string.Concat(ex.GetType().FullName, " ", ex.Message);
                logger.Log(logEvent);

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

                LogLevel logLevel = ((int)ex.StatusCode) >= 500 ? LogLevel.Error : LogLevel.Warn;
                var logEvent = new LogEventInfo(logLevel, "", "Post");
                logEvent.Properties["method"] = "POST";
                logEvent.Properties["resource"] = url + resource;
                logEvent.Properties["requestJson"] = ((requestObj == null) ? string.Empty : JsonConvert.SerializeObject(requestObj));
                logEvent.Properties["statusCode"] = ex.StatusCode;
                logEvent.Properties["exception"] = string.Concat(ex.GetType().FullName, " ", ex.Message);
                logger.Log(logEvent);

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
            return ExecutePostRequest<TReturn>(AppDefaults.UrlApiGateway, resource, requestObj, queryString, timeout: timeout);
        }

        public static string ExecutePostRequest(string resource, string requestJson, Dictionary<string, string> queryString = null, int timeout = 150)
        {
            return ExecutePostRequest(AppDefaults.UrlApiGateway, resource, requestJson, queryString, timeout: timeout);
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

                LogLevel logLevel = ((int)ex.StatusCode) >= 500 ? LogLevel.Error : LogLevel.Warn;
                var logEvent = new LogEventInfo(logLevel, "", "Post");
                logEvent.Properties["method"] = "POST";
                logEvent.Properties["resource"] = url + resource;
                logEvent.Properties["requestJson"] = requestJson;
                logEvent.Properties["statusCode"] = ex.StatusCode;
                logEvent.Properties["exception"] = string.Concat(ex.GetType().FullName, " ", ex.Message);
                logger.Log(logEvent);

                throw;
            }
        }
        #endregion

        #region DELETE
        public static bool ExecuteDeleteRequest(string resource)
        {
            try
            {
                return ExecuteDeleteRequest(AppDefaults.UrlApiGateway, resource, DefaultHeader);
            }
            catch (ApiException ex)
            {
                LogLevel logLevel = ((int)ex.StatusCode) >= 500 ? LogLevel.Error : LogLevel.Warn;
                var logEvent = new LogEventInfo(logLevel, "", "Delete");
                logEvent.Properties["method"] = "DELETE";
                logEvent.Properties["resource"] = AppDefaults.UrlApiGateway + resource;
                logEvent.Properties["requestJson"] = string.Empty;
                logEvent.Properties["statusCode"] = ex.StatusCode;
                logEvent.Properties["exception"] = string.Concat(ex.GetType().FullName, " ", ex.Message);
                logger.Log(logEvent);

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
                var logLevel = (int)ex.StatusCode >= 500 ? LogLevel.Error : LogLevel.Warn;
                var logEvent = new LogEventInfo(logLevel, "", "Delete");
                logEvent.Properties["method"] = "DELETE";
                logEvent.Properties["resource"] = AppDefaults.UrlApiGateway + resource;
                logEvent.Properties["requestJson"] = string.Empty;
                logEvent.Properties["statusCode"] = ex.StatusCode;
                logEvent.Properties["exception"] = string.Concat(ex.GetType().FullName, " ", ex.Message + " Status description: " + statusDescription + " Begin request: " + beginRequest);
                logger.Log(logEvent);
                throw;
            }
        }
        #endregion
    }
}