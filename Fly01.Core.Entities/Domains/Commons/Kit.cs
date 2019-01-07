using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Kit : PlataformaBase
    {
        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }
    }
}
