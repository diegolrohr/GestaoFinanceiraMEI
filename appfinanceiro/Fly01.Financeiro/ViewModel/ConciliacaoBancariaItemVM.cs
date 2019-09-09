using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using Fly01.Core.Helpers.Attribute;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class ConciliacaoBancariaItemVM : EmpresaBaseVM
    {
        [JsonProperty("conciliacaoBancariaId")]
        public Guid ConciliacaoBancariaId { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("data")]       
        public DateTime Data { get; set; }

        [JsonProperty("ofxLancamentoMD5")]
        public string OfxLancamentoMD5 { get; set; }

        [JsonProperty("statusConciliado")]
        [APIEnum("statusConciliado")]
        public string StatusConciliado { get; set; }

        #region Navigations Properties
        [JsonProperty("conciliacaoBancariaItemContasFinanceiras")]
        public virtual List<ConciliacaoBancariaItemContaFinanceiraVM> ConciliacaoBancariaItemContasFinanceiras { get; set; }

        #endregion
    }
}