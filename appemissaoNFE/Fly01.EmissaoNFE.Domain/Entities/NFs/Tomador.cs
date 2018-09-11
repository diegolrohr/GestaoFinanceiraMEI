using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFs
{
    public class Tomador
    {
        [XmlElement(ElementName = "inscmun")]
        public string InscricaoMunicipal { get; set; }

        [XmlElement(ElementName = "cpfcnpj")]
        public string CpfCnpj { get; set; }

        [XmlElement(ElementName = "razao")]
        public string RazaoSocial { get; set; }

        [XmlElement(ElementName = "tipologr")]
        public TipoLogradouroNFs TipoLogradouro { get; set; }

        [XmlElement(ElementName = "logradouro")]
        public string Logradouro { get; set; }

        [XmlElement(ElementName = "numend")]
        public string NumeroEndereco { get; set; }

        [XmlElement(ElementName = "tipobairro")]
        public TipoBairroNFs TipoBairro { get; set; }

        [XmlElement(ElementName = "bairro")]
        public string Bairro { get; set; }

        [XmlElement(ElementName = "codmunibge")]
        public int CodigoMunicipioIBGE { get; set; }

        [XmlElement(ElementName = "cidade")]
        public string Cidade { get; set; }

        [XmlElement(ElementName = "uf")]
        public string UF { get; set; }

        [XmlElement(ElementName = "cep")]
        public string CEP { get; set; }

        [XmlElement(ElementName = "email")]
        public string Email { get; set; }

        [XmlElement(ElementName = "ddd")]
        public string DDD { get; set; }

        [XmlElement(ElementName = "telefone")]
        public string Telefone { get; set; }

        [XmlElement(ElementName = "codpais")]
        public int CodigoPais { get; set; }

        [XmlElement(ElementName = "nomepais")]
        public string NomePais { get; set; }

        [XmlElement(ElementName = "estrangeiro")]
        public EstrangeiroNFs EstrangeiroNFs { get; set; }

        [XmlElement(ElementName = "notificatomador")]
        public NotificaTomadorNFs NotificaTomado { get; set; }

        [XmlElement(ElementName = "inscest")]
        public string InscricapCest { get; set; }

        [XmlElement(ElementName = "situacaoespecial")]
        public string SituacaoEspecial { get; set; }
        

    }
}
