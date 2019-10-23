using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ExtratoDetalhe
    {
        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }

        [JsonProperty("contaBancariaDescricao")]
        public string ContaBancariaDescricao { get; set; }

        [JsonProperty("data")]
        public DateTime DataMovimento { get; set; }

        [JsonIgnore]
        public DateTime DataInclusao { get; set; }

        [JsonProperty("contaFinanceiraNumero")]
        public string ContaFinanceiraNumero { get; set; }

        [JsonProperty("descricaoLancamento")]
        public string DescricaoLancamento { get; set; }

        [JsonProperty("pessoaNome")]
        public string PessoaNome { get; set; }

        [JsonProperty("valorLancamento")]
        public double ValorLancamento { get; set; }
    }
}