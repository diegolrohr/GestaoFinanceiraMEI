using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class DashboardVM : DomainBaseVM
    {

    }

    public class DashboardStatusVM
    {
        [JsonProperty("status")]
        public String Status { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }
    }

    public class DashboardFormaPagamentoVM
    {
        [JsonProperty("tipoformapagamento")]
        public String TipoFormaPagamento { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }
    }

    public class DashboardGridVM
    {
        [JsonProperty("codigoProduto")]
        public string CodigoProduto { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }

        [JsonProperty("unidadeMedida")]
        public string UnidadeMedida { get; set; }
    }
}

