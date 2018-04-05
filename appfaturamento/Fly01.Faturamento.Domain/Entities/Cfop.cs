using System.ComponentModel.DataAnnotations;
using Fly01.Core.Domain;
using Fly01.Faturamento.Domain.Enums;

namespace Fly01.Faturamento.Domain.Entities
{
    public class Cfop : DomainBase
    {
        [Required]
        public int Codigo { get; set; }

        [Required]
        [StringLength(400)]
        public string Descricao { get; set; }

        [Required]
        public TipoCFOP Tipo { get; set; }
    }
}