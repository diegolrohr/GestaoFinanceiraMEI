using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation
{
    [JsonObject("data")]
    public class BatchDataVM
    {
        [JsonProperty("fileContent")]
        public string FileContent { get; set; }

        [JsonProperty("md5")]
        public string Md5 { get; set; }
    }
}