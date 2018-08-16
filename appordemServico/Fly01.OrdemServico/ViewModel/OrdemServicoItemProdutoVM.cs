using Fly01.Core.ViewModels.Presentation.Commons;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.OrdemServico.ViewModel
{
    public class OrdemServicoItemProdutoVM : OrdemServicoItemVM
    {
        [Required]
        public Guid ProdutoId { get; set; }

        #region Navigation
        public virtual ProdutoVM Produto { get; set; }
        #endregion
    }
}
