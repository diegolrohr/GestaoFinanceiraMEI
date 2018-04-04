using Fly01.Utils.VM;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Estoque.Entities.ViewModel
{
    public class MovimentoVM : DomainBaseVM
    {
        [JsonProperty("quantidadeMovimento")]
        public double? QuantidadeMovimento { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("saldoAntesMovimento")]
        public double? SaldoAntesMovimento { get; set; }

        [JsonProperty("tipoMovimentoId")]
        public Guid TipoMovimentoId { get; set; }

        [JsonProperty("produtoId")]
        public Guid ProdutoId { get; set; }

        [JsonProperty("inventarioId")]
        public Guid? InventarioId { get; set; }

        #region Navigations Properties

        [JsonProperty("produto")]
        public virtual ProdutoVM Produto { get; set; }

        [JsonProperty("inventario")]
        public virtual InventarioVM Inventario { get; set; }

        [JsonProperty("tipoMovimento")]
        public virtual TipoMovimentoVM TipoMovimento { get; set; }

        #endregion

    }
}
