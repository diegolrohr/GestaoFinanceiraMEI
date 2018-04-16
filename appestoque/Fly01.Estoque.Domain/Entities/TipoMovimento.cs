using System.ComponentModel.DataAnnotations;
using Fly01.Core.Entities.Domains;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Estoque.Domain.Entities
{
    public class TipoMovimento : PlataformaBase
    {
        public const int DescricaoMaxLength = 40;

        [StringLength(DescricaoMaxLength, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Descricao { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public TipoEntradaSaida TipoEntradaSaida { get; set; }

        [NotMapped]
        [JsonProperty("tipoEntradaSaida")]
        public string TipoEntradaSaidaRest
        {
            get { return ((int)TipoEntradaSaida).ToString(); }
            set { TipoEntradaSaida = (TipoEntradaSaida)Enum.Parse(typeof(TipoEntradaSaida), value); }
        }

    }
}