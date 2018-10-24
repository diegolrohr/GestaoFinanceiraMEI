using Fly01.EmissaoNFE.Domain.Enums;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    [XmlRoot(ElementName = "pagamentos")]
    public class Pagamentos
    {
        [XmlElement(ElementName = "condpag")]
        public TipoCondicaoPagamento CondicaoPagamento
        {
            get
            {
               return (ListaPagamentos != null && ListaPagamentos.Count > 1) ?
                TipoCondicaoPagamento.APrazo :
                TipoCondicaoPagamento.AVista;
            }
            set { }
        }

        [XmlElement(ElementName = "pagamento")]
        public List<Pagamento> ListaPagamentos { get; set; }
    }
}
