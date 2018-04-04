using Fly01.EmissaoNFE.Domain.Entities.NFe;
using System.Collections.Generic;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class ItemTransmissaoRetornoVM
    {
        /// <summary>
        /// ID da nota
        /// </summary>
        public string NotaId { get; set; }
        public List<SchemaXMLRetornoVM> Error { get; set; }
    }
}
