using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class DashboardContaPagarVM : DomainBaseVM
    {

    }
    public class DashboardContaPagarChartVM
    {
        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }
    }
    public class ContasaPagarDoDiaVM
    {
        [JsonProperty("vencimento")]
        public string Vencimento { get; set; }

        [JsonProperty("descricao")]
        public string Descrição { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}



