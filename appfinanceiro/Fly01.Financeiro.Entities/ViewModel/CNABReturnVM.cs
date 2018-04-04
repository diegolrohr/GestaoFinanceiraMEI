using System;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class CNABReturnVM : DomainBaseVM
    {
        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("fileContent")]
        public string FileContent { get; set; }

        [JsonProperty("fileMD5")]
        public string FileMD5 { get; set; }

        [JsonProperty("parametersBankId")]
        public string ParametersBankId { get; set; }

        [JsonIgnore]
        public string ParametersBankName { get; set; }
    }
}
