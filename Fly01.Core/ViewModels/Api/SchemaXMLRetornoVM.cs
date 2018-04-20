using System.Collections.Generic;

namespace Fly01.Core.ViewModels.Api

{
    public class SchemaXMLRetornoVM
    {
        public string NotaId { get; set; }
        public string Mensagem { get; set; }
        public string XML { get; set; }
        public List<SchemaXMLMensagemVM> SchemaMensagem { get; set; }
    }
}