using Fly01.Core.Entities.Domains.Enum;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class MovimentoOrdemVenda : PlataformaBase
    {
        public double Quantidade { get; set; }
        public int PedidoNumero { get; set; }
        public Guid ProdutoId { get; set; }
        public TipoFinalidadeEmissaoNFe TipoVenda { get; set; }

        public virtual Produto Produto { get; set; }
    }
}