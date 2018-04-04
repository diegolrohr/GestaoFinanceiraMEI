using System.Xml.Serialization;

public enum TipoAliquota
{
    [XmlEnum(Name = "1")]
    AliquotaAdValorem = 1,

    [XmlEnum(Name = "2")]
    AliquotaEspecifica = 2
}
