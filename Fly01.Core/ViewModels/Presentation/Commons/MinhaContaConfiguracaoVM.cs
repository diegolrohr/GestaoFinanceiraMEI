﻿using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class MinhaContaConfiguracaoVM
    {
        [JsonProperty("codigoMaxime")]
        public string CodigoMaxime { get; set; }

        [JsonProperty("vencimentoInicial")]
        public DateTime VencimentoInicial { get; set; }

        [JsonProperty("vencimentoFinal")]
        public DateTime VencimentoFinal { get; set; }

        [JsonProperty("posicao")]
        public string Posicao { get; set; }

    }
}

