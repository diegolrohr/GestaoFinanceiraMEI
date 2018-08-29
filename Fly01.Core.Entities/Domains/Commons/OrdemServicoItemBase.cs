using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public abstract class OrdemServicoItemBase : PlataformaBase
    {
        [Required]
        public Guid OrdemServicoId { get; set; }

        [Required]
        public double Quantidade { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        #region Navigation
        public virtual OrdemServico OrdemServico { get; set; }
        #endregion
    }
}
