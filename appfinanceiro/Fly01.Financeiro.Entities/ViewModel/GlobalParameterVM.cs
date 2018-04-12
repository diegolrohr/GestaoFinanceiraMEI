using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class GlobalParameterVM : DomainBaseVM
    {
        [JsonProperty("stockControl")]
        public string StockControl { get; set; }
    }
}
