using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Estoque.ViewModel
{
    [Serializable]
    public class ImprimirMovimentoEstoqueVM : DomainBaseVM
    {
        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }

        [JsonProperty("saldoAntesMovimento")]
        public double SaldoAntesMovimento { get; set; }

        [JsonProperty("tipoMovimentoDescricao")]
        public string TipoMovimentoDescricao { get; set; }

        [JsonProperty("produtoDescricao")]
        public string ProdutoDescricao { get; set; }

        [JsonProperty("inventarioDescricao")]
        public string InventarioDescricao { get; set; }

    }
}