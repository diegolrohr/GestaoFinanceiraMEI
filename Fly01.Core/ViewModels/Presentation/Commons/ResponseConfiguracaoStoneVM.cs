using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ResponseConfiguracaoStoneVM
    {
        [JsonProperty("taxaAntecipacaoAutomatica")]
        public double TaxaAntecipacaoAutomatica { get; set; }

        [JsonProperty("taxaAntecipacaoPontual")]
        public double TaxaAntecipacaoPontual { get; set; }

        [JsonProperty("taxaAntecipacaoPontualCurrency")]
        public string TaxaAntecipacaoPontualCurrency
        {
            get
            {
                return TaxaAntecipacaoPontual.ToString("N", AppDefaults.CultureInfoDefault) + "%";
            }
            set
            { }
        }

        [JsonProperty("taxaGarantia")]
        public double TaxaGarantia { get; set; }

        [JsonProperty("taxaGarantiaPercent")]
        public string TaxaGarantiaPercent
        {
            get
            {
                return TaxaGarantia.ToString("N", AppDefaults.CultureInfoDefault) + "%";
            }
            set
            { }
        }

        [JsonProperty("antecipacaoAutomaticaAtivada")]
        public bool AntecipacaoAutomaticaAtivada { get; set; }

        [JsonProperty("bloqueado")]
        public bool Bloqueado { get; set; }        
    }
}