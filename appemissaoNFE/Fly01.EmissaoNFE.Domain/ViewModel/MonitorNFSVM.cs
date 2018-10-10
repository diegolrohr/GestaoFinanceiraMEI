using Fly01.Core.Entities.Domains.Enum;
using System;


namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class MonitorNFSVM : EntidadeVM
    {
        public string NotaInicial { get; set; }
        public string NotaFinal { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
    }
}
