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

        public bool ShouldSerializeEspecie()
        {
            return (!string.IsNullOrEmpty(Especie) && Especie.Replace(" ", "").Length > 0);
        }

        [XmlElement("marca")]
        public string Marca { get; set; }

        public bool ShouldSerializeMarca()
        {
            return (!string.IsNullOrEmpty(Marca) && Marca.Replace(" ", "").Length > 0);
        }

        [XmlElement("nVol")]
        public string Numeracao { get; set; }

        public bool ShouldSerializeNumeracao()
        {
            return (!string.IsNullOrEmpty(Numeracao) && Numeracao.Replace(" ", "").Length > 0);
        }

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
