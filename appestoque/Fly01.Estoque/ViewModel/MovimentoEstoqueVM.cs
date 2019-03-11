using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Estoque.ViewModel
{
    [Serializable]
    public class MovimentoEstoqueVM : DomainBaseVM
    {
        [JsonProperty("dataInclusao")]
        public DateTime DataInclusao { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("quantidadeMovimento")]
        public double? QuantidadeMovimento { get; set; }

        [JsonProperty("saldoAntesMovimento")]
        public double? SaldoAntesMovimento { get; set; }

        [JsonProperty("tipoMovimentoId")]
        public Guid? TipoMovimentoId { get; set; }

        [JsonProperty("produtoId")]
        public Guid ProdutoId { get; set; }

        [JsonProperty("inventarioId")]
        public Guid? InventarioId { get; set; }

        [JsonProperty("produto")]
        public virtual ProdutoVM Produto { get; set; }
        [JsonProperty("inventario")]
        public virtual InventarioVM Inventario { get; set; }
        [JsonProperty("tipoMovimento")]
        public virtual TipoMovimentoVM TipoMovimento { get; set; }

    }
}