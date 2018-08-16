using Fly01.Core.ViewModels.Presentation.Commons;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.OrdemServico.ViewModel
{
    public class OrdemServicoItemVM : DomainBaseVM
    {
        [Required]
        public Guid OrdemServicoId { get; set; }

        [Required]
        public double Quantidade { get; set; }

        public double Valor { get; set; }

        public double Desconto { get; set; }

        [NotMapped]
        public double Total => Quantidade > 0 ? Math.Round(((Quantidade * Valor) - Desconto), 2, MidpointRounding.AwayFromZero) : Math.Round((Valor - Desconto), 2, MidpointRounding.AwayFromZero);

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        #region Navigation
        public virtual OrdemServicoVM OrdemServico { get; set; }
        #endregion
    }
}
