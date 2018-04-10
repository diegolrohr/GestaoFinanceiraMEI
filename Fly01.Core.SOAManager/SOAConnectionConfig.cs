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
