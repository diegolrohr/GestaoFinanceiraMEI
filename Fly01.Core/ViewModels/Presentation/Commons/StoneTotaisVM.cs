using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class StoneTotaisVM
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

        [JsonProperty("totalBrutoAntecipavelCurrency")]
        public string TotalBrutoAntecipavelCurrency
        {
            get
            {
                return TotalBrutoAntecipavel.ToString("C", AppDefaults.CultureInfoDefault);
            }            
        }

        [JsonProperty("totalLiquidoAntecipavel")]
        public double TotalLiquidoAntecipavel { get; set; }

        [JsonProperty("totalLiquidoAntecipavelCurrency")]
        public string TotalLiquidoAntecipavelCurrency
        {
            get
            {
                return TotalLiquidoAntecipavel.ToString("C", AppDefaults.CultureInfoDefault);
            }
        }
    }
}