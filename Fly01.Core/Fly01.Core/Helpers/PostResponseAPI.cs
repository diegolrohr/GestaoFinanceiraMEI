using Newtonsoft.Json;
using System;

namespace Fly01.Core.Helpers
{
    /// <summary>
    /// Reposta de Cadastros
    /// </summary>
    [Serializable]
    public class PostResponseAPI
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public PostResponseAPI()
        {
            Error = new ErrorInfo();
        }

        /// <summary>
        /// Informações de Erro
        /// </summary>
        public ErrorInfo Error { get; set; }

        /// <summary>
        /// Identificador do Registro Inserido / Alterado
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}