using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class ResponseExtratoContaSaldoVM
    {
        [JsonProperty("value")]
        public List<ExtratoContaSaldoVM> Values { get; set; }
    }

    public class ExtratoContaSaldoVM
    {
        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }

        [JsonProperty("contaBancariaDescricao")]
        public string ContaBancariaDescricao { get; set; }

        [JsonProperty("saldoConsolidado")]
        public double SaldoConsolidado { get; set; }
}
}
