using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using Fly01.Financeiro.ViewModel;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class ConciliacaoBancariaVM : DomainBaseVM
    {
        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }

        [JsonProperty("arquivo")]
        public string Arquivo { get; set; }

        #region Navigations Properties
        [JsonProperty("contaBancaria")]
        public virtual ContaBancariaVM ContaBancaria { get; set; }

        [JsonProperty("conciliacaoBancariaItens")]
        public virtual List<ConciliacaoBancariaItemVM> ConciliacaoBancariaItens { get; set; }

        #endregion
    }
}