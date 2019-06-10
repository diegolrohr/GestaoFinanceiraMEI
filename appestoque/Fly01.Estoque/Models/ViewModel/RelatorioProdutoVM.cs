using Newtonsoft.Json;
using System;

namespace Fly01.Estoque.Models.ViewModel
{
    [Serializable]
    public class RelatorioProdutoVM
    {
        
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("tipoProduto")]
        public string TipoProduto { get; set; }

        [JsonProperty("grupoProduto")]
        public string GrupoProduto { get; set; }

        [JsonProperty("unidadeMedida")]
        public string UnidadeMedida { get; set; }

        [JsonProperty("ncm")]
        public string Ncm{ get; set; }

        [JsonProperty("enquadramentoLegalIPI")]
        public string EnquadramentoLegalIPI { get; set; }

        [JsonProperty("origemMercadoria")]
        public string OrigemMercadoria { get; set; }

        [JsonProperty("quantidade")]
        public string Quantidade { get; set; }

        [JsonProperty("valorCusto")]
        public string ValorCusto { get; set; }

        [JsonProperty("valorVenda")]
        public string ValorVenda { get; set; }

        [JsonProperty("imprimirQuantidade")]
        public string ImprimirQuantidade { get; set; }

        [JsonProperty("imprimirValorCusto")]
        public string ImprimirValorCusto { get; set; }

        [JsonProperty("imprimirValorVenda")]
        public string ImprimirValorVenda { get; set; }

        [JsonProperty("imprimirNCM")]
        public string ImprimirNCM { get; set; }

        [JsonProperty("imprimirEnquadramentoIPI")]
        public string ImprimirEnquadramentoIPI { get; set; }

        [JsonProperty("imprimirOrigemMercadoria")]
        public string ImprimirOrigemMercadoria { get; set; }
    }
}