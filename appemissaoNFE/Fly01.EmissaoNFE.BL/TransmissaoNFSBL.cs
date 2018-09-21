using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Fly01.Core.BL;
using Fly01.Core.Helpers;
using Fly01.EmissaoNFE.BL.Helpers;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;

namespace Fly01.EmissaoNFE.BL
{
    public class TransmissaoNFSBL : PlataformaBaseBL<TransmissaoNFSVM>
    {
        protected CidadeBL CidadeBL;
        protected EmpresaBL EmpresaBL;
        protected EntidadeBL EntidadeBL;
        protected EstadoBL EstadoBL;

        public TransmissaoNFSBL(AppDataContextBase context, CidadeBL cidadeBL, EmpresaBL empresaBL, EntidadeBL entidadeBL, EstadoBL estadoBL )
            : base(context)
        {
            CidadeBL = cidadeBL;
            EmpresaBL = empresaBL;
            EntidadeBL = entidadeBL;
            EstadoBL = estadoBL;
        }

        public override void ValidaModel(TransmissaoNFSVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            var entitesBLNFS = GetEntitiesBLToValidateNFS();

            var helperValidaModelTransmissaoNFS = new HelperValidaModelTransmissaoNFS(entity, entitesBLNFS);
            helperValidaModelTransmissaoNFS.ExecutarHelperValidaModelNFS();

            base.ValidaModel(entity);
        }

        public EntitiesBLToValidateNFS GetEntitiesBLToValidateNFS()
        {
            return new EntitiesBLToValidateNFS
            {
                _cidadeBL = CidadeBL, 
                _empresaBL = EmpresaBL, 
                _entidadeBL = EntidadeBL, 
                _estadoBL = EstadoBL
            };
        }

        public string SerializeNotaNFS(TransmissaoNFSVM entity)
        {
            return ConvertToXML(entity);
        }

        private string ConvertToXML(TransmissaoNFSVM entity)
        {
            XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();
            //nameSpaces.Add("", @"http://www.w3.org/2001/XMLSchema");

            var memoryStream = new MemoryStream();

            var settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true
            };

            var writer = XmlWriter.Create(memoryStream, settings);
            var xmlSerializer = new XmlSerializer(typeof(TransmissaoNFSVM));

            xmlSerializer.Serialize(writer, entity, nameSpaces);

            memoryStream.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            var streamReader = new StreamReader(memoryStream);

            var xmlString = streamReader.ReadToEnd();
            xmlString = xmlString.Insert(0, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

            return Base64Helper.RemoverAcentos(xmlString); ;
        }
    }
}
