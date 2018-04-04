using Newtonsoft.Json;
using System;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class NFSeVM : NotaFiscalVM
    {
        [JsonProperty("totalImpostosServicos")]
        public double TotalImpostosServicos { get; set; }
    }
}