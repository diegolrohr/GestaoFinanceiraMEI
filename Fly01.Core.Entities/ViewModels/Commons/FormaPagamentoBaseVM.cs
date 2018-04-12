using Fly01.Core.Attribute;
using Newtonsoft.Json;

namespace Fly01.Core.Entities.ViewModels.Commons
{
    public abstract class FormaPagamentoBaseVM : DomainBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("tipoFormaPagamento")]
        [APIEnum("TipoFormaPagamento")]
        public string TipoFormaPagamento { get; set; }

    }
}
