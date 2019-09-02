using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [Table("Transferencia")]
    public class TransferenciaFinanceira : EmpresaBase
    {
        [Required]
        public MovimentacaoFinanceira MovimentacaoOrigem { get; set; }

        [Required]
        public MovimentacaoFinanceira MovimentacaoDestino { get; set; }
    }
}