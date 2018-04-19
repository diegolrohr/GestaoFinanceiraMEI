using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class AggregatorSaldos
    {
        public double SumSaldoConsolidado { get; set; }

        public List<double> SaldoConsolidado { get; set; }

        public AggregatorSaldos()
        {
            SaldoConsolidado = new List<double>();

            SumSaldoConsolidado = default(double);
        }
    }

    public class ExtratoHistoricoSaldo
    {
        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }

        [JsonProperty("contaBancariaDescricao")]
        public string ContaBancariaDescricao { get; set; }

        [JsonProperty("saldos")]
        public List<ExtratoSaldoHistoricoItem> Saldos { get; set; }

        public ExtratoHistoricoSaldo()
        {
            this.Saldos = new List<ExtratoSaldoHistoricoItem>();
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