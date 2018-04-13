﻿using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class NBSVM : DomainBaseVM
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }
}