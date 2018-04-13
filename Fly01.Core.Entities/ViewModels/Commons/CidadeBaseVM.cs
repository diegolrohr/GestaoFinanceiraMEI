using Newtonsoft.Json;

namespace Fly01.Core.Entities.ViewModels.Commons
{
    public abstract class CidadeBaseVM : DomainBaseVM
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("codigoIbge")]
        public string CodigoIbge { get; set; }

        [JsonProperty("estadoId")]
        public int EstadoId { get; set; }

        #region Navigation Properties
        [JsonProperty("estado")]
        public virtual EstadoBaseVM Estado { get; set; }
        #endregion
    }
}
