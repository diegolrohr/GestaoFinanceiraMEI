using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class MonitorEventoRetornoVM
    {
        public string IdEvento { get; set; }
        public StatusCartaCorrecao Status { get; set; }
        public string Motivo { get; set; }
        public string MotivoEvento { get; set; }
        public string Protocolo { get; set; }
        public string XML { get; set; }
    }
}
