using Newtonsoft.Json;


namespace Fly01.Core.Entities.Domains.Commons
{
    public class ResponseConfiguracaoStone
    {
        [JsonProperty("automatic_rate")]
        public double TaxaAntecipacaoAutomatica { get; set; }

        [JsonProperty("spot_rate")]
        public double TaxaAntecipacaoPontual { get; set; }

        [JsonProperty("warranty_rate")]
        public double TaxaGarantia { get; set; }
        
        [JsonProperty("has_automatic_receivable_prepay")]
        public bool AntecipacaoAutomaticaAtivada { get; set; }

        [JsonProperty("is_blocked")]
        public bool Bloqueado { get; set; }
    }
}
