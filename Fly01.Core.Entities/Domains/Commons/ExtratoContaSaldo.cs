using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ExtratoContaSaldo
    {
        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }

        [JsonProperty("contaBancariaDescricao")]
        public string ContaBancariaDescricao { get; set; }

        [JsonProperty("saldoConsolidado")]
        public double SaldoConsolidado { get; set; }
    }
}