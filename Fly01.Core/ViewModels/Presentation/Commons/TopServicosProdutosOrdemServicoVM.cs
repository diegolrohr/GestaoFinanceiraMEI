using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class TopServicosProdutosOrdemServicoVM
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }
    }
}
