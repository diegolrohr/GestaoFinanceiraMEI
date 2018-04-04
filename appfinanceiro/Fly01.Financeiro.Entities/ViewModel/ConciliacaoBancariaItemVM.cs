using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;
using Fly01.Core.Api;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class ConciliacaoBancariaItemVM : DomainBaseVM
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("conciliacaoBancariaId")]
        public Guid ConciliacaoBancariaId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("valor")]
        public double Valor { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("data")]       
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("ofxLancamentoMD5")]
        public string OfxLancamentoMD5 { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("statusConciliado")]
        [APIEnum("statusConciliado")]
        public string StatusConciliado { get; set; }

        #region Navigations Properties
        [JsonProperty("conciliacaoBancariaItemContasFinanceiras")]
        public virtual List<ConciliacaoBancariaItemContaFinanceiraVM> ConciliacaoBancariaItemContasFinanceiras { get; set; }

        #endregion
    }
}