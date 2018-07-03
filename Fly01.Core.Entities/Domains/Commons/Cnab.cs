using Fly01.Core.Entities.Domains.Enum;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Cnab : PlataformaBase
    {
        [Required]
        [JsonIgnore]
        public StatusCnab Status { get; set; }

        [NotMapped]
        [JsonProperty("status")]
        public string StatusRest
        {
            get { return Status.ToString(); }
            set { Status = (StatusCnab)System.Enum.Parse(typeof(StatusCnab), value); }
        }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DataEmissao { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DataVencimento { get; set; }

        [Required]
        public int NossoNumero { get; set; }

        [Required]
        public string NossoNumeroFormatado { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataDesconto { get; set; }

        public double ValorDesconto { get; set; }

        public double ValorBoleto { get; set; }

        public Guid? ContaBancariaCedenteId { get; set; }

        public Guid? ContaReceberId { get; set; }

        public Guid? ArquivoRemessaId { get; set; }

        public virtual ContaBancaria ContaBancariaCedente { get; set; }
        public virtual ContaReceber ContaReceber { get; set; }
        public virtual ArquivoRemessa ArquivoRemessa { get; set; }
    }
}