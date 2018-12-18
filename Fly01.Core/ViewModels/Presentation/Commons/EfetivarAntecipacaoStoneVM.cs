using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class EfetivarAntecipacaoStoneVM : StoneTokenBaseVM
    {
        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("stoneBancoId")]
        public int StoneBancoId { get; set; }

        [JsonProperty("senha")]
        public string Senha { get; set; }
    }
}
