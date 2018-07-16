using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    [XmlRoot("envEvento")]
    public class EnvelopeEvento
    {
        /// <summary>
        /// Eventos do envelope
        /// </summary>
        [XmlElement("eventos")]
        public Eventos Eventos { get; set; }

    }
}
