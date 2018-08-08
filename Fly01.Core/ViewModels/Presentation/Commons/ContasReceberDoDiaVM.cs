using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ContasReceberDoDiaVM
    {
        [JsonProperty("tipo")]
        public string Dia { get; set; }

        [JsonProperty("total")]
        public double? Total { get; set; }
    }
}
