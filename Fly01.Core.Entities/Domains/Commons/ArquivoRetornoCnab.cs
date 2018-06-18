using Fly01.Core.Entities.Domains.Enum;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
