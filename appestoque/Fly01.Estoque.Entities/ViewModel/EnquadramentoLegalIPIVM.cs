using Fly01.Core.VM;
using Newtonsoft.Json;
using System;

namespace Fly01.Estoque.Entities.ViewModel
{
    [Serializable]
    public class EnquadramentoLegalIPIVM : DomainBaseVM
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("grupoCST")]
        public string GrupoCST { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }
}
