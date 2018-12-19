﻿using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ResponseConsultaTotalStoneVM
    {
        [JsonProperty("saldoDevedor")]
        public double SaldoDevedor { get; set; }

        [JsonProperty("saldoDevedorCurrency")]
        public string SaldoDevedorCurrency
        {
            get
            {
                return SaldoDevedor.ToString("C", AppDefaults.CultureInfoDefault);
            }
            set
            { }
        }

        [JsonProperty("totalBrutoAntecipavel")]
        public double TotalBrutoAntecipavel { get; set; }

        [JsonProperty("totalAntecipavelCurrency")]
        public string TotalAntecipavelCurrency
        {
            get
            {
                return TotalBrutoAntecipavel.ToString("C", AppDefaults.CultureInfoDefault);
            }
            set
            { }
        }
    }
}