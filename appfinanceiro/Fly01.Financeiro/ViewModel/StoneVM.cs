using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.ViewModel
{
    public class StoneVM : DomainBaseVM
    {
        [JsonProperty("valorRecebido")]
        public string ValorRecebido { get; set; }

        [JsonProperty("valorAntecipado")]
        public string ValorAntecipado { get; set; }

        [JsonProperty("taxa")]
        public string Taxa { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("senha")]
        public string Senha { get; set; }
    }
}