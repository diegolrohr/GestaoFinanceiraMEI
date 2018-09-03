using System;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Compras.ViewModel
{
    [Serializable]
    public class NotaFiscalItemEntradaVM : DomainBaseVM
    {
        [JsonProperty("notaFiscalId")]
        public Guid NotaFiscalEntradaId { get; set; }

        [JsonProperty("grupoTributarioId")]
        public Guid GrupoTributarioId { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("desconto")]
        public double Desconto { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("notaFiscal")]
        public virtual NotaFiscalEntradaVM NotaFiscalEntrada { get; set; }

        [JsonProperty("grupoTributario")]
        public virtual GrupoTributarioVM GrupoTributario { get; set; }
    }
}