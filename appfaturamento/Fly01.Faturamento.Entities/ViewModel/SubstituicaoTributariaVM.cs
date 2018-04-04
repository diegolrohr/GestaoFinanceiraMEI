using Fly01.Core.Api;
using Fly01.Core.VM;
using Newtonsoft.Json;
using System;

namespace Fly01.Faturamento.Entities.ViewModel
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

        #region NavigationProperties

        [JsonProperty("ncm")]
        public virtual NCMVM Ncm { get; set; }

        [JsonProperty("estadoOrigem")]
        public virtual EstadoVM EstadoOrigem { get; set; }

        [JsonProperty("estadoDestino")]
        public virtual EstadoVM EstadoDestino { get; set; }

        [JsonProperty("cest")]
        public virtual CestVM Cest { get; set; }

        #endregion
    }
}
