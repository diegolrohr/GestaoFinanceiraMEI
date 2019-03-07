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
    }
}