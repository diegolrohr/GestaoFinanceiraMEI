using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    public class Prestador
    {
        [XmlElement(ElementName = "inscmun")]
        public int InscricaoMunicipal { get; set; }

        [XmlElement(ElementName = "cpfcnpj")]
        public string CpfCnpj { get; set; }

        [XmlElement(ElementName = "razao")]
        public string RazaoSocial { get; set; }

        [XmlElement(ElementName = "fantasia")]
        public string NomeFantasia { get; set; }

        [XmlElement(ElementName = "codmunibge")]
        public string CodigoMunicipioIBGE { get; set; }

        [XmlElement(ElementName = "cidade")]
        public string Cidade { get; set; }

        [XmlElement(ElementName = "uf")]
        public string UF { get; set; }

        [XmlElement(ElementName = "ddd")]
        public string DDD { get; set; }

        [XmlElement(ElementName = "telefone")]
        public string Telefone { get; set; }

        /// <summary>
        /// O tipo é fixo sim, pois só atendemos ao regime Simples Nacional
        /// </summary>
        [XmlElement(ElementName = "simpnac")]
        public TipoSimNao EhSimplesNacional
        {
            get
            {
                return TipoSimNao.Sim;
            }
        }

        [XmlElement(ElementName = "incentcult")]
        public TipoSimNao TipoIcentivoCultural { get; set; }

        /// <summary>
        /// Enviar vazio para TSS
        /// </summary>
        [XmlElement(ElementName = "numproc")]
        public string NumeroProcedimento
        {
            get
            {
                return string.Empty;
            }
        }

        [XmlElement(ElementName = "logradouro")]
        public string Logradouro { get; set; }
        
        [XmlElement (ElementName = "numend")]
        public string NumeroEndereco { get; set; }

        [XmlElement(ElementName = "bairro")]
        public string Bairro { get; set; }

        [XmlElement(ElementName ="cep")]
        public string CEP { get; set; }

    }
}
