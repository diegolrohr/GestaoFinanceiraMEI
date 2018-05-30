using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Compras.Domain.Entities
{
    [NotMapped]
    public class ComprasFormasPagamento
    {
        [JsonProperty("tipoformapagamento")]
        public string TipoFormaPagamento { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }
    }


}
