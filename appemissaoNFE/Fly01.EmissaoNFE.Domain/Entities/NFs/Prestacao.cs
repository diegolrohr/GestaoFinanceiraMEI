using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    public class Prestacao
    {
        /// <summary>
        /// O tipo é fixo conforme códigos First
        /// Série da prestação dos serviços do documento.
        /// </summary>
        [XmlElement(ElementName = "serieprest")]
        public string SeriePrestacao
        {
            get
            {
                return "99";
            }
            set {}
        }

        [XmlElement(ElementName = "logradouro")]
        public string Logradouro { get; set; }

        /// <summary>
        /// Informe o Número ou SN
        /// </summary>
        [XmlElement(ElementName = "numend")]
        public string NumeroEndereco { get; set; }

        /// <summary>
        /// Complemento do endereço da prestação dos serviços do documento. Length 30
        /// </summary>
        //[XmlElement(ElementName = "complend")]
        [XmlIgnore]
        public string ComplementoEndereco { get; set; }

        /// <summary>
        /// Código do município do prestador padrão IBGE.
        /// </summary>
        [XmlElement(ElementName = "codmunibge")]
        public string CodigoMunicipioIBGE { get; set; }

        /// <summary>
        /// Código Municipio Incidência, mesmo código Municipio IBGE
        /// Código do município de Incidência padrão IBGE.
        /// </summary>
        [XmlElement(ElementName = "codmunibgeinc")]
        public string CodigoMunicipioIBGEIncidencia
        {
            get
            {
                return CodigoMunicipioIBGE;
            }
            set { }
        }

        [XmlElement(ElementName = "codmunsiafi")]        
        public string CodigoMunicipioSIAFI { get; set; }

        public bool ShouldSerializeCodigoMunicipioSIAFI()
        {
            return !string.IsNullOrEmpty(CodigoMunicipioSIAFI);
        }

        [XmlElement(ElementName = "municipio")]
        public string Municipio { get; set; }

        [XmlElement(ElementName = "bairro")]
        public string Bairro { get; set; }

        [XmlElement(ElementName = "uf")]
        public string UF { get; set; }

        [XmlElement(ElementName = "cep")]
        public string CEP { get; set; }
    }
}
