using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class OrdemServicoItemServico : OrdemServicoItem
    {
        [Required]
        public Guid ServicoId { get; set; }

        #region Navigation
        public virtual Servico Servico { get; set; }
        #endregion
    }
}
