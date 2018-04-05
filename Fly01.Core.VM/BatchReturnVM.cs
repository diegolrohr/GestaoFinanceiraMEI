using Newtonsoft.Json;

namespace Fly01.Core.VM
{
    [JsonObject("return")]
    public class BatchReturnVM
    {
        [JsonProperty("fileContent")]
        public string FileContent { get; set; }

        [JsonProperty("md5")]
        public string Md5 { get; set; }
    }
}