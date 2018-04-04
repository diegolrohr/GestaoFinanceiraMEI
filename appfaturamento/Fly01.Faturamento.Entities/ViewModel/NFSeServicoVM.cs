using System;
using Newtonsoft.Json;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class NFSeServicoVM : NotaFiscalItemVM
    {
        [JsonProperty("servicoId")]
        public Guid ServicoId { get; set; }

        #region NavigationProperties

        [JsonProperty("servico")]
        public virtual ServicoVM Servico { get; set; }

        #endregion
    }
}