using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Helpers;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class BankTransacVM : DomainBaseVM
    {
        [JsonProperty("operationType")]
        public string OperationType { get; set; }

        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreditDate { get; set; }

        [JsonProperty("creditDate")]
        public string CreditDateRest
        {
            get
            {
                return CreditDate.HasValue ? CreditDate.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                CreditDate = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

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

        [JsonProperty("category")]
        public string CategoryId { get; set; }

        [JsonIgnore]
        public string CategoryDescription { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("bankId")]
        public string BankId { get; set; }
        
        [JsonIgnore]
        public string BankDescription { get; set; }

        [JsonProperty("historic")]
        public string Historic { get; set; }

        [JsonProperty("discount")]
        [Display(Name = "Vlr.Desc.")]
        public double? Discount { get; set; }

        [JsonProperty("interest")]
        [Display(Name = "Juros")]
        public double? Interest { get; set; }
    }
}
