using Fly01.Core.Entities.Domains.Enum;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class MovimentoPedidoCompra : PlataformaBase
    {
        public double Quantidade { get; set; }
        public int PedidoNumero { get; set; }
        public Guid ProdutoId { get; set; }
        public TipoCompraVenda TipoCompra { get; set; }
        public bool NFeRefComplementarIsDevolucao { get; set; }
        public bool IsNFeImportacao { get; set; }
        public string Serie { get; set; }
        public int Numero { get; set; }
        public virtual Produto Produto { get; set; }
    }
}