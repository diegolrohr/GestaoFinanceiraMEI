﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public abstract class OrdemCompraItem : PlataformaBase
    {
        [Required]
        public Guid ProdutoId { get; set; }

        [Required]
        public double Quantidade { get; set; }

        [Required]
        public double Valor { get; set; }

        public double Desconto { get; set; }

        public double Total
        {
            get
            {
                return Math.Round(((Quantidade * Valor) - Desconto), 2, MidpointRounding.AwayFromZero);
            }
            set
            { }
        }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        public virtual Produto Produto { get; set; }
    }
}