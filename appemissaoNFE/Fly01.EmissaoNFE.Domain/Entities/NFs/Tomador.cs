using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    public class Tomador
    {
        [XmlElement(ElementName = "inscmun")]
        public string InscricaoMunicipal { get; set; }

        [XmlElement(ElementName = "cpfcnpj")]
        public string CpfCnpj { get; set; }

        [XmlElement(ElementName = "razao")]
        public string RazaoSocial { get; set; }

        /// <summary>
        /// O tipo é fixo conforme código FIRST
        /// 1 – Avenida; 2 – Rua; 3 – Rodovia; 4 – Ruela; 5 – Rio; 6 – Sítio; 7 – Sup Quadra; 8 – Travessa; 9 – Vale;
        /// 10 – Via; 11 – Viaduto; 12 – Viela; 13 – Vila 14 – Vargem 15– Alameda 16 – Praça 17 – Beco 18 – Travessa 19 – Via Elevada 20 – Parque
        /// 21 – largo 22 – Viela Particular 23 – Pátio 24 – Viela Sanitária 25 – Ladeira 26 – Jardim 27 – Estrada 28 – Ponte 29 – Rua Particular 30 – Praia
        /// </summary>
        [XmlElement(ElementName = "tipologr")]
        private string TipoLogradouro
        {
            get
            {
                return "2";
            }
            set { }
        }

        [XmlElement(ElementName = "logradouro")]
        public string Logradouro { get; set; }

        [XmlElement(ElementName = "numend")]
        public string NumeroEndereco { get; set; }

        [XmlElement(ElementName = "complend")]
        public string ComplementoEndereco { get; set; }

        public bool ShouldSerializeComplementoEndereco()
        {
            return !string.IsNullOrEmpty(ComplementoEndereco);
        }

        /// <summary>
        /// O tipo é fixo conforme código FIRST
        /// </summary>
        [XmlElement(ElementName = "tipobairro")]
        private string TipoBairro
        {
            get
            {
                return "1";
            }
            set { }
        }

        [XmlElement(ElementName = "bairro")]
        public string Bairro { get; set; }

        [XmlElement(ElementName = "codmunibge")]
        public string CodigoMunicipioIBGE { get; set; }

        [XmlElement(ElementName = "codmunsiafi")]
        public string CodigoMunicipioSIAFI { get; set; }

        public bool ShouldSerializeCodigoMunicipioSIAFI()
        {
            return !string.IsNullOrEmpty(CodigoMunicipioSIAFI);
        }

        [XmlElement(ElementName = "cidade")]
        public string Cidade { get; set; }

        [XmlElement(ElementName = "uf")]
        public string UF { get; set; }

        [XmlElement(ElementName = "cep")]
        public string CEP { get; set; }

        [XmlElement(ElementName = "email")]
        public string Email { get; set; }

        public bool ShouldSerializeEmail()
        {
            return !string.IsNullOrEmpty(Email);
        }

        [XmlElement(ElementName = "ddd")]
        public string DDD { get; set; }

        [XmlElement(ElementName = "telefone")]
        public string Telefone { get; set; }

        public bool ShouldSerializeTelefone()
        {
            return !string.IsNullOrEmpty(Telefone);
        }

        /// <summary>
        /// Fixo Brasil, se precisar outros paises importar tabela ibge e realizar demais validações
        /// </summary>
        [XmlElement(ElementName = "codpais")]
        public string CodigoPais
        {
            get
            {
                return "1058";
            }
            set { }
        }

        [XmlElement(ElementName = "nomepais")]
        public string NomePais
        {
            get
            {
                return "BRASIL";
            }
            set { }
        }

        [XmlElement(ElementName = "estrangeiro")]
        public TipoSimNao EstrangeiroNFs
        {
            get
            {
                return TipoSimNao.Nao;
            }
            set { }
        }

        [XmlElement(ElementName = "notificatomador")]
        public TipoSimNao NotificaTomado
        {
            get
            {
                return string.IsNullOrEmpty(Email) ? TipoSimNao.Nao : TipoSimNao.Sim;
            }
            set { }
        }

        [XmlElement(ElementName = "inscest")]
        public string InscricaoEstadual { get; set; }

        [XmlElement(ElementName = "situacaoespecial")]
        public TipoSituacaoEspecialNFS SituacaoEspecial { get; set; }

        [XmlIgnore]
        public bool ConsumidorFinal { get; set; }
    }
}
