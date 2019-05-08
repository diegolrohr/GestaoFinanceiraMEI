﻿using System;

namespace Fly01.Compras.Models.ViewModel
{
    public class ImprimirPedidoVM
    {
        public string Categoria { get; internal set; }
        public string CondicaoParcelamento { get; internal set; }
        public DateTime Data { get; internal set; }
        public DateTime? DataVencimento { get; internal set; }
        public string EstadoPlacaVeiculo { get; internal set; }
        public string Finalidade { get; internal set; }
        public string FormaPagamento { get; internal set; }
        public string Fornecedor { get; internal set; }

        public string EnderecoFornecedor { get; internal set; }
        public string NumeroEndereco { get; internal set; }
        public string Bairro { get; internal set; }
        public string Cidade { get; internal set; }
        public string CEP { get; internal set; }

        public string Id { get; set; }
        public string Marca { get; internal set; }
        public string NomeProduto { get; internal set; }
        public string NumeracaoVolumesTrans { get; internal set; }
        public int Numero { get; internal set; }
        public string Observacao { get; internal set; }
        public double? PesoBruto { get; internal set; }
        public double? PesoLiquido { get; internal set; }
        public string PlacaVeiculo { get; internal set; }
        public double QtdProduto { get; internal set; }
        public int? QuantVolumes { get; internal set; }
        public string Status { get; internal set; }
        public string TipoEspecie { get; internal set; }
        public string TipoFrete { get; internal set; }
        public double? TotalGeral { get; internal set; }
        public double? TotalImpostosProdutos { get; internal set; }
        public double? TotalProdutos { get; internal set; }
        public string Transportadora { get; internal set; }
        public double? ValorFrete { get; internal set; }
        public double? ValorTotal { get; internal set; }
        public double? ValorUnitario { get; internal set; }
    }
}