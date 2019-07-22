using System;

namespace Fly01.Compras.Models.ViewModel
{
    public class ImprimirOrcamentoVM
    {
        public string Categoria { get; internal set; }
        public string CondicaoParcelamento { get; internal set; }
        public DateTime? DataVencimento { get; internal set; }
        public string FormaPagamento { get; internal set; }
        public string Fornecedor { get; internal set; }
        public string Id { get; set; }
        public string NomeProduto { get; internal set; }
        public int Numero { get; internal set; }
        public string Observacao { get; internal set; }
        public double QtdProduto { get; internal set; }
        public double ValorTotal { get; internal set; }
        public double ValorUnitario { get; internal set; }
        public string ParcelaConta { get; internal set; }
    }
}