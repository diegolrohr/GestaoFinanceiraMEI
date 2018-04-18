﻿using Fly01.Core.Entities.Domains;
using Fly01.Core.Entities.Domains.Commons;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Faturamento.Domain.Entities
{
    public abstract class OrdemVendaItem : PlataformaBase
    {
        [Required]
        public Guid OrdemVendaId { get; set; }

        [Required]
        public Guid GrupoTributarioId { get; set; }

        [Required]
        public double Quantidade { get; set; }

        [Required]
        public double Valor { get; set; }

        public double Desconto { get; set; }

        //AppDataContext model.builder ignore
        public double Total
        {
            get
            {
                return Math.Round(((Quantidade * Valor) - Desconto), 2, MidpointRounding.AwayFromZero);
            }
            set
            {

            }
        }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        #region Navigations Properties

        public virtual OrdemVenda OrdemVenda { get; set; }
        public virtual GrupoTributario GrupoTributario { get; set; }

        #endregion
    }
}
