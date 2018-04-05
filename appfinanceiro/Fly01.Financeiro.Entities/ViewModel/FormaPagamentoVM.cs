using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;
using Fly01.Core.Attribute;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class FormaPagamentoVM : DomainBaseVM
    {
        [JsonProperty("descricao")]
        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(30, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]

        public string Descricao { get; set; }

        [JsonProperty("tipoFormaPagamento")]
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        [APIEnum("TipoFormaPagamento")]
        public string TipoFormaPagamento { get; set; }

        //[JsonIgnore]
        ////CardBrandList
        //public string ListaBandeiras { get; set; }

        //[JsonIgnore]
        ////PaymentProviderList
        //public string ListaRedesCartao { get; set; }

        //[JsonIgnore]
        //public string ListaCondicoesParcelamento { get; set; }
    }
}