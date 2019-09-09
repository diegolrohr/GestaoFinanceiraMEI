using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class CidadeVM : EmpresaBaseVM
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("codigoIbge")]
        public string CodigoIbge { get; set; }

        [JsonProperty("estadoId")]
        public Guid EstadoId { get; set; }

        [JsonProperty("estado")]
        public virtual EstadoVM Estado { get; set; }
    }
}