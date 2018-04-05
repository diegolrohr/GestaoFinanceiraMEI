using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fly01.Compras.Domain.Enums;
using Fly01.Core.Domain;

namespace Fly01.Compras.Domain.Entities
{
    public class Categoria : PlataformaBase
    {
        [Required]
        [StringLength(40)]
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
            set { TipoCarteira = (TipoCarteira)Enum.Parse(typeof(TipoCarteira), value); }
        }

        [JsonIgnore]
        public virtual Categoria CategoriaPai { get; set; }
    }
}