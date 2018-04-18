using System;

namespace Fly01.Compras.Models.ViewModel
{
    public class ImprimirPedidoVM
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
        public double? PesoBruto { get; internal set; }
        public double? PesoLiquido { get; internal set; }
        public double QtdProduto { get; internal set; }
        public int? QuantVolumes { get; internal set; }
        public string TipoFrete { get; internal set; }
        public double? TotalGeral { get; internal set; }
        public string Transportadora { get; internal set; }
        public double? ValorFrete { get; internal set; }
        public double ValorTotal { get; internal set; }
        public double ValorUnitario { get; internal set; }
    }
}