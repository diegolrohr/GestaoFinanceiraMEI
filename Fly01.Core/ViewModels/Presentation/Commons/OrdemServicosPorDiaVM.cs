using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ResponseOrdemServicosPorDiaVM
    {
        [JsonProperty("value")]
        public List<OrdemServicosPorDiaVM> Values { get; set; }
    }

    public class OrdemServicosPorDiaVM
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("quantidadeservicos")]
        public int QuantidadeServicos { get; set; }
    }
}