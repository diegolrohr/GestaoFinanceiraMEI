using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fly01.Core.Helpers
{
    public class ResultBaseFirst<T>
    {
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("lines")]
        public List<T> Data { get; set; }
        [JsonProperty("hasNext")]
        public bool HasNext { get; set; }
    }
}