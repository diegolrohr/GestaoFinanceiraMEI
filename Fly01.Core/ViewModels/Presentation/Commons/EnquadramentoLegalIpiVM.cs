﻿using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class EnquadramentoLegalIpiVM : DomainBaseVM
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("grupoCST")]
        public string GrupoCST { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }
}