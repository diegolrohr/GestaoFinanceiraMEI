using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class GrupoProdutoVM : DomainBaseVM
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

        [JsonProperty("ncm")]
        public virtual NcmVM Ncm { get; set; }

        [JsonProperty("unidadeMedida")]
        public virtual UnidadeMedidaVM UnidadeMedida { get; set; }
    }
}