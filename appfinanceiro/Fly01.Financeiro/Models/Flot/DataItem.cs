using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.Flot
{
    public class DataItem
    {
        [JsonProperty("data")]
        public List<object[]>  Data { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }

        public DataItem()
        {
            Data = new List<object[]>();
        }
    }
}