using Newtonsoft.Json;
using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class TipoMovimento : PlataformaBase
    {
        public const int DescricaoMaxLength = 40;

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(DescricaoMaxLength, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Descricao { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public TipoEntradaSaida TipoEntradaSaida { get; set; }

        [NotMapped]
        [JsonProperty("tipoEntradaSaida")]
        public string TipoEntradaSaidaRest
        {
            get { return ((int)TipoEntradaSaida).ToString(); }
            set { TipoEntradaSaida = (TipoEntradaSaida)System.Enum.Parse(typeof(TipoEntradaSaida), value); }
        }
    }
}