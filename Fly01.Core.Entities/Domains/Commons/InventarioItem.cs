using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class InventarioItem : PlataformaBase
    {
        //Saldo atual do produto
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public double SaldoInventariado { get; set; }

        //Produto feito Inventario
        public Guid ProdutoId { get; set; }

        //Inventario
        public Guid InventarioId { get; set; }

        public virtual Produto Produto { get; set; }

        public virtual Inventario Inventario { get; set; }
    }
}