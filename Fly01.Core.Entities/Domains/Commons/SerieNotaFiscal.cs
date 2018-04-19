using Newtonsoft.Json;
using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class SerieNotaFiscal : PlataformaBase
    {
        [Required]
        [StringLength(3)]
        public string Serie { get; set; }

        [JsonIgnore]
        public TipoOperacaoSerieNotaFiscal? TipoOperacaoSerieNotaFiscal { get; set; }

        [NotMapped]
        [JsonProperty("tipoOperacaoSerieNotaFiscal")]
        public string TipoOperacaoSerieNotaFiscalRest
        {
            get { return ((int)TipoOperacaoSerieNotaFiscal).ToString(); }
            set { TipoOperacaoSerieNotaFiscal = (TipoOperacaoSerieNotaFiscal)System.Enum.Parse(typeof(TipoOperacaoSerieNotaFiscal), value); }
        }

        [Required]
        public int NumNotaFiscal { get; set; }

        [JsonIgnore]
        public StatusSerieNotaFiscal StatusSerieNotaFiscal { get; set; }

        [NotMapped]
        [JsonProperty("statusSerieNotaFiscal")]
        public string StatusSerieNotaFiscalRest
        {
            get { return ((int)StatusSerieNotaFiscal).ToString(); }
            set { StatusSerieNotaFiscal = (StatusSerieNotaFiscal)System.Enum.Parse(typeof(StatusSerieNotaFiscal), value); }
        }
    }
}