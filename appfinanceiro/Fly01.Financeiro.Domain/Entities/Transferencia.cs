using Fly01.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Financeiro.Domain.Entities
{
    public class Transferencia : PlataformaBase
    {
        [Required]
        public Movimentacao MovimentacaoOrigem { get; set; }

        [Required]
        public Movimentacao MovimentacaoDestino { get; set; }
    }
}