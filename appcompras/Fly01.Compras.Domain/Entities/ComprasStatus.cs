using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Compras.Domain.Entities
{
    [NotMapped]
    public class ComprasStatus
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }
    }
}
