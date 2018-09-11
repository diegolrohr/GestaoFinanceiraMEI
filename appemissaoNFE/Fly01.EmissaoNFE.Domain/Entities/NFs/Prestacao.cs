using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFs
{
    public class Prestacao
    {
        [XmlElement(ElementName = "serieprest")]
        public string SeriePrestacao { get; set; }

        [XmlElement(ElementName = "logradouro")]
        public string Logradouro { get; set; }

        [XmlElement(ElementName = "numend")]
        public string NumeroEndereco { get; set; }

        [XmlElement(ElementName = "codmunibge")]
        public int CodigoMunicipioIBGE { get; set; }

        [XmlElement(ElementName = "codmunibgeinc")]
        public int CodigoMunicipioIBGEInc { get; set; }

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
