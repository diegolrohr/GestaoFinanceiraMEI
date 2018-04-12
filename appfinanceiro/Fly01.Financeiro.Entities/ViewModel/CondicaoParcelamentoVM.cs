using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
	public class CondicaoParcelamentoVM : DomainBaseVM
	{
		[JsonProperty("descricao")]
		[Display(Name = "Descrição")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(200, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Descricao { get; set; }

        [Display(Name = "Quantidade de Parcelas")]
        [JsonProperty("qtdParcelas")]
        public int? QtdParcelas { get; set; }

        [Display(Name = "Condições de Parcelamento")]
        [JsonProperty("condicoesParcelamento")]
        public string CondicoesParcelamento { get; set; }
    }
}
