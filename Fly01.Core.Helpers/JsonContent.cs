using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Fly01.Core.Helpers
{
    public class JsonContent : StringContent
    {
        public JsonContent(string content)
            : this(content, Encoding.UTF8)
        {
        }

        public JsonContent(object content)
            : this(content, Encoding.UTF8)
        {
        }

        public JsonContent(string content, Encoding encoding)
            : base(content, encoding, "application/json")
        {
        }

        public JsonContent(object content, Encoding encoding)
            : base(JsonConvert.SerializeObject(content), encoding, "application/json")
        {
        }
    }
}