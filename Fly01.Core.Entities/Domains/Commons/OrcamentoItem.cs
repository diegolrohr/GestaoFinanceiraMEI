using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class OrcamentoItem : OrdemCompraItem
    {
        [Required]
        public Guid OrcamentoId { get; set; }

        [Required]
        public Guid FornecedorId { get; set; }

        public virtual Pessoa Fornecedor { get; set; }
        public virtual Orcamento Orcamento { get; set; }
    }
}
