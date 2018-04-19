using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ResponseSimulacaoVM
    {
        [JsonProperty("value")]
        public List<CondicaoParcelamentoParcelaVM> Items { get; set; }
    }
}