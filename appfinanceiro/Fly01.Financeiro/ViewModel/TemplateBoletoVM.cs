﻿using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.ViewModel
{
    public class TemplateBoletoVM : DomainBaseVM
    {
        [JsonProperty("assunto")]
        public string Assunto { get; set; }

        [JsonProperty("mensagem")]
        public string Mensagem { get; set; }
    }
}