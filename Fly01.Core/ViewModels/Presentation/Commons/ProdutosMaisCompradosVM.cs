using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ProdutosMaisCompradosVM
    {
        [JsonProperty("codigoProduto")]
        public string CodigoProduto { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }

        [JsonProperty("unidadeMedida")]
        public string UnidadeMedida { get; set; }
    }
}
