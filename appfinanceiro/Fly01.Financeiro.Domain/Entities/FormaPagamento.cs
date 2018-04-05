using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fly01.Core.Domain;
using Fly01.Financeiro.Domain.Enums;

namespace Fly01.Financeiro.Domain.Entities
{
    public class FormaPagamento : PlataformaBase
    {
        [JsonProperty("descricao")]
        [Display(Name = "Descrição")]
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
            set { TipoFormaPagamento = (TipoFormaPagamento)Enum.Parse(typeof(TipoFormaPagamento), value); }
        }
   }
}