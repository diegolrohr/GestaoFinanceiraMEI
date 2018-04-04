using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Faturamento.Domain.Entities
{
    public class OrdemVendaServico : OrdemVendaItem
    {
        [Required]
        public Guid ServicoId { get; set; }

        #region Navigations Properties

        public virtual Servico Servico { get; set; }

        #endregion
    }
}
