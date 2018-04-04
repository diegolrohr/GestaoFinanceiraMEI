using Fly01.Core.Api.Domain;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Financeiro.Domain.Entities
{
    public class ConciliacaoBancariaTransacao : PlataformaBase
    {
        //ConciliacaoBancariaItemContaFinanceiraVM
        [JsonProperty("conciliacaoBancariaItemId")]
        public Guid ConciliacaoBancariaItemId { get; set; }

        [JsonProperty("contaFinanceiraId")]
        public Guid ContaFinanceiraId { get; set; }

        [JsonProperty("valorConciliado")]
        public double ValorConciliado { get; set; }

        //ContaFinanceiraVM
        [JsonProperty("valorPrevisto")]
        public double ValorPrevisto { get; set; }

        [JsonProperty("categoriaId")]
        public Guid CategoriaId { get; set; }

        [JsonProperty("formaPagamentoId")]
        public Guid FormaPagamentoId { get; set; }

        [JsonProperty("condicaoParcelamentoId")]
        public Guid CondicaoParcelamentoId { get; set; }

        [JsonProperty("pessoaId")]
        public Guid PessoaId { get; set; }

        [JsonProperty("dataEmissao")]
        [Column(TypeName = "date")]
        public DateTime DataEmissao { get; set; }

        [JsonProperty("dataVencimento")]
        [Column(TypeName = "date")]
        public DateTime DataVencimento { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("tipoContaFinanceira")]
        public string TipoContaFinanceira { get; set; }
    }
}