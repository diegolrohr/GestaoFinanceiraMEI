using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.ViewModels.Commons
{
    public abstract class CidadeBaseVM<TEstado> : DomainBaseVM
        where TEstado: EstadoBaseVM
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("codigoIbge")]
        public string CodigoIbge { get; set; }

        [JsonProperty("estadoId")]
        public Guid EstadoId { get; set; }

        #region Navigation Properties
        [JsonProperty("estado")]
        public virtual TEstado Estado { get; set; }
        #endregion
    }
}
