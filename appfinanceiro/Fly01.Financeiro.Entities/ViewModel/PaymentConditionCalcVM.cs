using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Helpers;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class PaymentConditionCalcVM
    {
        [JsonProperty("bills")]
        public List<PaymentConditionCalcBills> Bills { get; set; }
    }

    [Serializable]
    public class PaymentConditionCalcBills
    {
        [JsonProperty("dueId")]
        public string DueId { get; set; }

        [JsonIgnore]
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

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}