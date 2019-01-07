using Newtonsoft.Json;
using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Kit : PlataformaBase
    {
        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }
    }
}
