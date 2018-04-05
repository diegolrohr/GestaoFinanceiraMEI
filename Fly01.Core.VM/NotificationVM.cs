using Newtonsoft.Json;
using System;

namespace Fly01.Core.VM
{
    [Serializable]
    public class NotificationVM
    {
        //public string Chave { get; set; }
        //public int Codigo { get; set; }
        //public string Texto { get; set; }
        //public NotificationTypeVM TipoMensagem { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
