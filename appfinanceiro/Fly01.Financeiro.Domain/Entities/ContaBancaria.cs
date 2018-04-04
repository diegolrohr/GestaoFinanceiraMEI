using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Api.Domain;

namespace Fly01.Financeiro.Domain.Entities
{
    public class ContaBancaria : PlataformaBase
    {
        [JsonProperty("banco")]
        public virtual Banco Banco { get; set; }

        [JsonProperty("nomeConta")]
        [Display(Name = "Nome Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(150, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string NomeConta { get; set; }

        [Required]
        [JsonProperty("agencia")]
        public string Agencia { get; set; }

        [Required]
        [JsonProperty("digitoAgencia")]
        public string DigitoAgencia { get; set; }

        [Required]
        [JsonProperty("conta")]
        public string Conta { get; set; }

        [Required]
        [JsonProperty("digitoConta")]
        public string DigitoConta { get; set; }
        
        [Required]
        [JsonProperty("bancoId")]
        public Guid BancoId { get; set; }
    }
}