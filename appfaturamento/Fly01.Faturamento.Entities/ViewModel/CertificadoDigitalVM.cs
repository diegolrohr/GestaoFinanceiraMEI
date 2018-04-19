using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class CertificadoDigitalVM : DomainBaseVM
    {
        [JsonProperty("tipo")]
        public int Tipo { get; set; }

        [JsonProperty("dataEmissao")]
        public DateTime DataEmissao { get; set; }

        [JsonProperty("dataExpiracao")]
        public DateTime DataExpiracao { get; set; }

        [JsonProperty("versao")]
        public string Versao { get; set; }

        [JsonProperty("certificado")]
        public string Certificado { get; set; }

        [JsonProperty("senha")]
        public string Senha { get; set; }

        [JsonProperty("entidadeHomologacao")]
        public string EntidadeHomologacao { get; set; }

        [JsonProperty("entidadeProducao")]
        public string EntidadeProducao { get; set; }

        [JsonProperty("emissor")]
        public string Emissor { get; set; }

        [JsonProperty("pessoa")]
        public string Pessoa { get; set; }

        [JsonProperty("md5")]
        public string Md5 { get; set; }
    }
}