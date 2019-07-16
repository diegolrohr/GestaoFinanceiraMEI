using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fly01.Core.Helpers
{
    public class ResponseDataVM<T>
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("hasError")]
        public bool HasError { get; set; }

        [JsonProperty("errors")]
        public List<ErrorInfoVM> Errors { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class ResponseDataNewGatewayVM<T>
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("hasError")]
        public bool HasError { get; set; }

        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }

    public class ErrorInfoVM
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }
    }

    /// <summary>
    /// ErrorInfo
    /// </summary>
    [Serializable]
    public class ErrorInfo
    {
        /// <summary>
        /// Deu Erro?
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// Código
        /// </summary>
        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        /// <summary>
        /// Mensagem
        /// </summary>
        [JsonProperty("errorMessage")]
        public string Message { get; set; }
    }
}
