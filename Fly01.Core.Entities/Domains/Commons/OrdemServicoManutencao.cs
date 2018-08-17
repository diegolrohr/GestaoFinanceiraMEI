using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class OrdemServicoManutencao : PlataformaBase
    {
        [Required]
        public Guid OrdemServicoId { get; set; }

        [Required]
        public Guid ProdutoId { get; set; }

        [Required]
        public double Quantidade { get; set; }

        public string Observacao { get; set; }

        #region Navigation
        public virtual OrdemServico OrdemServico { get; set; }
        public virtual Produto Produto { get; set; }
        #endregion
    }
}
