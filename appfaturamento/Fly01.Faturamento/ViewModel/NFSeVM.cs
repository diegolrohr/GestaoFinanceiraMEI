using Newtonsoft.Json;
using System;

namespace Fly01.Faturamento.ViewModel
{
    [Serializable]
    public class NFSeVM : NotaFiscalVM
    {
        [JsonProperty("totalImpostosServicos")]
        public double TotalImpostosServicos { get; set; }

        [JsonProperty("xmlUnicoTSS")]
        public string XMLUnicoTSS { get; set; }
    }
}