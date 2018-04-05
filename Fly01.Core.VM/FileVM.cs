using Newtonsoft.Json;

namespace Fly01.Core.VM
{
    public class FileVM
    {
        [JsonProperty("fileName")]
        public string FileName { get; set; }
        
        [JsonProperty("fileContent")]
        public string FileContent { get; set; }

        [JsonProperty("fileMD5")]
        public string FileMD5 { get; set; }
    }
}