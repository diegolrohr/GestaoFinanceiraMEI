using Newtonsoft.Json;

namespace Fly01.Core.VM
{
    public class CidadeVM
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("codigoIbge")]
        public string CodigoIbge { get; set; }

        [JsonProperty("estadoId")]
        public int EstadoId { get; set; }

        #region Navigation Properties
        [JsonProperty("estado")]
        public virtual EstadoVM Estado { get; set; }
        #endregion
    }
}
