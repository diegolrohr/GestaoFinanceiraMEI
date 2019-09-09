using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.ViewModel
{
    public class BancoVM : DomainBaseVM
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }
    }
}