using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Compras.Domain.Entities
{
    [NotMapped]
    public class ProdutosMaisComprados
    {
        [JsonProperty("codigoProduto")]
        public string CodigoProduto { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }
    }
}
