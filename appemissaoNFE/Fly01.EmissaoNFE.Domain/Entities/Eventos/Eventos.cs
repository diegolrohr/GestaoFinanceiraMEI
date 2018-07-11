using System.Collections.Generic;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    [XmlRoot("eventos")]
    public class Eventos
    {
        /// <summary>
        /// Detalhe dos eventos
        /// </summary>
        //public List<DetalheEvento> DetalhesEventos { get; set; }//herança estava criando tags erradas
        [XmlElement("detEvento")]
        public List<CartaCorrecaoEvento> DetalhesEventos { get; set; }

    }
}
