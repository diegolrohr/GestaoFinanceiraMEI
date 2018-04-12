using Newtonsoft.Json;

namespace Fly01.Core.Entities.ViewModels
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