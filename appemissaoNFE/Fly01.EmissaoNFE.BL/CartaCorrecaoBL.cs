using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Fly01.Core.Helpers;
using System.Collections.Generic;
using System;

namespace Fly01.EmissaoNFE.BL
{
    public class CartaCorrecaoBL : PlataformaBaseBL<CartaCorrecaoVM>
    {
        protected EntidadeBL EntidadeBL;
        protected const string TipoEvento = "110110";

        public CartaCorrecaoBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(CartaCorrecaoVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            entity.Fail(string.IsNullOrEmpty(entity.Correcao), new Error("Informe a mensagem de correção.", "Correcao"));
            entity.Fail(string.IsNullOrEmpty(entity.SefazChaveAcesso), new Error("Informe a chave da nota fiscal.", "SefazChaveAcesso"));
            entity.Fail(!string.IsNullOrEmpty(entity.Correcao) && entity.Correcao.Length > 1000, new Error("Tamanho da mensagem de correção deve conter até 1000 caracteres.", "Correcao"));
            entity.Fail(!string.IsNullOrEmpty(entity.SefazChaveAcesso) && entity.SefazChaveAcesso.Length != 44, new Error("Tamanho da chave de acesso deve conter 44 caracteres.", "SefazChaveAcesso"));

            base.ValidaModel(entity);
        }

        public string Serialize(CartaCorrecaoVM entity)
        {
            var cartaCorrecaoEvento = new CartaCorrecaoEvento()
            {
                TipoEvento = TipoEvento,
                Correcao = entity.Correcao,
                SefazChaveAcesso = entity.SefazChaveAcesso
            };

            var evento = new EnvelopeEvento()
            {
                Eventos = new Eventos()
                {
                    DetalhesEventos = new List<CartaCorrecaoEvento>()
                    //DetalhesEventos = new List<DetalheEvento>()
                    {
                        cartaCorrecaoEvento
                    }
                }
            };

            return ConvertToBase64(evento);
        }

        protected string ConvertToBase64(EnvelopeEvento entity)
        {
            string result = string.Empty;

            XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add("", "");

            MemoryStream memoryStream = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true
            };

            XmlWriter writer = XmlWriter.Create(memoryStream, settings);

            XmlSerializer xser = new XmlSerializer(typeof(EnvelopeEvento));

            xser.Serialize(writer, entity, nameSpaces);

            memoryStream.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(memoryStream);

            string xmlString = streamReader.ReadToEnd();            
            xmlString = xmlString.Insert(0, "<?xml version=\"1.0\"?>");

            xmlString = Base64Helper.RemoverAcentos(xmlString);

            result = Base64Helper.CodificaBase64(xmlString);

            return result;
        }

        private XmlAttributeOverrides OverrideAttributes()
        {
            XmlAttributeOverrides specific_attributes = new XmlAttributeOverrides();

            XmlAttributes attrs = new XmlAttributes();
            attrs.XmlElements.Add(new XmlElementAttribute(typeof(CartaCorrecaoEvento)));

            specific_attributes.Add(typeof(DetalheEvento), "detEvento", attrs);

            return specific_attributes;
        }
    }
}
