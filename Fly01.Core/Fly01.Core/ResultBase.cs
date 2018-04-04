using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fly01.Core
{
    public class ResultBase<T>
    {
        [JsonProperty("@odata.count")]
        public int Total { get; set; }

        [JsonProperty("value")]
        public List<T> Data { get; set; }

        public bool HasNext
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_NextLink);
            }
        }

        [JsonProperty("@odata.context")]
        public string _Context { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string _NextLink { get; set; }
    }
}