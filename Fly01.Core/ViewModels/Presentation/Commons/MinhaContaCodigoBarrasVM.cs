using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class MinhaContaCodigoBarrasVM : DomainBaseVM
    {
        [JsonProperty("barcode")]
        public string CodigoBarras { get; set; }

        [JsonProperty("formattedBarcode")]
        public string CodigoBarrasFormatado { get; set; }
    }
}

