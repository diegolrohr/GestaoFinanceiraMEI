using Fly01.Core.Entities.Domains.Enum;
using System;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class MonitorRetornoVM
    {
        public string NotaId { get; set; }
        public StatusNotaFiscal Status { get; set; }
        public string Modalidade { get; set; }
        public string Mensagem { get; set; }
        public string Recomendacao { get; set; }
        public DateTime Data { get; set; }
        public string Hora { get; set; }
    }
}
