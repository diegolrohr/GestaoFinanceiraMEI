using System;
using Newtonsoft.Json;

namespace Fly01.Financeiro.ViewModel
{
    public class ExtratoDetalheVM
    {
        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }

        [JsonProperty("contaBancariaDescricao")]
        public string ContaBancariaDescricao { get; set; }

        [JsonProperty("data")]
        public DateTime DataMovimento { get; set; }

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