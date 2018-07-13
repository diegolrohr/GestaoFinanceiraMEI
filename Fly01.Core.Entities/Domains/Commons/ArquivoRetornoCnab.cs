using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ArquivoRetornoCnab : DomainBase
    {
        [JsonProperty("valueArquivo")]
        public string ValueArquivo { get; set; }

        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }
    }
}
