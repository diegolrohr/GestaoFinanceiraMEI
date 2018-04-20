using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fly01.Financeiro.ViewModel
{
    public class ResponseFluxoCaixaProjecaoVM
    {
        [JsonProperty("value")]
        public List<FluxoCaixaProjecaoVM> Values { get; set; }
    }

    public class FluxoCaixaProjecaoVM
    {
        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("totalRecebimentos")]
        public double TotalRecebimentos { get; set; }

        [JsonProperty("totalPagamentos")]
        public double TotalPagamentos { get; set; }

        [JsonProperty("saldoFinal")]
        public double SaldoFinal { get; set; }
    }
}