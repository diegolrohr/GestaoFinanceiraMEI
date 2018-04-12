using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly01.Faturamento.Entities.ViewModel
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
