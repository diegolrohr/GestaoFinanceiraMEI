using Fly01.Core.Attribute;
using Fly01.Core.VM;
using Newtonsoft.Json;
using System;

namespace Fly01.Compras.Entities.ViewModel
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
