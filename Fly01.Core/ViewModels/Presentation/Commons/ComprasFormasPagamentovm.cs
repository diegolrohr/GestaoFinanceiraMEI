﻿using Fly01.Core.Entities.Domains.Enum;
using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ComprasFormasPagamentoVM
    {
        [JsonProperty("tipoformapagamento")]
        public string TipoFormaPagamento { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }
    }
}
