using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation
{
    public class ArquivoVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }
        [JsonProperty("conteudo")]
        public string Conteudo { get; set; }
        [JsonProperty("md5")]
        public string MD5 { get; set; }
        [JsonProperty("cadastro")]
        public string Cadastro { get; set; }
        [JsonProperty("totalProcessado")]
        public string TotalProcessado { get; set; }
        [JsonProperty("retorno")]
        public string Retorno { get; set; }
        [JsonProperty("plataformaId")]
        public string PlataformaId { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("dataInclusao")]
        public string DataInclusao { get; set; }
        [JsonProperty("dataAlteracao")]
        public string DataAlteracao { get; set; }
        [JsonProperty("dataExclusao")]
        public string DataExclusao { get; set; }
        [JsonProperty("usuarioInclusao")]
        public string UsuarioInclusao { get; set; }
        [JsonProperty("usuarioAlteracao")]
        public string UsuarioAlteracao { get; set; }
        [JsonProperty("usuarioExclusao")]
        public string UsuarioExclusao { get; set; }
        [JsonProperty("ativo")]
        public string Ativo { get; set; }
    }
}