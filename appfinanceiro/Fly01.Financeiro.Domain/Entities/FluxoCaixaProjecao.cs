﻿using Newtonsoft.Json;
using System;

namespace Fly01.Financeiro.Domain.Entities
{
    public class FluxoCaixaProjecao
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
