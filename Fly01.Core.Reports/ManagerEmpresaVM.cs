using Newtonsoft.Json;

namespace Fly01.Core.Reports
{
    public class ManagerEstadoVM
    {
        [JsonProperty("sigla")]
        public string Sigla { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("utcId")]
        public string UtcId { get; set; }

        [JsonProperty("codigoIbge")]
        public string CodigoIbge { get; set; }
    }
    public class ManagerCidadeVM
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("codigoIbge")]
        public string CodigoIbge { get; set; }

        [JsonProperty("estadoId")]
        public int EstadoId { get; set; }

        #region Navigation Properties
        [JsonProperty("estado")]
        public virtual ManagerEstadoVM Estado { get; set; }
        #endregion
    }
    public class ManagerEmpresaVM
    {
        [JsonProperty("cnpj")]
        public string CNPJ { get; set; }

        [JsonProperty("razaoSocial")]
        public string RazaoSocial { get; set; }

        [JsonProperty("cep")]
        public string CEP { get; set; }

        [JsonProperty("endereco")]
        public string Endereco { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        [JsonProperty("telefone")]
        public string Telefone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        #region Navigations Properties
        [JsonProperty("cidade")]
        public virtual ManagerCidadeVM Cidade { get; set; }
        #endregion
    }
}
