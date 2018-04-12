using Fly01.Core.Attribute;
using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class FormaPagamentoVM : DomainBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("tipoFormaPagamento")]
        [APIEnum("TipoFormaPagamento")]
        public string TipoFormaPagamento { get; set; }

    }
}
