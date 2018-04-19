using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [NotMapped]
    public class PedidoProdutoEstoqueNegativo
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