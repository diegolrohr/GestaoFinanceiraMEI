using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.Utils
{
    public class ReponseBaixaMultiplaVM
    {
        [JsonProperty("details")]
        public List<ReponseBaixaMultiplaVM> details { get; set; }

        [JsonProperty("accountPayableId")]
        public string accountPayableId { get; set; }

        [JsonProperty("accountReceivableId")]
        public string accountReceivableId { get; set; }

        [JsonProperty("msg")]
        public string msg { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("result")]
        public string result { get; set; }
    }
}