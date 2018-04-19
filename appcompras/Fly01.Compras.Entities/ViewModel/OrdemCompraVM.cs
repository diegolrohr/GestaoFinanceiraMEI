using System;
using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Compras.Entities.ViewModel
{
    [Serializable]
    public class OrdemCompraVM : DomainBaseVM
    {
        [JsonProperty("numero")]
        public int Numero { get; set; }

        [JsonProperty("status")]
        [APIEnum("StatusOrdemCompra")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("formaPagamentoId")]
        public Guid? FormaPagamentoId { get; set; }

        [JsonProperty("condicaoParcelamentoId")]
        public Guid? CondicaoParcelamentoId { get; set; }

        [JsonProperty("categoriaId")]
        public Guid? CategoriaId { get; set; }

        [JsonProperty("dataVencimento")]
        public DateTime? DataVencimento { get; set; }

        [JsonProperty("tipoOrdemCompra")]
        [APIEnum("TipoOrdemCompra")]
        public string TipoOrdemCompra { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("total")]
        public double? Total { get; set; }

        #region Navigation Properties

        [JsonProperty("condicaoParcelamento")]
        public virtual CondicaoParcelamentoVM CondicaoParcelamento { get; set; }
        [JsonProperty("formaPagamento")]
        public virtual FormaPagamentoVM FormaPagamento { get; set; }
        [JsonProperty("categoria")]
        public virtual CategoriaVM Categoria { get; set; }

        #endregion

    }
}