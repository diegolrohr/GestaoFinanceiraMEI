using System;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class TransferenciaVM
    {
        [JsonProperty("movimentacaoOrigem")]
        public MovimentacaoVM MovimentacaoOrigem { get; set; }

        [JsonProperty("movimentacaoDestino")]
        public MovimentacaoVM MovimentacaoDestino { get; set; }
    }
}
