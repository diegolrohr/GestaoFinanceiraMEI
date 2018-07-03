using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Fly01.Core
{
    public static class MessageType
    {
        public static List<T> Resolve<T>(string message)
        {
            var verifyModelType = JsonConvert.DeserializeObject<dynamic>(message);

            if (verifyModelType.Type == JTokenType.Array)
                return JsonConvert.DeserializeObject<List<T>>(message);

            return new List<T>() { JsonConvert.DeserializeObject<T>(message) };
        }
    }
}