using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ResponseAntecipacaoStone
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("prepay_date")]
        public DateTime Data { get; set; }

        [JsonProperty("prepay_rate")]
        public double TaxaPontual { get; set; }

        [JsonProperty("creation_date")]
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Valor sempre vem em centavos
        /// </summary>
        [JsonProperty("total_prepay_net_amount")]
        public int LiquidoAnteciparCentavos { get; set; }

        public double LiquidoAntecipar
        {
            get
            {
                return (LiquidoAnteciparCentavos / 100);
            }
            set { }
        }

        /// <summary>
        /// Valor sempre vem em centavos
        /// </summary>
        [JsonProperty("total_net_amount")]
        public int SaldoLiquidoDisponivelCentavos { get; set; }

        public double SaldoLiquidoDisponivel
        {
            get
            {
                return (SaldoLiquidoDisponivelCentavos / 100);
            }
            set { }
        }
    }
}
