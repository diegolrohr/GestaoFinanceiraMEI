using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Fly01.Core.Api.Domain;

namespace Fly01.Estoque.Domain.Entities
{
    public class Estado : DomainBase
    {
        [Required]
        [StringLength(2)]
        public string Sigla { get; set; }

        [Required]
        [StringLength(20)]
        public string Nome { get; set; }

        [Required]
        [StringLength(35)]
        public string UtcId { get; set; }

        [StringLength(2)]
        public string CodigoIbge { get; set; }
    }
}
