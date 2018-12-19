using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class StoneAntecipacaoEfetivarVM : StoneAntecipacaoBaseVM
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}


