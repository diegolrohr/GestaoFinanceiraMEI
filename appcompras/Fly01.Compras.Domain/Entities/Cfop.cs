using System.ComponentModel.DataAnnotations;
using Fly01.Core.Api.Domain;
using Fly01.Compras.Domain.Enums;

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