using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using System;


namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public abstract class SubstituicaoTributariaBaseVM<TEstado, TNcm, TCest> : DomainBaseVM
        where TEstado : EstadoBaseVM
        where TNcm : NcmBaseVM
        where TCest : CestBaseVM<TNcm>
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
        public virtual TNcm Ncm { get; set; }

        [JsonProperty("estadoOrigem")]
        public virtual TEstado EstadoOrigem { get; set; }

        [JsonProperty("estadoDestino")]
        public virtual TEstado EstadoDestino { get; set; }

        [JsonProperty("cest")]
        public virtual TCest Cest { get; set; }

        #endregion
    }
}
