using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Fly01.Core.BL;
using Fly01.Core.Helpers;
using Fly01.EmissaoNFE.BL.Helpers;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;
using Fly01.EmissaoNFE.Domain.Entities.NFS;
using System.Collections.Generic;

namespace Fly01.EmissaoNFE.BL
{
    public class TransmissaoNFSBL : PlataformaBaseBL<TransmissaoNFSVM>
    {
        protected CidadeBL CidadeBL;
        protected EmpresaBL EmpresaBL;
        protected EntidadeBL EntidadeBL;
        protected EstadoBL EstadoBL;

        public TransmissaoNFSBL(AppDataContextBase context, CidadeBL cidadeBL, EmpresaBL empresaBL, EntidadeBL entidadeBL, EstadoBL estadoBL)
            : base(context)
        {
            CidadeBL = cidadeBL;
            EmpresaBL = empresaBL;
            EntidadeBL = entidadeBL;
            EstadoBL = estadoBL;
        }

        public void MontarValores(TransmissaoNFSVM entity)
        {
            if (entity.ItemTransmissaoNFSVM.Servicos != null)
            {
                if (entity.ItemTransmissaoNFSVM.Valores == null)
                {
                    entity.ItemTransmissaoNFSVM.Valores = new Valores();
                }

                entity.ItemTransmissaoNFSVM.Valores.ISS = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorISS);
                entity.ItemTransmissaoNFSVM.Valores.ISSRetido = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorISSRetido);
                entity.ItemTransmissaoNFSVM.Valores.OutrasRetencoes = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorOutrasRetencoes);
                entity.ItemTransmissaoNFSVM.Valores.PIS = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorPIS);
                entity.ItemTransmissaoNFSVM.Valores.COFINS = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorCofins);
                entity.ItemTransmissaoNFSVM.Valores.INSS = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorINSS);
                entity.ItemTransmissaoNFSVM.Valores.IR = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorIR);
                entity.ItemTransmissaoNFSVM.Valores.CSLL = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorCSLL);
                entity.ItemTransmissaoNFSVM.Valores.ValorTotalDocumento = entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorTotal);
            }
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

        /// <summary>
        /// Permitimos enviar vários serviços por nota fiscal, mas para o xml é somente 1 serviço, concatenamos as descrições
        /// valores e impostos somados.. porém código nbs, código Iss, código tributário municipal será considerado do primeiro serviço
        /// </summary>
        /// <param name="entity"></param>
        public void AglutinarServicos(TransmissaoNFSVM entity)
        {
            if (entity.ItemTransmissaoNFSVM.Servicos.Count() > 1)
            {
                var servicoAglutinado = new Servico();
                var descricaoAglutinada = "";

                //se necessário colocar alguma ordem antes do FirstOrDefault
                var primeiroServico = entity.ItemTransmissaoNFSVM.Servicos.FirstOrDefault();
                entity.ItemTransmissaoNFSVM.Servicos.Remove(primeiroServico);
                descricaoAglutinada = primeiroServico.Descricao;

                primeiroServico.CopyProperties<Servico>(servicoAglutinado);

                servicoAglutinado.Quantidade = 1.00;
                servicoAglutinado.ValorTotal += entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorTotal);
                servicoAglutinado.ValorUnitario = servicoAglutinado.ValorTotal;
                servicoAglutinado.BaseCalculo = servicoAglutinado.ValorTotal;
                servicoAglutinado.ValorDeducoes += entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorDeducoes);
                servicoAglutinado.ValorPIS += entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorPIS);
                servicoAglutinado.ValorCofins += entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorCofins);
                servicoAglutinado.ValorINSS += entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorINSS);
                servicoAglutinado.ValorIR += entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorIR);
                servicoAglutinado.ValorCSLL += entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorCSLL);
                servicoAglutinado.ValorISS += entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorISS);
                servicoAglutinado.ValorISSRetido += entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorISSRetido);
                servicoAglutinado.ValorOutrasRetencoes += entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.ValorOutrasRetencoes);
                servicoAglutinado.DescontoCondicional += entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.DescontoCondicional);
                servicoAglutinado.DescontoIncondicional += entity.ItemTransmissaoNFSVM.Servicos.Sum(x => x.DescontoIncondicional);

                foreach (var item in entity.ItemTransmissaoNFSVM.Servicos)
                {
                    descricaoAglutinada += (" | " + item.Descricao);
                }
                servicoAglutinado.Descricao = descricaoAglutinada;
                                
                entity.ItemTransmissaoNFSVM.Servicos.Clear();
                entity.ItemTransmissaoNFSVM.Servicos.Add(servicoAglutinado);
            }

            if(entity.ItemTransmissaoNFSVM.FormatarCodigoIssServico)
            {
                entity.ItemTransmissaoNFSVM.Servicos[0].CodigoIss = FormatarCodigoISS(entity.ItemTransmissaoNFSVM.Servicos[0].CodigoIss);
            }
        }

        public string FormatarCodigoISS(string codigoISS = "")
        {
            if(codigoISS.Length == 3)
            {
                codigoISS = codigoISS.Insert(1,".");
            }
            else if(codigoISS.Length == 4)
            {
                codigoISS = codigoISS.Insert(2, ".");
            }

            return codigoISS;
        }
    }
}
