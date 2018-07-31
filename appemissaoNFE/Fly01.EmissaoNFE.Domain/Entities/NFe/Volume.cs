using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot("vol")]
    public class Volume
    {
        [XmlElement("qVol")]
        public int Quantidade { get; set; }

        [XmlElement("esp")]
        public string Especie { get; set; }

        [XmlElement("marca")]
        public string Marca { get; set; }

        [XmlElement("nVol")]
        public string Numeracao { get; set; }

        [XmlIgnore]
        public double PesoLiquido { get; set; }

        [XmlElement("pesoL")]
        public string PesoLiquidoString
        {
            get { return PesoLiquido.ToString("0.000").Replace(",", "."); }
            set { PesoLiquido = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double PesoBruto { get; set; }

        [XmlElement("pesoB")]
        public string PesoBrutoString
        {
            get { return PesoBruto.ToString("0.000").Replace(",", "."); }
            set { PesoBruto = double.Parse(value.Replace(".", ",")); }
        }
    }
}
