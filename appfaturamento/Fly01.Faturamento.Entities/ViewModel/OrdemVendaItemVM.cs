using System;
using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class OrdemVendaItemVM : DomainBaseVM
    {
        [JsonProperty("ordemVendaId")]
        public Guid OrdemVendaId { get; set; }

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

        [JsonProperty("ordemVenda")]
        public virtual OrdemVendaVM OrdemVenda { get; set; }
        [JsonProperty("grupoTributario")]
        public virtual GrupoTributarioVM GrupoTributario { get; set; }

        #endregion
    }
}