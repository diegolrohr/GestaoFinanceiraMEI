using Newtonsoft.Json;
using System;

namespace Fly01.OrdemServico.ViewModel
{
    public class OrdemServicoItemServicoVM : OrdemServicoItemVM
    {
        [JsonProperty("servicoId")]
        public Guid ServicoId { get; set; }

        #region Navigation
        [JsonProperty("servico")]
        public virtual ServicoVM Servico { get; set; }
        #endregion
    }
}
