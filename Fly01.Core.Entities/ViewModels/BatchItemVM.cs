using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fly01.Core.Entities.ViewModels
{
    [JsonObject("items")]
    public class BatchItemVM
    {
        #region Public Properties

        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("operation")]
        public string Operation { get; set; }

        [JsonProperty("entries")]
        public List<BatchEntryVM> Entries { get; set; }

        #endregion
    }
}