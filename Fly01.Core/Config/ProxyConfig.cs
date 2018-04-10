namespace Fly01.Core.Config
{
    public class ProxyConfig
    {
        public bool Enabled { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public bool RequireAuthentication { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
    }
}
