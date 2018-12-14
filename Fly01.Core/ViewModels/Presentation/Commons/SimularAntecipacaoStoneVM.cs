using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class SimularAntecipacaoStoneVM : StoneTokenBaseVM
    {
        [JsonProperty("valor")]
        public double Valor { get; set; }
    }
}
