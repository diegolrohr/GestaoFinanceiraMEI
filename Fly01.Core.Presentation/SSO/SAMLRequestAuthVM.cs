using System;
using System.Text;
using System.Xml;

namespace Fly01.Core.Presentation.SSO
{
    public class SAMLRequestAuthVM
    {
        public string requestId { get; set; }
        public string issueInstant { get; set; }
        public string loginUrlSSO { get; set; }
        public string RelayState { get; set; }

        private string _authRequest2016 = @"<samlp:AuthnRequest
            xmlns:samlp=""urn:oasis:names:tc:SAML:2.0:protocol""
            xmlns:saml=""urn:oasis:names:tc:SAML:2.0:assertion""
            ID=""{0}""       
            Version=""2.0""
            IssueInstant=""{1}""
            AssertionConsumerServiceIndex=""0""
            AttributeConsumingServiceIndex=""0"">
            <saml:Issuer>serie1</saml:Issuer>           
            <samlp:NameIDPolicy
              AllowCreate=""true""
              Format=""urn:oasis:names:tc:SAML:2.0:nameid-format:emailAddress""/> 
          </samlp:AuthnRequest>";

        public SAMLRequestAuthVM()
        {
            requestId = Guid.NewGuid().ToString();
            issueInstant = issueInstant = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            //loginUrlSSO = AppConfig.LoginSSOUrl;
        }

        public string SAMLRequest
        {
            get
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(string.Format(_authRequest2016, requestId, issueInstant));
                //xmlDoc.LoadXml(string.Format(this._AuthNRequest, assertionLocation, messageID, issueInstant, binding, serviceProviderName));
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlDoc.OuterXml));
            }
            set { _authRequest2016 = value; }
        }
    }
}