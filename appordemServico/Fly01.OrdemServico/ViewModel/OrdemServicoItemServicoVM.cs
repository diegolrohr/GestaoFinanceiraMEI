using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.OrdemServico.ViewModel
{
    public class OrdemServicoItemServicoVM : OrdemServicoItemVM
    {
        [Required]
        public Guid ServicoId { get; set; }

        #region Navigation
        public virtual ServicoVM Servico { get; set; }
        #endregion
    }
}
