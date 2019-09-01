using System;

namespace Fly01.Financeiro.Models.ViewModel
{
    [Serializable]
    public class ImprimirListContasFiltroVM
    {
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public string DataEmissaoInicial { get; set; }
        public string DataEmissaoFinal { get; set; }
        public string FornecedorFiltro { get; set; }
        public string FormaPagamentoFiltro { get; set; }
        public string CategoriaFiltro { get; set; }
        public string DescricaoFiltro { get; set; }
        public string CondicaoParcelamentoFiltro { get; set; }
    }
}