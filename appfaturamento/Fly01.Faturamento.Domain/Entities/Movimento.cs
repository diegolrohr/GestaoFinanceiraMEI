using System;
using Fly01.Core.Domain;

namespace Fly01.Faturamento.Domain.Entities
{
    public class Movimento : PlataformaBase
    {
        public double? QuantidadeMovimento { get; set; }

        public string Observacao { get; set; }
        public double? SaldoAntesMovimento { get; set; }
        public Guid? TipoMovimentoId { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid? InventarioId { get; set; }
    }
}
