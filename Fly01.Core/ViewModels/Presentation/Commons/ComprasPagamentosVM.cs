using Fly01.Core.Entities.Domains.Enum;
using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ComprasPagamentosVM
    {
        public TipoFormaPagamento? TipoFormaPagamento { get; set; }
        public double Total { get; set; }
        public double Quantidade { get; set; }
    }
}
