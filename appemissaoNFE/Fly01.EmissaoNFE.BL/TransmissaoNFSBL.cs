using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Fly01.Core.BL;
using Fly01.Core.Helpers;
using Fly01.EmissaoNFE.BL.Helpers;
using Fly01.EmissaoNFE.Domain.Entities.NFS;
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

        public TransmissaoNFSVM MontaValores(TransmissaoNFSVM entity)
        {
            if (entity.ItemTransmissaoNFSVM.Servicos != null)
            {
                entity.ItemTransmissaoNFSVM.Valores.ISS = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorISS);
                entity.ItemTransmissaoNFSVM.Valores.ISSRetido = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ISSRetido);
                entity.ItemTransmissaoNFSVM.Valores.OutrasRetencoes = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.OutrasRetencoes);
                entity.ItemTransmissaoNFSVM.Valores.PIS = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorPIS);
                entity.ItemTransmissaoNFSVM.Valores.COFINS = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorCofins);
                entity.ItemTransmissaoNFSVM.Valores.INSS = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorINSS);
                entity.ItemTransmissaoNFSVM.Valores.IR = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorIR);
                entity.ItemTransmissaoNFSVM.Valores.CSLL = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorCSLL);
                entity.ItemTransmissaoNFSVM.Valores.ValorTotalDocumento = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorTotal);
                entity.ItemTransmissaoNFSVM.Valores.ValorCarTributacao = 0;
                entity.ItemTransmissaoNFSVM.Valores.ValorPercapitaTributacao = CalcularValorPercapitaTributacao(entity);
            }

            return entity;
        }

        private double CalcularValorPercapitaTributacao(TransmissaoNFSVM entity)
        {
            var valorCargaTributaria = entity.ItemTransmissaoNFSVM.Valores.ValorCarTributacao;
            var ValorTotalDocumento = entity.ItemTransmissaoNFSVM.Valores.ValorTotalDocumento;
            var result = (valorCargaTributaria / ValorTotalDocumento) * 100;

            return result;
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
            var xmlSerializer = new XmlSerializer(typeof(ItemTransmissaoNFSVM));

            xmlSerializer.Serialize(writer, entity.ItemTransmissaoNFSVM, nameSpaces);

            memoryStream.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            var streamReader = new StreamReader(memoryStream);

            var xmlString = streamReader.ReadToEnd();
            xmlString = xmlString.Insert(0, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

            return Base64Helper.RemoverAcentos(xmlString); ;
        }
    }
}
