﻿using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Compras.ViewModel
{
    public class NFeImportacaoVM : DomainBaseVM
    {
        [JsonProperty("dataEmissao")]
        public DateTime DataEmissao { get; set; }

        [JsonProperty("serie")]
        public string Serie { get; set; }

        [JsonProperty("numero")]
        public int Numero { get; set; }

        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("atualizaDadosFornecedor")]
        public bool AtualizaDadosFornecedor { get; set; }

        [JsonProperty("novoFornecedor")]
        public bool NovoFornecedor { get; set; }

        [JsonProperty("fornecedorId")]
        public Guid? FornecedorId { get; set; }

        [JsonProperty("tipoFrete")]
        public string TipoFrete { get; set; }

        [JsonProperty("atualizaDadosTransportadora")]
        public bool AtualizaDadosTransportadora { get; set; }

        [JsonProperty("novaTransportadora")]
        public bool NovaTransportadora { get; set; }

        [JsonProperty("transportadoraId")]
        public Guid? TransportadoraId { get; set; }

        [JsonProperty("geraFinanceiro")]
        public bool GeraFinanceiro { get; set; }

        [JsonProperty("geraContasXml")]
        public bool GeraContasXml { get; set; }

        [JsonProperty("condicaoParcelamentoId")]
        public Guid? CondicaoParcelamentoId { get; set; }

        [JsonProperty("formaPagamentoId")]
        public Guid? FormaPagamentoId { get; set; }

        [JsonProperty("categoriaId")]
        public Guid? CategoriaId { get; set; }

        [JsonProperty("dataVencimento")]
        public DateTime DataVencimento { get; set; }

        [JsonProperty("valorTotal")]
        public double ValorTotal { get; set; }

        [JsonProperty("valorFrete")]
        public double ValorFrete { get; set; }

        [JsonProperty("somatorioICMSST")]
        public double SomatorioICMSST { get; set; }

        [JsonProperty("somatorioIPI")]
        public double SomatorioIPI { get; set; }

        [JsonProperty("somatorioFCPST")]
        public double SomatorioFCPST { get; set; }

        [JsonProperty("somatorioDesconto")]
        public double SomatorioDesconto { get; set; }

        [JsonProperty("contaFinanceiraPaiId")]
        public Guid? ContaFinanceiraPaiId { get; set; }

        [JsonProperty("novoPedido")]
        public bool NovoPedido { get; set; }

        [JsonProperty("pedidoId")]
        public Guid? PedidoId { get; set; }

        [JsonProperty("centroCustoId")]
        public Guid? CentroCustoId { get; set; }

        #region navigations
        [JsonProperty("pedido")]
        public virtual PedidoVM Pedido { get; set; }
        [JsonProperty("fornecedor")]
        public virtual PessoaVM Fornecedor { get; set; }
        [JsonProperty("transportadora")]
        public virtual PessoaVM Transportadora { get; set; }
        [JsonProperty("condicaoParcelamento")]
        public virtual CondicaoParcelamentoVM CondicaoParcelamento { get; set; }
        [JsonProperty("formaPagamento")]
        public virtual FormaPagamentoVM FormaPagamento { get; set; }
        [JsonProperty("categoria")]
        public virtual CategoriaVM Categoria { get; set; }
        [JsonProperty("centroCusto")]
        public virtual CentroCustoVM CentroCusto { get; set; }
        #endregion


    }
}