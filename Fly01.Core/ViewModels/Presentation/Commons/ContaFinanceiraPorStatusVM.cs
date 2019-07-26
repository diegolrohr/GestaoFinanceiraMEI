using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ContaFinanceiraPorStatusVM
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("quantidade")]
        public int Quantidade { get; set; }

        [JsonProperty("quantidadetotal")]
        public int QuantidadeTotal { get; set; }

        [JsonProperty("valortotal")]
        public double Valortotal { get; set; }
    }
}
