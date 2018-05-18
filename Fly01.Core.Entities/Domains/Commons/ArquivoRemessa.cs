using Fly01.Core.Entities.Domains.Enum;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ArquivoRemessa : PlataformaBase
    {
        [Required]
        public string Descricao { get; set; }

        [Required]
        public int TotalBoletos { get; set; }

        [Required]
        public double ValorTotal { get; set; }

        [Required]
        [JsonIgnore]
        public StatusArquivoRemessa StatusArquivoRemessa { get; set; }

        [NotMapped]
        [JsonProperty("statusArquivoRemessa")]
        public string StatusArquivoRemessaRest
        {
            get { return ((int)StatusArquivoRemessa).ToString(); }
            set { StatusArquivoRemessa = (StatusArquivoRemessa)System.Enum.Parse(typeof(StatusArquivoRemessa), value); }
        }

        [Required]
        public DateTime DataExportacao { get; set; }

        public DateTime? DataRetorno { get; set; }

    }
}
