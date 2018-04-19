using System;
using Newtonsoft.Json;

namespace Fly01.Faturamento.ViewModel
{
    [Serializable]
    public class OrdemVendaServicoVM : OrdemVendaItemVM
    {
        [JsonProperty("servicoId")]
        public Guid ServicoId { get; set; }

        #region NavigationProperties

        [JsonProperty("servico")]
        public virtual ServicoVM Servico { get; set; }

        #endregion
    }
}