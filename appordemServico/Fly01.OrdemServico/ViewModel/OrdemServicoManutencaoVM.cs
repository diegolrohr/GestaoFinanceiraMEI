using Fly01.Core.ViewModels.Presentation.Commons;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.OrdemServico.ViewModel
{
    public class OrdemServicoManutencaoVM : DomainBaseVM
    {
        [Required]
        public Guid OrdemServicoId { get; set; }

        [Required]
        public Guid ProdutoId { get; set; }

        [Required]
        public double Quantidade { get; set; }

        public string Observacao { get; set; }

        #region Navigation
        public virtual OrdemServicoVM OrdemServico { get; set; }
        public virtual ProdutoVM Produto { get; set; }
        #endregion
    }
}
