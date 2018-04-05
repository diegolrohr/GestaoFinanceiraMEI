using Fly01.Compras.Entities.ViewModel;
using Fly01.Core.Attribute;
using Fly01.Core.VM;
using Newtonsoft.Json;
using System;

namespace Fly01.Compras.Domain.Entities
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
        public virtual Compras.Entities.ViewModel.EstadoVM EstadoOrigem { get; set; }

        [JsonProperty("estadoDestino")]
        public virtual Compras.Entities.ViewModel.EstadoVM EstadoDestino { get; set; }

        [JsonProperty("cest")]
        public virtual CestVM Cest { get; set; }

        #endregion
    }
}
