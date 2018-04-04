using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class CNABEmissionVM : DomainBaseVM
    {
        [JsonProperty("initialBorderoId")]
        public string InitialBorderoId { get; set; }

        [JsonProperty("finalBorderoId")]
        public string FinalBorderoId { get; set; }

        [JsonProperty("parametersBankid")]
        public string ParametersBankId { get; set; }

        [JsonIgnore]
        public string ParametersBankName { get; set; }
    }
}