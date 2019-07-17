using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using Fly01.Core.Helpers.Attribute;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class ContaFinanceiraVM : DomainBaseVM
    {
        [JsonProperty("contaFinanceiraRepeticaoPaiId")]
        public Guid? ContaFinanceiraRepeticaoPaiId { get; set; }

        [Display(Name = "Valor Previsto")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("valorPrevisto")]
        public double ValorPrevisto { get; set; }

        [Display(Name = "Valor Pago")]
        [JsonProperty("valorPago")]
        public double? ValorPago { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("categoriaId")]
        public Guid CategoriaId { get; set; }

        [Display(Name = "Condição Parcelamento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("condicaoParcelamentoId")]
        public Guid CondicaoParcelamentoId { get; set; }

        [Display(Name = "Pessoa")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("pessoaId")]
        public Guid PessoaId { get; set; }

        [Display(Name = "Data Emissão")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("dataEmissao")]
        public DateTime DataEmissao { get; set; }
        
        [Display(Name = "Data Vencimento")]
        [JsonProperty("dataVencimento")]
        public DateTime DataVencimento { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [Display(Name = "Observação")]
        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [Display(Name = "Forma Pagamento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("formaPagamentoId")]
        public Guid FormaPagamentoId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("statusContaBancaria")]
        [APIEnum("StatusContaBancaria")]
        public string StatusContaBancaria { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
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

        [JsonProperty("centroCustoId")]
        public Guid? CentroCustoId { get; set; }

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

        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }

        [JsonProperty("contaBancaria")]
        public virtual ContaBancaria ContaBancaria { get; set; }

        [JsonProperty("centroCusto")]
        public virtual CentroCustoVM CentroCusto { get; set; }

        [JsonIgnore]
        public string Titulo { get; set; }
    }
}