using System.Collections.Generic;

namespace Fly01.Core.ViewModels.Api

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
