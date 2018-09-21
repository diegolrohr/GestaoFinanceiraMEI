using Fly01.Core.Entities.Domains.Enum;
using System;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class MonitorNFSRetornoVM
    {
        public string NotaFiscalId { get; set; }
        public StatusNotaFiscal Status { get; set; }
        public string Modalidade { get; set; }
        public string Protocolo { get; set; }
        public string XML { get; set; }
        public string Recomendacao { get; set; }
        //LIST<ErroNFSVM> ErrosNFSVM
    }
}
