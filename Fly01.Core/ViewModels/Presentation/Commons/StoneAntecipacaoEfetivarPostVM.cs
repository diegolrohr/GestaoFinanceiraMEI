using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class StoneAntecipacaoEfetivarPostVM : StoneTokenBaseVM
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("stoneBancoId")]
        public int StoneBancoId { get; set; }

        [JsonProperty("senha")]
        public string Senha { get; set; }
    }
}
