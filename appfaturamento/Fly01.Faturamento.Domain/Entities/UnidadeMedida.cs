using System.ComponentModel.DataAnnotations;
using Fly01.Core.Entities.Domains;

namespace Fly01.Faturamento.Domain.Entities
{
    public class UnidadeMedida : DomainBase
    {
        [Required]
        [MaxLength(2)]
        public string Abreviacao { get; set; }

        [Required]
        [MaxLength(20)]
        public string Descricao { get; set; }
    }
}
