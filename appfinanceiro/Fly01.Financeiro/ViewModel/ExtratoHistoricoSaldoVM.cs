using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fly01.Financeiro.ViewModel
{
    public class ResponseExtratoHistoricoSaldoVM
    {
        [JsonProperty("value")]
        public ExtratoHistoricoSaldoVM Value { get; set; }
    }

    public class ExtratoHistoricoSaldoVM
    {
        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }

        [JsonProperty("contaBancariaDescricao")]
        public string ContaBancariaDescricao { get; set; }

        [JsonProperty("saldos")]
        public List<ExtratoSaldoHistoricoItem> Saldos { get; set; }

        public ExtratoHistoricoSaldoVM()
        {
            Saldos = new List<ExtratoSaldoHistoricoItem>();
        }
    }

    public class ExtratoSaldoHistoricoItem
    {
        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("totalPagamentos")]
        public double TotalPagamentos { get; set; }

        [JsonProperty("totalRecebimentos")]
        public double TotalRecebimentos { get; set; }

        [JsonProperty("saldoDia")]
        public double SaldoDia { get; set; }

        [JsonProperty("saldoConsolidado")]
        public double SaldoConsolidado { get; set; }
    }
}
