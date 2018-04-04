using System;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class NotaFiscalItemVM : DomainBaseVM
    {
        [JsonProperty("notaFiscalId")]
        public Guid NotaFiscalId { get; set; }

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

        #region NavigationProperties

        [JsonProperty("notaFiscal")]
        public virtual NotaFiscalVM NotaFiscal { get; set; }
        [JsonProperty("grupoTributario")]
        public virtual GrupoTributarioVM GrupoTributario { get; set; }

        #endregion
    }
}