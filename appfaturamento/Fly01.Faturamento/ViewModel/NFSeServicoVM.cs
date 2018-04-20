using System;
using Newtonsoft.Json;

namespace Fly01.Faturamento.ViewModel
{
    [Serializable]
    public class NFSeServicoVM : NotaFiscalItemVM
    {
        [JsonProperty("servicoId")]
        public Guid ServicoId { get; set; }

        [JsonProperty("servico")]
        public virtual ServicoVM Servico { get; set; }
    }
}