using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class ConciliacaoBancariaVM : DomainBaseVM
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
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