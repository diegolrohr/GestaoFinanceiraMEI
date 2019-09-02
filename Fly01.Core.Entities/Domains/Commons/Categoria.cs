using Newtonsoft.Json;
using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Categoria : EmpresaBase
    {
        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }

        public Guid? CategoriaPaiId { get; set; }

        [Required]
        [JsonIgnore]
        public TipoCarteira TipoCarteira { get; set; }

        [NotMapped]
        [JsonProperty("tipoCarteira")]
        public string TipoCarteiraRest
        {
            get { return ((int)TipoCarteira).ToString(); }
            set { TipoCarteira = (TipoCarteira)System.Enum.Parse(typeof(TipoCarteira), value); }
        }

        [JsonIgnore]
        public virtual Categoria CategoriaPai { get; set; }
    }
}
