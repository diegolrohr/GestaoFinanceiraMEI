using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class AgreementbankBusinessVM : DomainBaseVM
    {
        //
        [JsonProperty("bankId")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string BankId { get; set; }

        //Informe o código do Banco.
        [JsonProperty("bankCode")]
        [Display(Name = "Banco")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string BankCode { get; set; }

        //Agencia do convenio.
        [JsonProperty("agencyCode")]
        [Display(Name = "Nro Agencia")]
        [StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string AgencyCode { get; set; }

        //Conta do convenio.
        [JsonProperty("account")]
        [Display(Name = "Nro Conta")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Account { get; set; }

        //Informe o convênio.
        [JsonProperty("agreement")]
        [Display(Name = "Convenio")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Agreement { get; set; }

        //Informe a operação de crédito
        [JsonProperty("creditOperation")]
        [Display(Name = "Ope. Crédito")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CreditOperation { get; set; }

        //Informe o número da variação da carteira (Ope. Crédito)
        [JsonProperty("changePortfolio")]
        [Display(Name = "Variação Carteira")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string ChangePortfolio { get; set; }

        //Informe o tipo de convênio (C=Cobrança/P=Pagamento)
        [JsonProperty("typeAgreement")]
        [Display(Name = "Tipo Convênio")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string TypeAgreement { get; set; }

        [JsonProperty("add")]
        public bool Add { get; set; }

        [JsonProperty("update")]
        public bool Update { get; set; }

        [JsonProperty("delete")]
        public bool Delete { get; set; }

        public AgreementbankBusinessVM()
        {
            this.Add = true;
            this.Update = true;
            this.Delete = false;
        }
    }
}
