using Fly01.Core.Entities.Domains.Commons;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class NFeImportacaoVM : DomainBaseVM
    {
        [JsonProperty("xml")]
        public string XML{ get; set; }

        [JsonProperty("xmlMd5")]
        public string XmlMd5 { get; set; }

        [JsonProperty("dataEmissao")]
        public DateTime DataEmissao { get; set; }

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

        #region Informações XML
        [JsonProperty("fornecedorNomeXml")]
        public string FornecedorNomeXml { get; set; }

        [JsonProperty("fornecedorCnpjXml")]
        public string FornecedorCnpjXml { get; set; }

        [JsonProperty("fornecedorInscEstadualXml")]
        public string FornecedorInscEstadualXml { get; set; }

        [JsonProperty("fornecedorRazaoSocialXml")]
        public string FornecedorRazaoSocialXml { get; set; }

        [JsonProperty("transportadoraXml")]
        public string TransportadoraXml { get; set; }

        [JsonProperty("transportadorCNPJXml")]
        public string TransportadorCNPJXml { get; set; }

        [JsonProperty("transportadoraRazaoSocialXml")]
        public string TransportadoraRazaoSocialXml { get; set; }

        [JsonProperty("transportadoraInscEstadualXml")]
        public string TransportadoraInscEstadualXml { get; set; }

        [JsonProperty("transportadoraUFXml")]
        public string TransportadoraUFXml { get; set; }
        #endregion

        #region navigations
        public virtual Pedido Pedido { get; set; }
        public virtual Pessoa Fornecedor { get; set; }
        public virtual Pessoa Transportadora { get; set; }
        public virtual CondicaoParcelamento CondicaoParcelamento { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }
        public virtual Categoria Categoria { get; set; }
        #endregion


    }
}