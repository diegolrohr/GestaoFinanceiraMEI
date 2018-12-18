using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ResponseAntecipacaoStoneVM
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("taxaPontual")]
        public double TaxaPontual { get; set; }

        [JsonProperty("taxaPontualPercent")]
        public string TaxaPontualPercent
        {
            get
            {
                return TaxaPontual.ToString("N", AppDefaults.CultureInfoDefault)+"%";
            }
            set
            { }
        }

        [JsonProperty("dataCriacao")]
        public DateTime DataCriacao { get; set; }

        [JsonProperty("liquidoAntecipar")]
        public double LiquidoAntecipar { get; set; }

        [JsonProperty("liquidoAnteciparCurrency")]
        public string LiquidoAnteciparCurrency
        {
            get
            {
                return LiquidoAntecipar.ToString("C", AppDefaults.CultureInfoDefault);
            }
            set
            { }
        }

        [JsonProperty("brutoAntecipar")]
        public double BrutoAntecipar { get; set; }

        [JsonProperty("brutoAnteciparCurrency")]
        public string BrutoAnteciparCurrency
        {
            get
            {
                return BrutoAntecipar.ToString("C", AppDefaults.CultureInfoDefault);
            }
            set
            { }
        }

        [JsonProperty("saldoLiquidoDisponivel")]
        public double SaldoLiquidoDisponivel { get; set; }        

        [JsonProperty("saldoLiquidoDisponivelCurrency")]
        public string SaldoLiquidoDisponivelCurrency
        {
            get
            {
                return SaldoLiquidoDisponivel.ToString("C", AppDefaults.CultureInfoDefault);
            }
            set
            { }
        }
    }
}
