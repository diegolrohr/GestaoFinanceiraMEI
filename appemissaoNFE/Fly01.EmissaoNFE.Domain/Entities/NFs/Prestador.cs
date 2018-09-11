using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFs
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
        public int CodigoMunicipalIBGE { get; set; }

        [XmlElement(ElementName = "cidade")]
        public string Cidade { get; set; }

        [XmlElement(ElementName = "uf")]
        public string UF { get; set; }

        [XmlElement(ElementName = "ddd")]
        public string DDD { get; set; }

        [XmlElement(ElementName = "telefone")]
        public string Telefone { get; set; }

        [XmlElement(ElementName = "simpnac")]
        public TipoSimplesNacionalNFs TipoSimplesNacional { get; set; }

        [XmlElement(ElementName = "incentcult")]
        public TipoIcentivoCulturalNFs TipoIcentivoCultural { get; set; }

        [XmlElement(ElementName = "numproc")]
        public int NumeroProcedimento { get; set; }

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
