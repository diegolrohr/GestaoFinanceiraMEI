using Fly01.Core.Api.Domain;
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

        public double FCPBase { get; set; }

        public double FCPAliquota { get; set; }

        public double FCPValor { get; set; }

        public bool CalculaCOFINS { get; set; }

        public double COFINSBase { get; set; }

        public double COFINSAliquota { get; set; }

        public double COFINSValor { get; set; }

        public bool CalculaPIS { get; set; }

        public double PISBase { get; set; }

        public double PISAliquota { get; set; }

        public double PISValor { get; set; }

        #region Navigations Properties

        public virtual NotaFiscalItem NotaFiscalItem { get; set; }

        #endregion
    }
}
