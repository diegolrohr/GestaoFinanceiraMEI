using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Helpers;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class BankTransferVM : DomainBaseVM
    {
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

        [JsonProperty("sourceBankId")]
        public string SourceBankId { get; set; }

        [JsonIgnore]
        public string SourceBankDescription { get; set; }

        [JsonProperty("sourceCategoryId")]
        public string SourceCategoryId { get; set; }

        [JsonIgnore]
        public string SourceCategoryDescription { get; set; }

        [JsonProperty("destinationBankId")]
        public string DestinationBankId { get; set; }

        [JsonIgnore]
        public string DestinationBankDescription { get; set; }

        [JsonProperty("destinationCategoryId")]
        public string DestinationCategoryId { get; set; }

        [JsonIgnore]
        public string DestinationCategoryDescription { get; set; }

        [JsonProperty("movementType")]
        public string MovementType { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}