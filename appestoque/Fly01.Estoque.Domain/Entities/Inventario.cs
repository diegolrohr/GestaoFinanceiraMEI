using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Api.Domain;
using Newtonsoft.Json;
using Fly01.Estoque.Domain.Enums;

namespace Fly01.Estoque.Domain.Entities
{
    public class Inventario : PlataformaBase
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DataUltimaInteracao { get; set; }

        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Descricao { get; set; }

        public InventarioStatus InventarioStatus { get; set; }

        #region Navigation

        //public virtual ICollection<InventarioItem> InventarioItens { get; set; }

        #endregion Navigation

        
    }
}