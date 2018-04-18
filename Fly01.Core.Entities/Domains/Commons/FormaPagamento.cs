using Fly01.Core.Entities.Domains.Enum;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class FormaPagamento : PlataformaBase
    {
        [JsonProperty("descricao")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Descricao { get; set; }

        [Required]
        [JsonIgnore]
        public TipoFormaPagamento TipoFormaPagamento { get; set; }

        [NotMapped]
        [JsonProperty("tipoFormaPagamento")]
        public string TipoFormaPagamentoRest
        {
            get { return ((int)TipoFormaPagamento).ToString(); }
            set { TipoFormaPagamento = (TipoFormaPagamento)System.Enum.Parse(typeof(TipoFormaPagamento), value); }
        }
    }
}
