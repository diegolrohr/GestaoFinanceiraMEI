using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.ViewModel
{
    [Serializable]
    public class ContaBancariaVM : DomainBaseVM
    {
        [JsonProperty("codigoDV")]
        [Display(Name = "CódigoDV")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CodigoDV { get; set; }

        [JsonProperty("bancoId")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Banco")]
        public Guid? BancoId { get; set; }

        [JsonProperty("banco")]
        [Display(Name = "Banco")]
        public virtual BancoVM Banco { get; set; }

        //Número da agência
        [JsonProperty("agencia")]
        [Display(Name = "Agência")]
        [StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Agencia { get; set; }

        //Informe o dígito da agência.
        [JsonProperty("digitoAgencia")]
        [Display(Name = "Dígito da Agência")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DigitoAgencia { get; set; }

        //Número da conta
        [JsonProperty("conta")]
        [Display(Name = "Conta")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Conta { get; set; }

        //Informe o dígito da conta.
        [JsonProperty("digitoConta")]
        [Display(Name = "Dígito da Conta")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DigitoConta { get; set; }

        //Nome do banco
        [JsonProperty("nomeConta")]
        [Display(Name = "Nome da Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string NomeConta { get; set; }

        [JsonProperty("valorInicial")]
        public double? ValorInicial { get; set; }

    }
}