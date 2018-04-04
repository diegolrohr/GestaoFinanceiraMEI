using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly01.Core.SOAManager
{
    public class SOAConnectionConfig
    {
        public SOAConnectionConfig(string clientId, string user, string password)
        {
            MashupClientId = clientId;
            MashupUser = user;
            MashupPassword = password;
        }

        public string MashupClientId { get; set; }
        public string MashupUser { get; set; }
        public string MashupPassword { get; set; }
    }
}
