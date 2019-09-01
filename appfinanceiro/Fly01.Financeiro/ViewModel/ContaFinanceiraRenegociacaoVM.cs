using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.ViewModel
{
    public class ContaFinanceiraRenegociacaoVM : DomainBaseVM
    {
        [JsonIgnore]
        public string ContasFinanceirasIds { get; set; }

        [JsonProperty("contasFinanceirasOrigemIds")]
        public List<Guid> ContasFinanceirasOrigemIds
        {
            get
            {
                var result = new List<Guid>();
                if (!String.IsNullOrWhiteSpace(this.ContasFinanceirasIds))
                {
                    ContasFinanceirasIds.Split(',').ToList<string>().ForEach(x =>
                    {
                        result.Add(Guid.Parse(x));
                    });
                }
                return result;
            }
        }

        [JsonProperty("pessoaId")]
        [Display(Name = "Pessoa")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid PessoaId { get; set; }

        [Display(Name = "Tipo da Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("tipoContaFinanceira")]
        public string TipoContaFinanceira { get; set; }

        [Display(Name = "Valor Acumulado")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("valorAcumulado")]
        public double ValorAcumulado { get; set; }

        [Display(Name = "Tipo Valor Diferença")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("tipoRenegociacaoValorDiferenca")]
        public string TipoRenegociacaoValorDiferenca { get; set; }

        [Display(Name = "Tipo do Cálculo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("tipoRenegociacaoCalculo")]
        public string TipoRenegociacaoCalculo { get; set; }

        [Display(Name = "Valor Diferença")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("valorDiferenca")]
        public double ValorDiferenca { get; set; }

        [Display(Name = "Valor Final")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("valorFinal")]
        public double ValorFinal { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("categoriaId")]
        public Guid CategoriaId { get; set; }

        [Display(Name = "Forma Pagamento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("formaPagamentoId")]
        public Guid FormaPagamentoId { get; set; }

        [Display(Name = "Condição Parcelamento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("condicaoParcelamentoId")]
        public Guid CondicaoParcelamentoId { get; set; }

        [Display(Name = "Data Emissão")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("dataEmissao")]
        public DateTime DataEmissao { get; set; }

        [Display(Name = "Data Vencimento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("dataVencimento")]
        public DateTime DataVencimento { get; set; }

        //pois tem a coluna do display data com o mesmo nome
        //tratamento front-end
        [JsonProperty("dtVencimento")]
        public DateTime? DtVencimento { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("descricaoRenegociacao")]
        public string DescricaoRenegociacao { get; set; }

        [JsonProperty("motivo")]
        public string Motivo { get; set; }

        [JsonProperty("categoria")]
        public virtual CategoriaVM Categoria { get; set; }

        [JsonProperty("condicaoParcelamento")]
        public virtual CondicaoParcelamentoVM CondicaoParcelamento { get; set; }

        [JsonProperty("pessoa")]
        public virtual PessoaVM Pessoa { get; set; }

        [JsonProperty("formaPagamento")]
        public virtual FormaPagamentoVM FormaPagamento { get; set; }
    }
}
