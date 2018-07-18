using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using System;
using System.Collections.Generic;
using Fly01.Core.ViewModels;
using System.Text;
using System.IO;
using System.Linq;

namespace Fly01.Financeiro.BL
{
    public class ArquivoRetornoBL
    {
        public static List<Cnab> ImportarArquivoRetorno(ArquivoRetornoCnab entity, CedenteVM cedente, CnabBL cnabBL, ManagerEmpresaVM dadosEmpresa)
        {
            var cnabs = new List<Cnab>();
            var formatoArquivo = 1;
            var boletos = new Boleto2Net.Boletos()
            {
                Banco = Boleto2Net.Banco.Instancia(Convert.ToInt32(cedente.ContaBancariaCedente.CodigoBanco))
            };
            boletos.Banco.Cedente = GetCedente( dadosEmpresa, cedente);

            var byteArray = Encoding.ASCII.GetBytes(entity.ValueArquivo);
            MemoryStream pConteudo = new MemoryStream(byteArray);

            var arquivoRetorno = new Boleto2Net.ArquivoRetorno(boletos.Banco, (Boleto2Net.TipoArquivo)formatoArquivo);
            var boletosRetorno = arquivoRetorno.LerArquivoRetorno(pConteudo);

            foreach (var item in boletosRetorno)
            {
                var cnab = cnabBL.AllIncluding(x => x.ContaReceber, x => x.ContaReceber.Pessoa)
                                .FirstOrDefault(x => 
                                    x.NossoNumeroFormatado.Contains(item.NossoNumero)
                                    && x.Status != StatusCnab.Baixado);
                if (cnab != null)
                    cnabs.Add(cnab);
            }

            return cnabs;
        }

        private static Boleto2Net.Cedente GetCedente(ManagerEmpresaVM dadosEmpresa, CedenteVM cedente)
        {
            var contaBancaria = cedente.ContaBancariaCedente;
            var dadosCedente = dadosEmpresa;
            var carteira = BoletoBL.GetTipoCarteira(contaBancaria.CodigoBanco);

            return new Boleto2Net.Cedente
            {
                CPFCNPJ = dadosCedente.CNPJ,
                Nome = dadosCedente.NomeFantasia,
                Observacoes = "",
                ContaBancaria = new Boleto2Net.ContaBancaria
                {
                    Agencia = contaBancaria.Agencia,
                    DigitoAgencia = contaBancaria.DigitoAgencia,
                    OperacaoConta = "",
                    Conta = contaBancaria.Conta,
                    DigitoConta = contaBancaria.DigitoConta,
                    CarteiraPadrao = carteira.CarteiraPadrao,
                    VariacaoCarteiraPadrao = carteira.VariacaoCarteira,
                    TipoCarteiraPadrao = Boleto2Net.TipoCarteira.CarteiraCobrancaSimples,
                    TipoFormaCadastramento = Boleto2Net.TipoFormaCadastramento.ComRegistro,
                    TipoImpressaoBoleto = Boleto2Net.TipoImpressaoBoleto.Empresa,
                    TipoDocumento = Boleto2Net.TipoDocumento.Tradicional
                },
                Codigo = cedente.CodigoCedente,
                CodigoDV = cedente.CodigoDV,
                CodigoTransmissao = ""
            };
        }
    }
}
