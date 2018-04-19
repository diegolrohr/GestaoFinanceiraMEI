using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ContaBancaria : PlataformaBase
    {
        [JsonProperty("nomeConta")]
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

        [JsonProperty("banco")]
        public virtual Banco Banco { get; set; }
    }
}