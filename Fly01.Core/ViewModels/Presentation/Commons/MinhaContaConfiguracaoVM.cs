using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class MinhaContaConfiguracaoVM
    {
        [JsonProperty("codigoMaxime")]
        public string CodigoMaxime { get; set; }

        [JsonProperty("vencimentoInicial")]
        public string VencimentoInicial { get; set; }

        [JsonProperty("vencimentoFinal")]
        public string VencimentoFinal { get; set; }

        [JsonProperty("posicao")]
        public string Posicao { get; set; }

    }
}

