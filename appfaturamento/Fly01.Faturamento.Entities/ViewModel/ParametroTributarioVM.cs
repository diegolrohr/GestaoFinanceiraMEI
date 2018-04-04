using System;
using Newtonsoft.Json;
using Fly01.Core.VM;
using Fly01.Core.Api;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class ParametroTributarioVM : DomainBaseVM
    {
        [JsonProperty("simplesNacional")]
        public bool SimplesNacional { get; set; }

        [JsonProperty("aliquotaSimplesNacional")]
        public double AliquotaSimplesNacional { get; set; }

        [JsonProperty("aliquotaISS")]
        public double AliquotaISS { get; set; }

        [JsonProperty("aliquotaPISPASEP")]
        public double AliquotaPISPASEP { get; set; }

        [JsonProperty("aliquotaCOFINS")]
        public double AliquotaCOFINS { get; set; }

        [JsonProperty("registroSimplificadoMT")]
        public bool RegistroSimplificadoMT { get; set; }

        [JsonProperty("mensagemPadraoNota")]
        public string MensagemPadraoNota { get; set; }

        [JsonProperty("numeroRetornoNF")]
        public string NumeroRetornoNF { get; set; }

        [JsonProperty("tipoAmbiente")]
        [APIEnum("TipoAmbiente")]
        public string TipoAmbiente { get; set; }

        [JsonProperty("tipoVersaoNFe")]
        [APIEnum("TipoVersaoNFe")]
        public string TipoVersaoNFe { get; set; }

        [JsonProperty("tipoModalidade")]
        [APIEnum("TipoModalidade")]
        public string TipoModalidade { get; set; }

        [JsonProperty("aliquotaFCP")]
        public double AliquotaFCP { get; set; }
        
        #region NFS
        //[JsonProperty("incentivoCultura")]
        //public bool IncentivoCultura { get; set; }

        //[JsonProperty("tipoRegimeEspecialTrib")]
        //[APIEnum("TipoRegimeEspecialTrib")]
        //public string TipoRegimeEspecialTrib { get; set; }

        //[JsonProperty("tipoMensagemNFSE")]
        //[APIEnum("TipoMensagemNFSE")]
        //public string TipoMensagemNFSE { get; set; }

        //[JsonProperty("tipoLayoutNFSE")]
        //[APIEnum("TipoLayoutNFSE")]
        //public string TipoLayoutNFSE { get; set; }

        //[JsonProperty("novoModeloUnicoXMLTSS")]
        //public bool NovoModeloUnicoXMLTSS { get; set; }

        //[JsonProperty("siafi")]
        //public string SIAFI { get; set; }

        //[JsonProperty("tipoAmbienteNFS")]
        //[APIEnum("TipoAmbienteNFS")]
        //public string TipoAmbienteNFS { get; set; }

        //[JsonProperty("versao")]
        //public string Versao { get; set; }

        //[JsonProperty("usuario")]
        //public string Usuario { get; set; }

        //[JsonProperty("senha")]
        //public string Senha { get; set; }

        //[JsonProperty("chaveAutenticacao")]
        //public string ChaveAutenticacao { get; set; }
        #endregion
    }
}