using System;
using Fly01.Core.Domain;

namespace Fly01.Faturamento.Domain.Entities
{
    public class MovimentoOrdemVenda : PlataformaBase
    {
        public double QuantidadeBaixa { get; set; }
        public int PedidoNumero { get; set; }
        public Guid ProdutoId { get; set; }
    }
}
