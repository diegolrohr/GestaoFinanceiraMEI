using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel.Base
{
    public  abstract class BatchBaseVM
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("totalProcessed")]
        public string TotalProcessed { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("items")]
        public List<BatchItemVM> Items { get; set; }
    }
}
