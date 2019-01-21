using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class StoneAntecipacaoVM : StoneAntecipacaoBaseVM
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}
