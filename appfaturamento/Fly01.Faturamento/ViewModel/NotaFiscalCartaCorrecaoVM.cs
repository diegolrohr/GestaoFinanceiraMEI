using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Faturamento.ViewModel
{
    public class NotaFiscalCartaCorrecaoVM : DomainBaseVM
    {
        [JsonProperty("notaFiscalId")]
        public Guid NotaFiscalId { get; set; }

        [JsonProperty("idRetorno")]
        public string IdRetorno { get; set; }

        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("numero")]
        public int Numero { get; set; }

        [JsonProperty("mensagemCorrecao")]
        public string MensagemCorrecao { get; set; }

        [JsonProperty("mensagem")]
        public string Mensagem { get; set; }

        [JsonProperty("xml")]
        public string XML { get; set; }

        [JsonProperty("notaFiscal")]
        public virtual NotaFiscal NotaFiscal { get; set; }
    }
}