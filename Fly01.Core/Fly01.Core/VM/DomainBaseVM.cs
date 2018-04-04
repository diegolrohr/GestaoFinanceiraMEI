using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.VM
{
    [Serializable]
    public class DomainBaseVM
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonIgnore]
        public bool IsClone { get; set; }

        ////Data de inclusão da pessoa
        //[JsonProperty("datainclusao")]
        //[Display(Name = "Data De Inclusão")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? DataInclusao { get; set; }

        ////Data de inclusão da pessoa
        //[JsonProperty("dataalteracao")]
        //[Display(Name = "Data De Alteração")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? DataAlteracao { get; set; }
    }
}