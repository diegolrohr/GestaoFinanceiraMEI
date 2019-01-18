using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class MinhaContaResponseVM
    {
        [JsonProperty("hasError")]
        public bool HasError { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("data")]
        public MinhaContaVM[] Data { get; set; }
    }
}

