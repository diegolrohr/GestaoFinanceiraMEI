using System;

namespace Fly01.Faturamento.Models.ViewModel
{
    public class ImprimirOrcamentoPedidoVM
    {
        public string Id { get; set; }
        public string CategoriaDescricao { get; set; }
        public string ClienteNome { get; set; }
        public string NumeroNota { get; set; }
        public string Data { get; set; }
        public string CondicaoParcelamentoDescricao { get; set; }
        public int? CondicaoParcelamentoQtdParcelas { get; set; }
        public string FormaPagamentoDescricao { get; set; }
        public double PesoBruto { get; set; }
        public double PesoLiquido { get; set; }
        public string Status { get; set; }
        public string TipoOrdemVenda { get; set; }
        public string TipoFrete { get; set; }
        public string PlacaVeiculo { get; set; }
        public string EstadoPlacaVeiculo { get; set; }
        public int QuantidadeVolumes { get; set; }
        public double ValorFrete { get; set; }
        public string TransportadoraNome { get; set; }
        public string NumeracaoVolumesTrans { get; set; }
        public string Marca { get; set; }
        public string TipoEspecie { get; set; }
        public string Observacao { get; set; }
        public Guid? ItemId { get; set; }
        public double ItemQtd { get; set; }
        public double ItemValor { get; set; }
        public double ItemDesconto { get; set; }
        public double ItemTotal { get; set; }
        public string ItemNome { get; set; }
        public double Total { get; set; }
        public double ValorFreteTotal { get; set; }
        public double TotalRetencoesServicos { get; set; }
        public double TotalServicos { get; set; }
        public double TotalImpostosProdutos { get; set; }
        public double TotalProdutos { get; set; }
        public string Finalidade { get; internal set; }
        public string EmiteNotaFiscal { get; internal set; }
        public string ExibirTransportadora { get; internal set; }
        public string ExibirProdutos { get; internal set; }
        public string ExibirServicos { get; internal set; }
        public string Endereco { get; internal set; }
        public string NumeroCliente { get; internal set; }
        public string Bairro { get; internal set; }
        public string Cidade { get; internal set; }
        public string Cep { get; internal set; }
        public string Complemento { get; internal set; }
        public string Estado { get; internal set; }
        public string Pais { get; internal set; }
        public string ParcelaConta { get; internal set; }
    }
}