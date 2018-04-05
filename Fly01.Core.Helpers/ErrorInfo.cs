using Newtonsoft.Json;
using System;

namespace Fly01.Core.Helpers
{
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
