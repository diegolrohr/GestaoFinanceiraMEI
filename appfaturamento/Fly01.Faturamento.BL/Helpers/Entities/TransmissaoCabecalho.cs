using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels;
using System.Linq;

namespace Fly01.Faturamento.BL.Helpers.Entities
{
    public class TransmissaoCabecalho
    {
        public bool IsLocal { get; set; }
        public string Versao { get; set; }
        public Pessoa Cliente { get; set; }
        public ManagerEmpresaVM Empresa { get; set; }
        public CondicaoParcelamento CondicaoParcelamento { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public Pessoa Transportadora { get; set; }
        public SerieNotaFiscal SerieNotaFiscal { get; set; }
        public Estado UFSaidaPais { get; set; }
        public IQueryable<NFeProduto> NFeProdutos { get; set; }
    }
}
