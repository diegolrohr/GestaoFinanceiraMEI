using System;
using Fly01.Compras.Entities.ViewModel.Base;
using Newtonsoft.Json;

namespace Fly01.Compras.Entities.ViewModel
{
    [Serializable]
    public class GlobalParameterVM : DomainBaseVM
    {
        [JsonProperty("stockControl")]
        public string StockControl { get; set; }
    }
}
