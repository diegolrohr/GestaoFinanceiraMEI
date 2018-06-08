using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class DashboardContaReceberVM : DomainBaseVM
    {

    }
    public class DashboardContaReceberStatusVM
    {
        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }
    }
    
    public class DashboardContaReceberFormaPagamentoVM
    {
        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }
    }
    
    public class DashboardContaReceberCategoriaVM
    {
        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }
    }
    public class DashboardContaReceberSaldoDiaVM
    {
        [JsonProperty("dia")]
        public string Dia { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }
    }
}



