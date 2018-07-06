using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    [XmlRoot("envEvento")]
    public class EnvelopeEvento
    {
        /// <summary>
        /// Eventos do envelope
        /// </summary>
        public Eventos Eventos { get; set; }

    }
}
