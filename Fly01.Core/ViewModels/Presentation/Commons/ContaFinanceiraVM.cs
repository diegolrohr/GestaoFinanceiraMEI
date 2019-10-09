using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using Fly01.Core.Helpers.Attribute;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class ContaFinanceiraVM : EmpresaBaseVM
    {
        [JsonProperty("contaFinanceiraRepeticaoPaiId")]
        public Guid? ContaFinanceiraRepeticaoPaiId { get; set; }

        [JsonProperty("valorPrevisto")]
        public double ValorPrevisto { get; set; }

        [JsonProperty("valorPago")]
        public double? ValorPago { get; set; }

        [JsonProperty("categoriaId")]
        public Guid CategoriaId { get; set; }

        [JsonProperty("condicaoParcelamentoId")]
        public Guid CondicaoParcelamentoId { get; set; }

        [JsonProperty("pessoaId")]
        public Guid PessoaId { get; set; }

        [JsonProperty("dataEmissao")]
        public DateTime DataEmissao { get; set; }
        
        [JsonProperty("dataVencimento")]
        public DateTime DataVencimento { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("formaPagamentoId")]
        public Guid FormaPagamentoId { get; set; }

        [JsonProperty("statusContaBancaria")]
        [APIEnum("StatusContaBancaria")]
        public string StatusContaBancaria { get; set; }

        [JsonProperty("repetir")]
        public bool Repetir { get; set; }

        [JsonProperty("tipoPeriodicidade")]
        public string TipoPeriodicidade { get; set; }

        [JsonProperty("numeroRepeticoes")]
        public int? NumeroRepeticoes { get; set; }

        [JsonProperty("descricaoParcela")]
        public string DescricaoParcela { get; set; }

        [JsonProperty("saldo")]
        public double Saldo { get; set; }

        [JsonIgnore]
        public int DiasVencidos
        {
            get
            {
                return (DataVencimento < DateTime.Now.Date) ? Convert.ToInt32((DateTime.Now.Date - DataVencimento).TotalDays) : 0;
            }

        }

        [JsonProperty("numero")]
        public int Numero { get; set; }

        [JsonProperty("contaFinanceiraRepeticaoPai")]
        public virtual ContaFinanceiraVM ContaFinanceiraRepeticaoPai { get; set; }

        [JsonProperty("categoria")]
        public virtual CategoriaVM Categoria { get; set; }

        [JsonProperty("condicaoParcelamento")]
        public virtual CondicaoParcelamentoVM CondicaoParcelamento { get; set; }

        [JsonProperty("pessoa")]
        public virtual PessoaVM Pessoa { get; set; }

        [JsonProperty("formaPagamento")]
        public virtual FormaPagamentoVM FormaPagamento { get; set; }

        [JsonProperty("contaBancaria")]
        public virtual ContaBancaria ContaBancaria { get; set; }
    }
}