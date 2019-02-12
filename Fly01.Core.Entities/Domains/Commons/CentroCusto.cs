using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class CentroCusto : PlataformaBase
    {
        [Required]
        [StringLength(100)]
        public string Codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }
    }
}