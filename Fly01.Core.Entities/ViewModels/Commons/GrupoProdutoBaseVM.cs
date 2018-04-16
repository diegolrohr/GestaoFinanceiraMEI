using Fly01.Core.Entities.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.ViewModels.Commons
{
    public abstract class GrupoProdutoBaseVM<TNcm, TUnidadeMedida> : DomainBaseVM
        where TNcm : NcmBaseVM
        where TUnidadeMedida : UnidadeMedidaBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("aliquotaIpi")]
        public double AliquotaIpi { get; set; }

        [JsonProperty("tipoProduto")]
        [APIEnum("TipoProduto")]
        public string TipoProduto { get; set; }

        [JsonProperty("unidadeMedidaId")]
        public Guid? UnidadeMedidaId { get; set; }

        [JsonProperty("ncmId")]
        public Guid? NcmId { get; set; }

        #region Navigations Properties

        [JsonProperty("ncm")]
        public virtual TNcm Ncm { get; set; }

        [JsonProperty("unidadeMedida")]
        public virtual TUnidadeMedida UnidadeMedida { get; set; }

        #endregion
    }
}
