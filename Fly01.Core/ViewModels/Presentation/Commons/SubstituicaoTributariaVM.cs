using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class SubstituicaoTributariaVM : DomainBaseVM
    {
        [JsonProperty("ncmId")]
        public Guid NcmId { get; set; }

        [JsonProperty("estadoOrigemId")]
        public Guid EstadoOrigemId { get; set; }

        [JsonProperty("estadoDestinoId")]
        public Guid EstadoDestinoId { get; set; }

        [JsonProperty("mva")]
        public double Mva { get; set; }

        [APIEnum("TipoSubstituicaoTributaria")]
        [JsonProperty("tipoSubstituicaoTributaria")]
        public string TipoSubstituicaoTributaria { get; set; }

        [JsonProperty("cestId")]
        public Guid? CestId { get; set; }

        [JsonProperty("ncm")]
        public virtual NcmVM Ncm { get; set; }

        [JsonProperty("estadoOrigem")]
        public virtual EstadoVM EstadoOrigem { get; set; }

        [JsonProperty("estadoDestino")]
        public virtual EstadoVM EstadoDestino { get; set; }

        [JsonProperty("cest")]
        public virtual CestVM Cest { get; set; }
    }
}