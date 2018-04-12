using System.ComponentModel.DataAnnotations;
using Fly01.Compras.Domain.Enums;
using Fly01.Core.Entities.Domains;

namespace Fly01.Compras.Domain.Entities
{
    public class Cfop : DomainBase
    {
        [Required]
        public int Codigo { get; set; }

        [Required]
        [MaxLength(400)]
        public string Descricao { get; set; }

        [Required]
        public TipoCFOP Tipo { get; set; }
    }
}