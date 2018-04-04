using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Helpers;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class CheckVM : DomainBaseVM
    {
        [JsonProperty("id")]
        public string RecordId { get; set; }

        //Informe o número do banco do Cheque.
        [JsonProperty("bankCode")]
        [Display(Name = "Banco")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string BankCode { get; set; }

        [JsonProperty("bankDescription")]
        public string BankDescription { get; set; }

        [JsonIgnore]
        public string ContaFinanceiraId { get; set; }

        //Informa o número da agencia do Cheque.
        [JsonProperty("agencyNumber")]
        [Display(Name = "Agencia")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string AgencyNumber { get; set; }

        //Informa o número da conta do Cheque.
        [JsonProperty("accountNum")]
        [Display(Name = "Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string AccountNum { get; set; }

        //Informe o número do Cheque.
        [JsonProperty("checkNumber")]
        [Display(Name = "Cheque")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CheckNumber { get; set; }

        //Informe o valor do Cheque.
        [JsonProperty("value")]
        [Display(Name = "Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public double Value { get; set; }

        [Display(Name = "Data Cheque")]
        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CheckDate { get; set; }

        [JsonProperty("checkDate")]
        public string CheckDateRest
        {
            get
            {
                return CheckDate.HasValue ? CheckDate.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                CheckDate = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

        //Informe o código da pessoa se o cheque  for Nominal.
        [JsonProperty("person")]
        [Display(Name = "Cod.Pessoa")]
        [StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Person { get; set; }

        //
        [JsonProperty("status")]
        [Display(Name = "Status Do Cheque")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Status { get; set; }

        //Informe o historico do cheque.
        [JsonProperty("historic")]
        [Display(Name = "Historico")]
        public string Historic { get; set; }

        [Display(Name = "Emissão")]
        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EmissionDate { get; set; }

        [JsonProperty("emissionDate")]
        public string EmissionDateRest
        {
            get
            {
                return EmissionDate.HasValue ? EmissionDate.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                EmissionDate = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

        //Informa o nome da pessoa se o Cheque for nominal.
        [JsonProperty("nominal")]
        [Display(Name = "Nominal")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Nominal { get; set; }

        //Informa a data de baixa do Cheque.
        [Display(Name = "Data Baixa")]
        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DonwtownDate { get; set; }

        [JsonProperty("donwtownDate")]
        public string DonwtownDateRest
        {
            get
            {
                return DonwtownDate.HasValue ? DonwtownDate.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                DonwtownDate = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

        //Informa a cidade de emissão do Cheque.
        [JsonProperty("city")]
        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(30, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string City { get; set; }

        //
        [JsonProperty("sourceBranch")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string SourceBranch { get; set; }

        //Informa se o cheque é de Recebimento (contas a receber) ou Pagamento (contas a pagar)
        [JsonProperty("typeOperation")]
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string TypeOperation { get; set; }
    }
}
