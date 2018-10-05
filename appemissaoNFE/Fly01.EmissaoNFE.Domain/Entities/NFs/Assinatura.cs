using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;
using System;
using System.Text;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    public class Assinatura
    {
        public string CodigoIBGEPrestador { get; set; }

        public string GeraAssinatura(ItemTransmissaoNFSVM itemVM)
        {
            string cAssinatura = "";

            //Adiciona a inscricao municipal
            cAssinatura += itemVM.Prestador.InscricaoMunicipalPrestador.ToString().PadLeft(11, '0').ToLower().Trim();
            cAssinatura += "NF   ";
            cAssinatura += itemVM.Identificacao.NumeroRPS.ToString().Trim().PadLeft(12, '0').ToLower().Trim();
            cAssinatura += itemVM.Identificacao.DataHoraEmissao.ToString("yyyyMMdd").Trim();

            //Verificar as nossas tags e qual é o vdd
            switch (itemVM.Identificacao.TipoTributacao)
            {
                //Descomentar quando subir as mudanças no enum
                //case TipoTributacaoNFS.ForaMunicipio:
                //    cAssinatura += "H ";
                //    break;
                case TipoTributacaoNFS.Isencao:
                    cAssinatura += "C ";
                    break;
                case TipoTributacaoNFS.Imune:
                    cAssinatura += "F ";
                    break;
                case TipoTributacaoNFS.ExibilidadeSuspeJudicial:
                    cAssinatura += "K ";
                    break;
                case TipoTributacaoNFS.ExibilidadeProcessoADM:
                    cAssinatura += "K ";
                    break;
                default:
                    cAssinatura += "T ";
                    break;
            }

            cAssinatura += "N";
            cAssinatura += itemVM.Identificacao.TipoRecolhimento == TipoSimNao.Sim ? "S" : "N";
            cAssinatura += (itemVM.Valores.ValorTotalDocumento * 100).ToString().PadLeft(15, '0').ToLower().Trim();
            cAssinatura += "000000000000000";
            cAssinatura += itemVM.Atividade.CodigoCNAE.ToString().PadLeft(10, '0').ToLower().Trim();
            cAssinatura += itemVM.Tomador.CpfCnpj.ToString().PadLeft(14, '0').ToLower().Trim();

            cAssinatura = CalculateSHA1(cAssinatura);

            return cAssinatura.Trim().ToLower();
        }

        private string CalculateSHA1(string text)
        {
            try
            {
                byte[] buffer = Encoding.Default.GetBytes(text);
                System.Security.Cryptography.SHA1CryptoServiceProvider cryptoTransformSHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
                return hash;
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
        }
    }
}