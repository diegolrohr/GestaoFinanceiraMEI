using System.ComponentModel.DataAnnotations;
using Fly01.Core.Domain;

namespace Fly01.Faturamento.Domain.Entities
{
    public class NBS : DomainBase
    {
        [Required]
        public string Codigo { get; set; }

        [Required]
        [StringLength(600)]
        public string Descricao { get; set; }
    }
}