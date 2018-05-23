﻿using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ContaBancaria : PlataformaBase
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(150, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string NomeConta { get; set; }

        public string Agencia { get; set; }

        public string DigitoAgencia { get; set; }

        public string Conta { get; set; }

        public string DigitoConta { get; set; }

        public Guid BancoId { get; set; }

        public string CodigoBanco { get; set; }

        public virtual Banco Banco { get; set; }

        //public string Carteira { get; set; }

        //public string VariacaoCarteira { get; set; }

        //public int TipoCarteira { get; set; } //Boleto2Net.TipoCarteira

        //public int TipoEspecieDocumento { get; set; } //Boleto2Net.TipoEspecieDocumento
    }
}