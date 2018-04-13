using Newtonsoft.Json;

namespace Fly01.Core.Entities.ViewModels
{
    [JsonObject("entries")]
    public class BatchEntryVM
    {
        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public BatchDataVM Data { get; set; }

        [JsonProperty("return")]
        public BatchReturnVM Return { get; set; }

        [JsonProperty("defaultFields")]
        public object DefaultFields { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}