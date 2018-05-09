using System;
using Newtonsoft.Json;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Financeiro.ViewModel
{
    public class CnabVM : DomainBaseVM
    {
        [JsonProperty("numeroBoleto")]
        public int NumeroBoleto { get; set; }

        [JsonProperty("valorBoleto")]
        public double ValorBoleto { get; set; }

        [JsonProperty("valorDesconto")]
        public double ValorDesconto { get; set; }

        [JsonProperty("pessoaId")]
        public Guid? PessoaId { get; set; }

        [JsonProperty("bancoId")]
        public Guid? BancoCedenteId { get; set; }

        [JsonProperty("arquivoRemessaId")]
        public Guid? ArquivoRemessaId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("dataEmissao")]
        public DateTime DataEmissao { get; set; }

        [JsonProperty("dataVencimento")]
        public DateTime DataVencimento { get; set; }

        [JsonProperty("pessoa")]
        public virtual PessoaVM Pessoa { get; set; }

        [JsonProperty("banco")]
        public virtual BancoVM BancoCedente { get; set; }

        [JsonProperty("arquivoRemessa")]
        public virtual ArquivoRemessaVM ArquivoRemessa { get; set; }
    }
}