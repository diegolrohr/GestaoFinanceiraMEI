﻿using Fly01.Compras.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Compras.Domain.Entities
{
    public class FormaPagamento : PlataformaBase
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Descricao { get; set; }


        [Display(Name = "Forma Pagamento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public TipoFormaPagamento TipoFormaPagamento { get; set; }
    }
}
