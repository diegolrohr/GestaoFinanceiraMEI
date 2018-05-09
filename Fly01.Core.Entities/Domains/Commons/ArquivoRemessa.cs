using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ArquivoRemessa : PlataformaBase
    {
        [Required]
        public int NumeroArquivo { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public int TotalBoletos { get; set; }

        [Required]
        public double ValorTotal { get; set; }

        [Required]
        public string StatusArquivoRemessa { get; set; }

        [Required]
        public DateTime DataExportacao { get; set; }

        public DateTime DataRetorno { get; set; }

    }
}
