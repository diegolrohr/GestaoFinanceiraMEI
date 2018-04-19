using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public abstract class ResponseSimulacaoBaseVM<TParcela> where TParcela : CondicaoParcelamentoParcelaBaseVM
    {
        [JsonProperty("value")]
        public List<TParcela> Items { get; set; }
    }
}