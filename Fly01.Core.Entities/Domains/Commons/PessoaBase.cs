﻿using System;
using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class PessoaBase : PlataformaBase
    {
        [Required]
        [StringLength(100, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Nome { get; set; }

        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string TipoDocumento { get; set; }

        [StringLength(18, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CPFCNPJ { get; set; }

        [StringLength(8, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CEP { get; set; }

        [StringLength(50, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Endereco { get; set; }

        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Numero { get; set; }

        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Complemento { get; set; }

        [StringLength(30, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Bairro { get; set; }

        public Guid? CidadeId { get; set; }

        public Guid? EstadoId { get; set; }

        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Telefone { get; set; }

        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Celular { get; set; }

        [StringLength(45, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Contato { get; set; }

        [StringLength(100, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Observacao { get; set; }

        [StringLength(70, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string NomeComercial { get; set; }

        [StringLength(18, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string InscricaoEstadual { get; set; }

        [StringLength(18, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string InscricaoMunicipal { get; set; }

        public TipoIndicacaoInscricaoEstadual TipoIndicacaoInscricaoEstadual { get; set; }

        [Required]
        public bool ConsumidorFinal { get; set; }

        [Required]
        public bool Transportadora { get; set; }

        [Required]
        public bool Cliente { get; set; }

        [Required]
        public bool Fornecedor { get; set; }

        [Required]
        public bool Vendedor { get; set; }
    }
}