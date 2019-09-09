using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.ViewModel
{
    [Serializable]
    public class MovimentacaoVM : EmpresaBaseVM
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public double Valor { get; set; }

        [JsonProperty("contaBancariaOrigemId")]
        public Guid? ContaBancariaOrigemId { get; set; }

        [JsonProperty("contaBancariaDestinoId")]
        public Guid? ContaBancariaDestinoId { get; set; }

        [JsonProperty("contaFinanceiraId")]
        public Guid? ContaFinanceiraId { get; set; }

        [JsonProperty("categoriaId")]
        public Guid? CategoriaId { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("contaBancariaOrigem")]
        public virtual ContaBancariaVM ContaBancariaOrigem { get; set; }

        [JsonProperty("contaBancariaDestino")]
        public virtual ContaBancariaVM ContaBancariaDestino { get; set; }

        [JsonProperty("contaFinanceira")]
        public virtual ContaFinanceiraVM ContaFinanceira { get; set; }

        [JsonProperty("categoria")]
        public virtual CategoriaVM Categoria { get; set; }
    }
}