using Fly01.Core.Attribute;
using Newtonsoft.Json;
using System;


namespace Fly01.Core.Entities.ViewModels.Commons
{
    public abstract class SubstituicaoTributariaBaseVM : DomainBaseVM
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

        #region NavigationProperties

        [JsonProperty("ncm")]
        public virtual NcmBaseVM Ncm { get; set; }

        [JsonProperty("estadoOrigem")]
        public virtual EstadoBaseVM EstadoOrigem { get; set; }

        [JsonProperty("estadoDestino")]
        public virtual EstadoBaseVM EstadoDestino { get; set; }

        [JsonProperty("cest")]
        public virtual CestBaseVM Cest { get; set; }

        #endregion
    }
}
