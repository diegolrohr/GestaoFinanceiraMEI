using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation
{
    [Serializable]
    public class BatchVM
    {
        [JsonProperty("id")]
        public string Id { get; set; }

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
