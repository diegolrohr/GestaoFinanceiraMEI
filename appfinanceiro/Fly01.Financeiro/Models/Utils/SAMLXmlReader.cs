using System.Xml;

namespace Fly01.Financeiro.Models.Utils
{
    public class SAMLXmlReader
    {
        private XmlDocument xmlDoc { get; }
        private XmlNamespaceManager xMan { get; }
        private XmlNode xNode { get; set; }
        public SAMLXmlReader(string xmlContent)
        {

            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            xMan = new XmlNamespaceManager(xmlDoc.NameTable);
            xMan.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");
            xMan.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            xMan.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
        }

        public bool IsAuthenticated()
        {
            bool ret = false;
            XmlNode xNode = xmlDoc.SelectSingleNode("/samlp:Response/samlp:Status/samlp:StatusCode/@Value", xMan);
            if (xNode != null)
            {
                if (xNode.Value.EndsWith("status:Success"))
                    ret = true;
            }
            return ret;
        }

        public string GetResponseId()
        {
            string ret = null;
            xNode = xmlDoc.SelectSingleNode("/samlp:Response/@ID", xMan);
            if (xNode != null)
            {
                ret = xNode.Value;
            }
            return ret;
        }

        public string GetInResponseTo()
        {
            string ret = null;
            xNode = xmlDoc.SelectSingleNode("/samlp:Response/@InResponseTo", xMan);
            if (xNode != null)
            {
                ret = xNode.Value;
            }
            return ret;
        }

        public string GetIssuer()
        {
            string ret = null;
            xNode = xmlDoc.SelectSingleNode("/samlp:Response/saml:Issuer", xMan);
            if (xNode != null)
            {
                ret = xNode.InnerText;
            }
            return ret;
        }

        public string GetEmailAddress()
        {
            string ret = null;
            XmlNode node = xmlDoc.SelectSingleNode("/samlp:Response/saml:Assertion/saml:Subject/saml:NameID", xMan);
            if (node != null)
            {
                ret = node.InnerText;
            }
            return ret;
        }

        public string GetAttributeValue(string attributeName)
        {
            string ret = null;
            xNode = xmlDoc.SelectSingleNode("/samlp:Response/saml:Assertion/saml:AttributeStatement/saml:Attribute[@Name = '" + attributeName + "']/saml:AttributeValue", xMan);
            if (xNode != null)
                ret = xNode.InnerText;
            return ret;
        }
    }
}