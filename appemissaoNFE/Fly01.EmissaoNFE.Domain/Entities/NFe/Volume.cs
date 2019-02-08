using Newtonsoft.Json;
using System.Xml.Serialization;
using Fly01.Core;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot("vol")]
    public class Volume
    {
        [JsonProperty("qVol")]
        [XmlElement("qVol")]
        public int Quantidade { get; set; }

        [JsonProperty("esp")]
        [XmlElement("esp")]
        public string Especie { get; set; }

        public bool ShouldSerializeEspecie()
        {
            return (!string.IsNullOrEmpty(Especie) && Especie.Replace(" ", "").Length > 0);
        }

        [JsonProperty("marca")]
        [XmlElement("marca")]
        public string Marca { get; set; }

        public bool ShouldSerializeMarca()
        {
            return (!string.IsNullOrEmpty(Marca) && Marca.Replace(" ", "").Length > 0);
        }

        [JsonProperty("nVol")]
        [XmlElement("nVol")]
        public string Numeracao { get; set; }

        public bool ShouldSerializeNumeracao()
        {
            return (!string.IsNullOrEmpty(Numeracao) && Numeracao.Replace(" ", "").Length > 0);
        }

        [XmlIgnore]
        public double PesoLiquido { get; set; }

        [JsonProperty("pesoL")]
        [XmlElement("pesoL")]
        public string PesoLiquidoString
        {
            get { return PesoLiquido.ToString("0.000").Replace(",", "."); }
            set { PesoLiquido = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double PesoBruto { get; set; }

        [JsonProperty("pesoB")]
        [XmlElement("pesoB")]
        public string PesoBrutoString
        {
            get { return PesoBruto.ToString("0.000").Replace(",", "."); }
            set { PesoBruto = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
    }
}
