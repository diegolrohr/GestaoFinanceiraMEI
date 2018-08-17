using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class OrdemServicoItemProduto : OrdemServicoItem
    {
        [Required]
        public Guid ProdutoId { get; set; }

        #region Navigation
        public virtual Produto Produto { get; set; }
        #endregion
    }
}
