using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class WalletVM : DomainBaseVM
    {
        //
        [JsonProperty("bankCode")]
        [Display(Name = "Chave")]
        [StringLength(9, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string BankCode { get; set; }

        //
        [JsonProperty("item")]
        [Display(Name = "Item")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Item { get; set; }

        //
        [JsonProperty("description")]
        [Display(Name = "Descricao")]
        [StringLength(150, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Description { get; set; }
    }
}
