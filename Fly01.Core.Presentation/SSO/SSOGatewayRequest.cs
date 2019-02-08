namespace Fly01.Core.Presentation.SSO
{
    public class SSOGatewayRequest
    {
        public string AssertionUrl { get; set; }

        public string SSOUrl { get; set; }

        public string AppId { get; set; }

        public string AppPassword { get; set; }
    }
}