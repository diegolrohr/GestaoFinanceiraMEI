using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class CNABHistoryVM : DomainBaseVM
    {
        //
        [JsonProperty("accountReceivableId")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string AccountReceivableId { get; set; }

        //
        [JsonProperty("personId")]
        [Display(Name = "Pessoa")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string PersonId { get; set; }

        //
        [JsonProperty("personName")]
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string PersonName { get; set; }

        [Display(Name = "Emissão")]
        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? IssueDate { get; set; }

        [JsonProperty("issueDate")]
        public string IssueDateRest
        {
            get
            {
                return IssueDate.HasValue ? IssueDate.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                IssueDate = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

        [Display(Name = "Vencimento")]
        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        [JsonProperty("dueDate")]
        public string DueDateRest
        {
            get
            {
                return DueDate.HasValue ? DueDate.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                DueDate = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

        //
        [JsonProperty("value")]
        [Display(Name = "Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public double Value { get; set; }

        //
        [JsonProperty("movementId")]
        [Display(Name = "Cod.Movimento 1")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string MovementId { get; set; }

        //
        [JsonProperty("movementDescription")]
        [Display(Name = "Desc. Movimento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string MovementDescription { get; set; }

        //
        [JsonProperty("ocurrenceId")]
        [Display(Name = "Cod.Ocorrencia 1")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string OcurrenceId { get; set; }

        //
        [JsonProperty("ocurrenceDescription")]
        [Display(Name = "Desc.Ocorrencia")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string OcurrenceDescription { get; set; }

        //
        [JsonProperty("movementId2")]
        [Display(Name = "Cod.Movimento 2")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string MovementId2 { get; set; }

        //
        [JsonProperty("movementDescription2")]
        [Display(Name = "Desc. Movimento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string MovementDescription2 { get; set; }

        //
        [JsonProperty("ocurrenceId2")]
        [Display(Name = "Cod.Ocorrencia 2")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string OcurrenceId2 { get; set; }

        //
        [JsonProperty("ocurrenceDescription2")]
        [Display(Name = "Desc.Ocorrencia")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string OcurrenceDescription2 { get; set; }
    }
}
