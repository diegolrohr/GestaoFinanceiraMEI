﻿using Fly01.Core.Entities.Domains;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Faturamento.Domain.Entities
{
    public class NotaFiscalItemTributacao : PlataformaBase
    {
        [Required]
        public Guid NotaFiscalItemId { get; set; }

        public double FreteValorFracionado { get; set; }

        public bool CalculaICMS { get; set; }

        public double ICMSBase { get; set; }

        public double ICMSAliquota { get; set; }

        public double ICMSValor { get; set; }

        public bool CalculaIPI { get; set; }

        public double IPIBase { get; set; }

        public double IPIAliquota { get; set; }

        public double IPIValor { get; set; }

        public bool CalculaST { get; set; }

        public double STBase { get; set; }

        public double STAliquota { get; set; }

        public double STValor { get; set; }

        public bool CalculaCOFINS { get; set; }

        public double COFINSBase { get; set; }

        public double COFINSAliquota { get; set; }

        public double COFINSValor { get; set; }

        public bool CalculaPIS { get; set; }

        public double PISBase { get; set; }

        public double PISAliquota { get; set; }

        public double PISValor { get; set; }

        #region FCP

        public double FCPBase { get; set; }

        public double FCPAliquota { get; set; }

        public double FCPValor { get; set; }
        #endregion FCP

        #region FCP ST
        /// <summary>
        /// Valor da Base de Cálculo do FCP retido por Substituição Tributária
        /// </summary>
        public double FCPSTBase { get; set; }

        /// <summary>
        /// Percentual do FCP retido por Substituição Tributária
        /// </summary>
        public double FCPSTAliquota { get; set; }

        /// <summary>
        /// Valor do FCP retido por Substituição Tributária
        /// </summary>
        public double FCPSTValor { get; set; }
                        
        #endregion


        #region Navigations Properties

        public virtual NotaFiscalItem NotaFiscalItem { get; set; }

        #endregion
    }
}
