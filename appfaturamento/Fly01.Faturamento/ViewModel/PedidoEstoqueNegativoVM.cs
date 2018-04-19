using System;
using Newtonsoft.Json;

namespace Fly01.Faturamento.ViewModel
{
    [Serializable]
    public class PedidoEstoqueNegativoVM
    {
        [JsonProperty("produtoId")]
        public Guid ProdutoId { get; set; }

        [JsonProperty("quantPedido")]
        public double QuantPedido { get; set; }

        [JsonProperty("quantEstoque")]
        public double QuantEstoque { get; set; }

        [JsonProperty("saldoEstoque")]
        public double SaldoEstoque { get; set; }

        [JsonProperty("produtoDescricao")]
        public string ProdutoDescricao { get; set; }
    }
}