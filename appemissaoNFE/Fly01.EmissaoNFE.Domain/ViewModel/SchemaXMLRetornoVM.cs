using System.Collections.Generic;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class SchemaXMLRetornoVM
    {
        public string NotaId { get; set; }
        public string Mensagem { get; set; }
        public string XML { get; set; }
        public List<SchemaXMLMensagemVM> SchemaMensagem { get; set; }
    }
}