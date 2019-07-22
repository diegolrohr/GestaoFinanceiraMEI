using System.Collections.Generic;
using Fly01.Financeiro.ViewModel;
using Newtonsoft.Json;
using Fly01.Core.ViewModels.Presentation.Commons;

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