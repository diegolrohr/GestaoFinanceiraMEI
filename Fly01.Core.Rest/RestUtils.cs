using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Fly01.Core.Rest
{
    public static class RestUtils
    {
        public const int DEFAULT_TIMEOUT = 45;
        public const string AUTH_HEADER = "Authorization";
        private static RestClient CreateRestClient(string baseUrl, int timeout = DEFAULT_TIMEOUT)
        {
            return new RestClient
            {
                BaseUrl = new Uri(baseUrl),
                Timeout = timeout * 1000
            };
        }
        private static RestRequest CreateJsonRequest(string resource, Method method)
        {
            RestRequest request = new RestRequest();
            request.Resource = resource;
            request.Method = method;
            request.RequestFormat = DataFormat.Json;
            return request;
        }

        #region DELETEs
        public static bool ExecuteDeleteRequest(string url, string resource, Dictionary<string, string> header, int timeout = DEFAULT_TIMEOUT)
        {
            HttpStatusCode statusCode;
            string statusDescription;
            return ExecuteDeleteRequest(url, resource, out statusCode, out statusDescription, header, timeout);
        }
        public static bool ExecuteDeleteRequest(string url, string resource, out HttpStatusCode statusCode, out string statusDescription, Dictionary<string, string> header, int timeout = DEFAULT_TIMEOUT)
        {
            try
            {
                RestClient client = RestUtils.CreateRestClient(url, timeout);
                RestRequest request = CreateJsonRequest(resource, Method.DELETE);

                if (header != null)
                    header.ToList().ForEach(x => request.AddHeader(x.Key, x.Value));

                IRestResponse response = client.Execute(request);
                statusCode = response.StatusCode;
                statusDescription = response.StatusDescription;
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.Accepted)
                {
                    return true;
                }
                else
                {
                    string errorMessage = !string.IsNullOrEmpty(response.Content) ? response.Content : response.ErrorMessage;
                    throw new ApiException(statusCode, errorMessage);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool ExecuteDeleteRequest(string url, string resource, out HttpStatusCode statusCode, out string statusDescription, Dictionary<string, string> header, Dictionary<string, string> queryString, int timeout = DEFAULT_TIMEOUT)
        {
            try
            {
                RestClient client = RestUtils.CreateRestClient(url, timeout);
                RestRequest request = CreateJsonRequest(resource, Method.DELETE);

                if (queryString != null)
                    queryString.ToList().ForEach(x => request.AddParameter(x.Key, x.Value, ParameterType.QueryString));

                if (header != null)
                    header.ToList().ForEach(x => request.AddHeader(x.Key, x.Value));

                IRestResponse response = client.Execute(request);
                statusCode = response.StatusCode;
                statusDescription = response.StatusDescription;
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.Accepted)
                {
                    return true;
                }
                else
                {
                    string errorMessage = !string.IsNullOrEmpty(response.Content) ? response.Content : response.ErrorMessage;
                    throw new ApiException(statusCode, errorMessage);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region GETs
        public static string ExecuteGetRequest(string url, string resource, out HttpStatusCode statusCode, out string statusDescription, Dictionary<string, string> header, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            string responseData;
            RestClient client = RestUtils.CreateRestClient(url, timeout);
            RestRequest request = CreateJsonRequest(resource, Method.GET);

            if (queryString != null)
                queryString.ToList().ForEach(x => request.AddParameter(x.Key, x.Value, ParameterType.QueryString));

            if (header != null)
                header.ToList().ForEach(x => request.AddHeader(x.Key, x.Value));

            IRestResponse response = client.Execute(request);
            statusCode = response.StatusCode;
            statusDescription = response.StatusDescription;
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.Accepted)
                responseData = response.Content;
            else
            {
                string errorMessage = !string.IsNullOrEmpty(response.Content) ? response.Content : response.ErrorMessage;
                throw new ApiException(statusCode, errorMessage);
            }
            return responseData;
        }
        public static string ExecuteGetRequest(string url, string resource, Dictionary<string, string> header, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            HttpStatusCode statusCode;
            string statusDescription;
            return ExecuteGetRequest(url, resource, out statusCode, out statusDescription, header, queryString, timeout);
        }
        public static T ExecuteGetRequest<T>(string url, string resource, out HttpStatusCode statusCode, out string statusDescription, Dictionary<string, string> header, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            string responseData = ExecuteGetRequest(url, resource, out statusCode, out statusDescription, header, queryString, timeout);
            return JsonConvert.DeserializeObject<T>(responseData);
        }
        public static T ExecuteGetRequest<T>(string url, string resource, Dictionary<string, string> header, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            HttpStatusCode statusCode;
            string statusDescription;
            return ExecuteGetRequest<T>(url, resource, out statusCode, out statusDescription, header, queryString, timeout);
        }
        public static T ExecuteGetNoAuthRequest<T>(string url, string resource, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            HttpStatusCode statusCode;
            string statusDescription;
            return ExecuteGetRequest<T>(url, resource, out statusCode, out statusDescription, null, queryString, timeout);
        }
        #endregion

        #region POSTs
        public static string ExecutePostRequest(string url, string resource, out HttpStatusCode statusCode, out string statusDescription, object json, Dictionary<string, string> header = null, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            string responseData = string.Empty;
            statusCode = HttpStatusCode.NoContent;
            statusDescription = string.Empty;

            RestClient client = RestUtils.CreateRestClient(url, timeout);
            RestRequest request = CreateJsonRequest(resource, Method.POST);

            if (json != null)
            {
                if (json is string)
                    request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                else
                    request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(json), ParameterType.RequestBody);
            }

            if (header != null)
                header.ToList().ForEach(x => request.AddHeader(x.Key, x.Value));

            if (queryString != null)
                queryString.ToList().ForEach(x => request.AddParameter(x.Key, x.Value, ParameterType.QueryString));

            IRestResponse response = client.Execute(request);
            statusCode = response.StatusCode;
            statusDescription = response.StatusDescription;

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.Accepted)
                responseData = response.Content;
            else
            {
                string errorMessage = !string.IsNullOrEmpty(response.Content) ? response.Content : response.ErrorMessage;
                throw new ApiException(statusCode, errorMessage);
            }
            return responseData;
        }
        public static string ExecutePostRequest(string url, string resource, object json, Dictionary<string, string> header = null, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            HttpStatusCode statusCode;
            string statusDescription;
            return ExecutePostRequest(url, resource, out statusCode, out statusDescription, json, header, queryString, timeout);

        }
        public static TReturn ExecutePostRequest<TReturn>(string url, string resource, out HttpStatusCode statusCode, out string statusDescription, object obj, Dictionary<string, string> header = null, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            string response = ExecutePostRequest(url, resource, out statusCode, out statusDescription, obj, header, queryString, timeout);
            return JsonConvert.DeserializeObject<TReturn>(response);
        }
        public static TReturn ExecutePostRequest<TReturn>(string url, string resource, object obj, Dictionary<string, string> header = null, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            HttpStatusCode statusCode;
            string statusDescription;
            return ExecutePostRequest<TReturn>(url, resource, out statusCode, out statusDescription, obj, header, queryString, timeout);
        }
        public static T ExecuteGeneratedAuthToken<T>(string url, string encodedBody)
        {
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("application/x-www-form-urlencoded", encodedBody, ParameterType.RequestBody);
            request.AddParameter("Content-Type", "application/x-www-form-urlencoded", ParameterType.HttpHeader);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.Accepted)
                return JsonConvert.DeserializeObject<T>(response.Content);

            string errorMessage = !string.IsNullOrEmpty(response.Content) ? response.Content : response.ErrorMessage;
            throw new ApiException(response.StatusCode, errorMessage);
        }
        public static T GenerateToken<T>(string url, string resource, Dictionary<string, string> headers, Dictionary<string, string> tokenParams)
        {
            RestClient client = new RestClient(url);
            //client.
            RestRequest request = new RestRequest(resource, Method.POST);
            //request.UseDefaultCredentials = true;
            //request.
            //request.Attempts = 10;
            //request.
            if (headers != null)
                headers.ToList().ForEach(h => request.AddHeader(h.Key, h.Value));
            //request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            if (tokenParams != null)
                tokenParams.ToList().ForEach(t => request.AddParameter(t.Key, t.Value));
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent
                || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.Accepted)
                return JsonConvert.DeserializeObject<T>(response.Content);

            string errorMessage = !string.IsNullOrEmpty(response.Content) ? response.Content : response.ErrorMessage;
            throw new ApiException(response.StatusCode, errorMessage);
        }
        #endregion

        #region PUTs
        public static string ExecutePutRequest(string url, string resource, object json, out HttpStatusCode statusCode, out string statusDescription, Dictionary<string, string> header, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            string responseData;

            RestClient client = RestUtils.CreateRestClient(url, timeout);
            RestRequest request = CreateJsonRequest(resource, Method.PUT);
            if (json != null)
            {
                if (json is string)
                    request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                else
                    request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(json), ParameterType.RequestBody);
            }

            if (queryString != null)
                queryString.ToList().ForEach(x => request.AddParameter(x.Key, x.Value, ParameterType.QueryString));

            if (header != null)
                header.ToList().ForEach(x => request.AddHeader(x.Key, x.Value));

            IRestResponse response = client.Execute(request);
            statusCode = response.StatusCode;
            statusDescription = response.StatusDescription;
            if (response.StatusCode == HttpStatusCode.OK)
                responseData = response.Content;
            else
            {
                string errorMessage = !string.IsNullOrEmpty(response.Content) ? response.Content : response.ErrorMessage;
                throw new ApiException(statusCode, errorMessage);
            }
            return responseData;
        }
        public static string ExecutePutRequest(string url, string resource, object json, Dictionary<string, string> header, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            HttpStatusCode statusCode;
            string statusDescription;
            return ExecutePutRequest(url, resource, json, out statusCode, out statusDescription, header, queryString, timeout);
        }
        public static TReturn ExecutePutRequest<TReturn>(string url, string resource, object obj, out HttpStatusCode statusCode, out string statusDescription, Dictionary<string, string> header, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            string response = ExecutePutRequest(url, resource, obj, out statusCode, out statusDescription, header, queryString, timeout);
            return JsonConvert.DeserializeObject<TReturn>(response);
        }
        public static TReturn ExecutePutRequest<TReturn>(string url, string resource, object obj, Dictionary<string, string> header, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            HttpStatusCode statusCode;
            string statusDescription;
            return ExecutePutRequest<TReturn>(url, resource, obj, out statusCode, out statusDescription, header, queryString, timeout);
        }
        #endregion

        #region PATCH
        public static string ExecutePatchRequest(string url, string resource, object json, out HttpStatusCode statusCode, out string statusDescription, Dictionary<string, string> header, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            string responseData;

            RestClient client = RestUtils.CreateRestClient(url, timeout);
            RestRequest request = CreateJsonRequest(resource, Method.PATCH);
            if (json != null)
            {
                if (json is string)
                    request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                else
                    request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(json), ParameterType.RequestBody);
            }

            if (queryString != null)
                queryString.ToList().ForEach(x => request.AddParameter(x.Key, x.Value, ParameterType.QueryString));

            if (header != null)
                header.ToList().ForEach(x => request.AddHeader(x.Key, x.Value));

            IRestResponse response = client.Execute(request);
            statusCode = response.StatusCode;
            statusDescription = response.StatusDescription;
            if (response.StatusCode == HttpStatusCode.OK)
                responseData = response.Content;
            else
            {
                string errorMessage = !string.IsNullOrEmpty(response.Content) ? response.Content : response.ErrorMessage;
                throw new ApiException(statusCode, errorMessage);
            }
            return responseData;
        }
        public static string ExecutePatchRequest(string url, string resource, object json, Dictionary<string, string> header, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            HttpStatusCode statusCode;
            string statusDescription;
            return ExecutePatchRequest(url, resource, json, out statusCode, out statusDescription, header, queryString, timeout);
        }
        public static TReturn ExecutePatchRequest<TReturn>(string url, string resource, object obj, out HttpStatusCode statusCode, out string statusDescription, Dictionary<string, string> header, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            string response = ExecutePatchRequest(url, resource, obj, out statusCode, out statusDescription, header, queryString, timeout);
            return JsonConvert.DeserializeObject<TReturn>(response);
        }
        public static TReturn ExecutePatchRequest<TReturn>(string url, string resource, object obj, Dictionary<string, string> header, Dictionary<string, string> queryString = null, int timeout = DEFAULT_TIMEOUT)
        {
            HttpStatusCode statusCode;
            string statusDescription;
            return ExecutePatchRequest<TReturn>(url, resource, obj, out statusCode, out statusDescription, header, queryString, timeout);
        }
        #endregion

        public static Dictionary<string, string> GetAuthHeader(string authorization)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add(AUTH_HEADER, authorization);
            return header;
        }
    }
}
