using System;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.Core.Helpers
{
    /// <summary>
    /// Resposta Genérica
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ResponseAPI<T>
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public ResponseAPI()
        {
            Records = new List<T>();
            Error = new ErrorInfo();
            Total = 1;
        }

        /// <summary>
        /// Informações de Erro
        /// </summary>
        public ErrorInfo Error { get; set; }

        /// <summary>
        /// Lista dos Registros solicitados
        /// </summary>
        public List<T> Records { get; set; }

        /// <summary>
        /// Número de registros
        /// </summary>
        public int Count { get { return Records.Count(); } }

        /// <summary>
        /// Total de Registros
        /// </summary>
        public int Total { get; set; }
    }
}
