using System;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class BankBalancesVM : DomainBaseVM
    {
        [JsonProperty("balance")]
        public double Balance { get; set; }
    }
}
