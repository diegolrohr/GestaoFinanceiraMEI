using Fly01.Core.Entities.Domains.Commons;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Compras.Domain.Entities
{
    public class OrcamentoItem : OrdemCompraItem
    {
        [Required]
        public Guid OrcamentoId { get; set; }

        [Required]
        public Guid FornecedorId { get; set; }

        #region Navigations Properties

        public virtual Pessoa Fornecedor { get; set; }
        public virtual Orcamento Orcamento { get; set; }

        #endregion
    }
}
