using Fly01.Core.Entities.Domains.Enum;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class MovimentoOrdemVenda : PlataformaBase
    {
        public double Quantidade { get; set; }
        public int PedidoNumero { get; set; }
        public Guid ProdutoId { get; set; }
        public TipoCompraVenda TipoVenda { get; set; }
        public TipoNfeComplementar TipoNfeComplementar { get; set; }
        public bool NFeRefComplementarIsDevolucao { get; set; }        
    }
}