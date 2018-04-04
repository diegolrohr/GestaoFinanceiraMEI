using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Helpers;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class BankStatementVM : DomainBaseVM
    {
        //Data do Crédito da Movimentacão
        [Display(Name = "Data Crédito")]
        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [JsonProperty("date")]
        public string DateRest
        {
            get
            {
                return Date.HasValue ? Date.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                Date = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

        //Codigo da categoria da movimentacao.
        [JsonProperty("categoryId")]
        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CategoryId { get; set; }

        //Informe a descrição dessa categoria
        [JsonProperty("categoryDescription")]
        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CategoryDescription { get; set; }

        //Valor do Movimento Bancário na moeda padrão
        [JsonProperty("value")]
        [Display(Name = "Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public double Value { get; set; }

        [JsonProperty("bankId")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string BankId { get; set; }

        //Identifica o código do banco referente a movimentação bancária
        [JsonProperty("bankCode")]
        [Display(Name = "Banco")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string BankCode { get; set; }

        [JsonProperty("personId")]
        public string PersonId { get; set; }

        [JsonProperty("personName")]
        [Display(Name = "Cliente/Fornecedor")]
        [StringLength(100, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string PersonName { get; set; }
        
        //Identifica o código da agência referente a movimentação bancária
        [JsonProperty("agency")]
        [Display(Name = "Agencia")]
        [StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Agency { get; set; }

        //Identifica a conta corrente referente a movimentação bancária
        [JsonProperty("account")]
        [Display(Name = "Conta")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Account { get; set; }

        //Número do documento referente a         movimentação bancária.
        [JsonProperty("checkId")]
        [Display(Name = "Documento")]
        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CheckId { get; set; }

        //Histórico da movimentação
        [JsonProperty("history")]
        [Display(Name = "Historico")]
        [StringLength(120, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string History { get; set; }

        [JsonProperty("balance")]
        [Display(Name = "Saldo")]
        public double Balance { get; set; }

        [JsonProperty("operationType")]
        [Display(Name = "OperationType")]
        public string OperationType { get; set; }
    }
}
