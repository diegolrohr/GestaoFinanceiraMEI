using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.ViewModels
{
    [Serializable]
    public class KeyValueVM
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
