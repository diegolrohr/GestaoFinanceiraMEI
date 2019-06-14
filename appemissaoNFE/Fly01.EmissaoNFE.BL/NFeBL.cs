using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS;
using Fly01.EmissaoNFE.Domain.Entities.NFe.IPI;
using Fly01.EmissaoNFE.Domain.Entities.NFe.PIS;
using Fly01.EmissaoNFE.Domain.Enums;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.BL
{
    public class NFeBL
    {
        public NFeBL(AppDataContextBase context)
        {
        }

        public string ConvertToXML(NFeVM entity, TipoCRT CodigoRegimeTributario)
        {
            string result = string.Empty;

            XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add("", @"http://www.portalfiscal.inf.br/nfe");

            entity.InfoNFe.Detalhes.ForEach(e =>
            {
                e.Imposto.ICMS.DoTheIcms();

                if (e.Imposto.IPI != null)
                    e.Imposto.IPI.DoTheIpi(TipoAliquota.AliquotaAdValorem);

                if (e.Imposto.PIS != null)
                    e.Imposto.PIS.DoThePIS(CodigoRegimeTributario);

                if (e.Imposto.COFINS != null)
                    e.Imposto.COFINS.DoTheCofins(CodigoRegimeTributario);
            });

            MemoryStream memoryStream = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true
            };

            XmlWriter writer = XmlWriter.Create(memoryStream, settings);

            XmlSerializer xser = new XmlSerializer(typeof(NFeVM), OverrideAttributes());

            xser.Serialize(writer, entity, nameSpaces);

            memoryStream.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(memoryStream);

            string xmlString = streamReader.ReadToEnd();
            xmlString = xmlString.Insert(0, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

            xmlString = Base64Helper.RemoverAcentos(xmlString);

            result = xmlString;

            return result;
        }

        private XmlAttributeOverrides OverrideAttributes()
        {
            XmlAttributeOverrides specific_attributes = new XmlAttributeOverrides();

            #region ICMS

            XmlAttributes attrsICMS = new XmlAttributes();
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMSSN101)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMSSN102)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMSSN201)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMSSN202)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMSSN500)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMSSN900)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMS00)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMS20)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMS30)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMS40)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMS41)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMS50)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMS51)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMS60)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMS70)));
            attrsICMS.XmlElements.Add(new XmlElementAttribute(typeof(ICMS90)));

            specific_attributes.Add(typeof(ICMSPai), "ICMS", attrsICMS);

            #endregion ICMS

            #region IPI

            XmlAttributes attrsIPI = new XmlAttributes();
            attrsIPI.XmlElements.Add(new XmlElementAttribute(typeof(IPITrib)));
            attrsIPI.XmlElements.Add(new XmlElementAttribute(typeof(IPINT)));

            specific_attributes.Add(typeof(IPIPai), "IPI", attrsIPI);

            #endregion IPI

            #region PIS

            XmlAttributes attrsPIS = new XmlAttributes();
            attrsPIS.XmlElements.Add(new XmlElementAttribute(typeof(PISOutr)));
            attrsPIS.XmlElements.Add(new XmlElementAttribute(typeof(PISNT)));
            attrsPIS.XmlElements.Add(new XmlElementAttribute(typeof(PISAliq)));
            attrsPIS.XmlElements.Add(new XmlElementAttribute(typeof(PISQtde)));

            specific_attributes.Add(typeof(PISPai), "PIS", attrsPIS);

            #endregion PIS

            #region COFINS

            XmlAttributes attrsCOFINS = new XmlAttributes();
            attrsCOFINS.XmlElements.Add(new XmlElementAttribute(typeof(COFINSAliq)));
            attrsCOFINS.XmlElements.Add(new XmlElementAttribute(typeof(COFINSQtde)));
            attrsCOFINS.XmlElements.Add(new XmlElementAttribute(typeof(COFINSNT)));
            attrsCOFINS.XmlElements.Add(new XmlElementAttribute(typeof(COFINSOutr)));

            specific_attributes.Add(typeof(COFINSPai), "COFINS", attrsCOFINS);

            #endregion COFINS

            return specific_attributes;
        }
    }
}
