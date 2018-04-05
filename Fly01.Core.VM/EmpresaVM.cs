using Newtonsoft.Json;
using System;

namespace Fly01.Core.VM
{
    public class EmpresaVM
    {
        [JsonProperty("cnpj")]
        public string CNPJ { get; set; }

        [JsonProperty("razaoSocial")]
        public string RazaoSocial { get; set; }

        [JsonProperty("cep")]
        public string CEP { get; set; }

        [JsonProperty("nomeFantasia")]
        public string NomeFantasia { get; set; }

        [JsonProperty("endereco")]
        public string Endereco { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        [JsonProperty("telefone")]
        public string Telefone { get; set; }

        [JsonProperty("fax")]
        public string Fax { get; set; }

        [JsonProperty("inscricaoEstadual")]
        public string InscricaoEstadual { get; set; }

        [JsonProperty("inscricaoMunicipal")]
        public string InscricaoMunicipal { get; set; }

        [JsonProperty("tipoInscricaoEstadual")]
        public int TipoInscricaoEstadual { get; set; }

        [JsonProperty("simplesNacional")]
        public bool SimplesNacional { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("nire")]
        public string Nire { get; set; }

        [JsonProperty("nireData")]
        public DateTime? NireData { get; set; }

        [JsonProperty("cnae")]
        public string CNAE { get; set; }

        [JsonProperty("cidadeId")]
        public int CidadeId { get; set; }

        [JsonProperty("estadoId")]
        public int EstadoId { get; set; }

        [JsonProperty("estadoNome")]
        public string EstadoNome { get; set; }

        [JsonProperty("platformUrlId")]
        public int PlatformUrlId { get; set; }

        #region Navigations Properties
        [JsonProperty("cidade")]
        public virtual CidadeVM Cidade { get; set; }
        #endregion
    }
}
