using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Domain;
using Newtonsoft.Json;

namespace Fly01.Estoque.Domain.Entities
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

        #region Navigations Properties
        public virtual Produto Produto { get; set; }

        public virtual Inventario Inventario { get; set; }
        #endregion
    }
}