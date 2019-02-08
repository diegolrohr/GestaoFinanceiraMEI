using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    /// <summary>
    /// Does not support untyped value in non-open type
    /// </summary>
    public class NFeImportacaoFormVM : NFeImportacaoVM
    {
        [JsonProperty("xml")]
        public string XML{ get; set; }

        #region Informações XML
        [JsonProperty("fornecedorNomeXml")]
        public string FornecedorNomeXml { get; set; }

        [JsonProperty("fornecedorCnpjXml")]
        public string FornecedorCnpjXml { get; set; }

        [JsonProperty("fornecedorInscEstadualXml")]
        public string FornecedorInscEstadualXml { get; set; }

        [JsonProperty("fornecedorRazaoSocialXml")]
        public string FornecedorRazaoSocialXml { get; set; }

        [JsonProperty("transportadoraXml")]
        public string TransportadoraXml { get; set; }

        [JsonProperty("transportadorCNPJXml")]
        public string TransportadorCNPJXml { get; set; }

        [JsonProperty("transportadoraRazaoSocialXml")]
        public string TransportadoraRazaoSocialXml { get; set; }

        [JsonProperty("transportadoraInscEstadualXml")]
        public string TransportadoraInscEstadualXml { get; set; }

        [JsonProperty("transportadoraUFXml")]
        public string TransportadoraUFXml { get; set; }
        #endregion

    }
}