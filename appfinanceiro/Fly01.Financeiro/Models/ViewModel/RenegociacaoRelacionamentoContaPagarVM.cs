using System.Collections.Generic;
using Fly01.Financeiro.Entities.ViewModel;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.ViewModel
{
    public class RenegociacaoRelacionamentoContaPagarVM
    {
        [JsonProperty("renegociacao")]
        public ContaFinanceiraRenegociacaoVM Renegociacao { get; set; }

        [JsonProperty("value")]
        public List<ContaPagarVM> Data { get; set; }
    }
}