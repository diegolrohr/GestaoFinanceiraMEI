using System;
using Newtonsoft.Json;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Helpers.Attribute;
using System.Collections.Generic;

namespace Fly01.Financeiro.ViewModel
{
    public class CnabVM : DomainBaseVM
    {
        [JsonProperty("status")]
        [APIEnum("StatusCnab")]
        public string Status { get; set; }

        [JsonProperty("dataEmissao")]
        public DateTime DataEmissao { get; set; }

        [JsonProperty("dataVencimento")]
        public DateTime DataVencimento { get; set; }

        [JsonProperty("nossoNumero")]
        public string NossoNumero { get; set; }

        [JsonProperty("dataDesconto")]
        public DateTime DataDesconto { get; set; }

        [JsonProperty("valorDesconto")]
        public double ValorDesconto { get; set; }

        [JsonProperty("contaBancariaCedenteId")]
        public Guid? ContaBancariaCedenteId { get; set; }

        [JsonProperty("contaReceberId")]
        public Guid? ContaReceberId { get; set; }

        [JsonProperty("arquivoRemessaId")]
        public Guid? ArquivoRemessaId { get; set; }

        [JsonProperty("valorBoleto")]
        public double ValorBoleto { get; set; }

        #region Navigation Property

        [JsonProperty("contaReceber")]
        public virtual ContaReceberVM ContaReceber { get; set; }

        [JsonProperty("contaBancariaCedente")]
        public virtual ContaBancariaVM ContaBancariaCedente { get; set; }

        [JsonProperty("arquivoRemessa")]
        public virtual ArquivoRemessaVM ArquivoRemessa { get; set; }
        #endregion
    }
}