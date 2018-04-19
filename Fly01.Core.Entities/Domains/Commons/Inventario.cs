using System;
using Newtonsoft.Json;
using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Inventario : PlataformaBase
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DataUltimaInteracao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Descricao { get; set; }

        public InventarioStatus InventarioStatus { get; set; }
    }
}