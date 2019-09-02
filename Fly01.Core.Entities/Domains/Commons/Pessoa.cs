using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Pessoa : EmpresaBase
    {
        [Required]
        [StringLength(180, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Nome { get; set; }

        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string TipoDocumento { get; set; }

        [StringLength(18, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CPFCNPJ { get; set; }

        [StringLength(8, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CEP { get; set; }

        [StringLength(180, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Endereco { get; set; }

        [StringLength(60, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Numero { get; set; }

        [StringLength(500, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Complemento { get; set; }

        [StringLength(200, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Bairro { get; set; }

        public Guid? CidadeId { get; set; }

        public Guid? EstadoId { get; set; }

        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Telefone { get; set; }

        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Celular { get; set; }

        [StringLength(60, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Contato { get; set; }

        [StringLength(500, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Observacao { get; set; }

        [StringLength(100, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Email { get; set; }

        [StringLength(180, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string NomeComercial { get; set; }

        [Required]
        public bool Cliente { get; set; }

        [Required]
        public bool Fornecedor { get; set; }

        [JsonIgnore]
        public virtual Cidade Cidade { get; set; }

        [JsonIgnore]
        public virtual Estado Estado { get; set; }
    }
}