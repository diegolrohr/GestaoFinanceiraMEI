using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Domain;
using Fly01.Core.BL;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using System;
using System.Linq;
using Fly01.Core.API;

namespace Fly01.EmissaoNFE.BL
{
    public class TransmissaoBL : PlataformaBaseBL<TransmissaoVM>
    {
        protected CfopBL CfopBL;
        protected CidadeBL CidadeBL;
        protected EmpresaBL EmpresaBL;
        protected EntidadeBL EntidadeBL;
        protected EstadoBL EstadoBL;
        protected NFeBL NFeBL;
        private static string msgError;

        public TransmissaoBL(AppDataContextBase context, CfopBL cfopBL, CidadeBL cidadeBL, EmpresaBL empresaBL, EntidadeBL entidadeBL, EstadoBL estadoBL, NFeBL nfeBL) : base(context)
        {
            CfopBL = cfopBL;
            CidadeBL = cidadeBL;
            EmpresaBL = empresaBL;
            EntidadeBL = entidadeBL;
            EstadoBL = estadoBL;
            NFeBL = nfeBL;
        }

        public string SerializeNota(ItemTransmissaoVM nfe)
        {
            var nota = ConvertToNFe(nfe);

            var base64 = NFeBL.ConvertToBase64(nota, CRT.SimplesNacional);

            return base64;
        }

        public NFe ConvertToNFe(ItemTransmissaoVM item)
        {
            var nota = new NFe();
            nota.InfoNFe = new InfoNFe();

            item.NotaId = NotaId(
                                    item.Identificador.CodigoUF.ToString(),
                                    item.Identificador.Emissao.Year.ToString(),
                                    item.Identificador.Emissao.Month.ToString(),
                                    item.Emitente.Cnpj,
                                    item.Identificador.ModeloDocumentoFiscal.ToString(),
                                    item.Identificador.Serie.ToString(),
                                    item.Identificador.NumeroDocumentoFiscal.ToString(),
                                    ((int)item.Identificador.TipoDocumentoFiscal).ToString(),
                                    item.Identificador.CodigoNF.ToString()
                                );

            nota.InfoNFe.NotaId = item.NotaId;
            nota.InfoNFe.Versao = item.Versao;
            nota.InfoNFe.Identificador = item.Identificador;
            nota.InfoNFe.Identificador.ChaveAcessoDV = Int32.Parse(item.NotaId.Substring(46, 1));
            nota.InfoNFe.Identificador.CodigoNF = ZeroAEsquerda(item.Identificador.CodigoNF, 8);
            nota.InfoNFe.Emitente = item.Emitente;
            nota.InfoNFe.Destinatario = item.Destinatario;

            if ((int)nota.InfoNFe.Identificador.Ambiente == 2)
                nota.InfoNFe.Destinatario.Nome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";

            nota.InfoNFe.Detalhes = item.Detalhes;
            nota.InfoNFe.Total = item.Total;
            nota.InfoNFe.Transporte = item.Transporte;
            nota.InfoNFe.Cobranca = item.Cobranca;
            nota.InfoNFe.InformacoesAdicionais = item.InformacoesAdicionais;

            return nota;
        }

        public string NotaId(string UF, string ano, string mes, string cnpj, string modelo, string serie, string numeroNota, string tipoNota, string codigoNF)
        {
            var retorno = UF + ano[2] + ano[3] + ZeroAEsquerda(mes, 2) + ZeroAEsquerda(cnpj, 14) + ZeroAEsquerda(modelo, 2) +
                            ZeroAEsquerda(serie, 3) + ZeroAEsquerda(numeroNota, 9) + tipoNota + ZeroAEsquerda(codigoNF, 8);
            int dv = 0;

            if (retorno.Length != 43)
                throw new NotImplementedException();
            else
            {
                var peso = 2;
                var total = 0;
                for (int x = retorno.Length - 1; x >= 0; x--)
                {
                    int valor = 0;
                    Int32.TryParse(retorno[x].ToString(), out valor);

                    total = total + valor * peso;
                    peso++;

                    if (peso == 10)
                        peso = 2;
                }

                dv = 11 - (total % 11);

                if (dv > 9)
                    dv = 0;

            }

            var notaId = "NFe" + retorno + dv.ToString();

            return notaId;
        }

        public string ZeroAEsquerda(string texto, int tamanho)
        {
            string zerada = "";

            int y = 0;

            for (int x = 0; x < tamanho; x++)
            {
                if (x + texto.Length >= tamanho)
                {
                    zerada += texto[y];
                    y++;
                }
                else
                    zerada += "0";
            }

            return zerada;
        }

        public double Arredondar(double? valor, int casas)
        {
            double retorno = valor.HasValue ? valor.Value : 0;

            if (valor.HasValue)
            {
                string[] split = { "." };
                var numero = valor.ToString().Replace(",", ".").Split(split, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < numero.Length; x++)
                {
                    if (x == 0)
                        continue;
                    else
                        retorno = Math.Round(valor.Value, casas);
                }
            }

            return retorno;
        }

        public override void ValidaModel(TransmissaoVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            foreach (var item in entity.Item)
            {
                entity.Fail(string.IsNullOrEmpty(item.Versao), VersaoRequerida);

                #region Validações da classe Identificador

                if (item.Identificador == null)
                    entity.Fail(true, IdentificadorNulo);
                else
                {
                    entity.Fail(string.IsNullOrEmpty(EstadoBL.All.Where(e => e.CodigoIbge == item.Identificador.CodigoUF.ToString()).FirstOrDefault().CodigoIbge), CodigoUFInvalido);
                    entity.Fail(string.IsNullOrEmpty(item.Identificador.CodigoNF.ToString()), CodigoNFRequerido);
                    entity.Fail(string.IsNullOrEmpty(item.Identificador.NaturezaOperacao), NaturezaOperacaoRequerido);
                    entity.Fail((item.Identificador.FormaPagamento < 0 || (int)item.Identificador.FormaPagamento > 2), FormaPagamentoInvalida);
                    entity.Fail(string.IsNullOrEmpty(item.Identificador.Serie.ToString()), SerieRequerida);
                    entity.Fail(item.Identificador.Serie > 889 && item.Identificador.FormaEmissao == TipoModalidade.Normal, SerieNormalInvalida);
                    entity.Fail(item.Identificador.Serie < 900 && item.Identificador.FormaEmissao == TipoModalidade.SCAN, SerieContingenciaInvalida);
                    entity.Fail(string.IsNullOrEmpty(item.Identificador.NumeroDocumentoFiscal.ToString()), NumeroDocumentoRequerido);
                    entity.Fail((item.Identificador.TipoDocumentoFiscal < 0 || (int)item.Identificador.TipoDocumentoFiscal > 1), TipoNotaInvalido);
                    entity.Fail(((int)item.Identificador.DestinoOperacao < 1 || (int)item.Identificador.DestinoOperacao > 3), TipoDestinoInvalido);
                    entity.Fail(string.IsNullOrEmpty(CidadeBL.All.Where(e => e.CodigoIbge == item.Identificador.CodigoMunicipio.ToString()).FirstOrDefault().CodigoIbge), CodigoIBGEInvalido);
                    entity.Fail((item.Identificador.ImpressaoDANFE < 0 || (int)item.Identificador.ImpressaoDANFE > 5), TipoImpressaoDanfeInvalido);
                    entity.Fail(((int)item.Identificador.FormaEmissao < 1 || (int)item.Identificador.FormaEmissao > 7) && (int)item.Identificador.FormaEmissao != 9, FormaEmissaoInvalida);
                    entity.Fail(((int)item.Identificador.Ambiente < 1 || (int)item.Identificador.Ambiente > 2), AmbienteInvalido);
                    entity.Fail(((int)item.Identificador.FinalidadeEmissaoNFe < 1 || (int)item.Identificador.FinalidadeEmissaoNFe > 4), FinalidadeInvalida);
                    entity.Fail(item.Identificador.ConsumidorFinal != 0 && item.Identificador.ConsumidorFinal != 1, ConsumidorFinalInvalido);
                }

                #endregion

                #region Validações da classe Emitente

                if (item.Emitente == null)
                    entity.Fail(true, EmitenteNulo);
                else
                {
                    entity.Fail(item.Emitente.Cnpj == null && item.Emitente.Cpf == null, CpfouCnpjRequerido);
                    entity.Fail(item.Emitente.Cpf != null && (!EmpresaBL.ValidaCPF(item.Emitente.Cpf) || item.Emitente.Cnpj.Length != 11), CpfInvalido);
                    entity.Fail(item.Emitente.Cnpj != null && (!EmpresaBL.ValidaCNPJ(item.Emitente.Cnpj) || item.Emitente.Cnpj.Length != 14), CnpjInvalido);
                    entity.Fail(string.IsNullOrEmpty(item.Emitente.Nome), NomeRequerido);
                    entity.Fail(string.IsNullOrEmpty(item.Emitente.Endereco.Logradouro), LogradouroRequerido);
                    entity.Fail(string.IsNullOrEmpty(item.Emitente.Endereco.Numero), NumeroRequerido);
                    entity.Fail(string.IsNullOrEmpty(item.Emitente.Endereco.Bairro), BairroRequerido);
                    entity.Fail(string.IsNullOrEmpty(CidadeBL.All.Where(e => e.CodigoIbge == item.Emitente.Endereco.CodigoMunicipio.ToString()).FirstOrDefault().CodigoIbge), EnderecoCodigoIBGEInvalido);
                    entity.Fail(item.Emitente.Endereco.CodigoMunicipio != item.Identificador.CodigoMunicipio, CodigoIBGEDivergente);
                    entity.Fail(string.IsNullOrEmpty(item.Emitente.Endereco.Municipio), MunicipioRequerido);
                    if (item.Emitente.InscricaoEstadual != null)
                    {
                        if (!EmpresaBL.ValidaIE(item.Emitente.Endereco.UF, item.Emitente.InscricaoEstadual, out msgError))
                        {
                            switch (msgError)
                            {
                                case "1":
                                    entity.Fail(true, IEDigitoVerificador);
                                    break;
                                case "2":
                                    entity.Fail(true, IEQuantidadeDeDigitos);
                                    break;
                                case "3":
                                    entity.Fail(true, IEInvalida);
                                    break;
                                case "4":
                                    entity.Fail(true, UFInvalida);
                                    break;
                                case "5":
                                    entity.Fail(true, UFRequerida);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                        entity.Fail(true, IERequerida);

                    entity.Fail(item.Emitente.Endereco.Cep == null, CepRequerido);
                    entity.Fail(item.Emitente.Endereco.Cep != null && !EmpresaBL.ValidaCEP(item.Emitente.Endereco.Cep), CepInvalido);
                }

                #endregion

                #region Validações da classe Destinatario

                if (item.Destinatario == null)
                    entity.Fail(true, DestinatarioNulo);
                else
                {
                    entity.Fail(item.Destinatario.Cnpj == null && item.Destinatario.Cpf == null, CpfouCnpjDestinatarioRequerido);
                    entity.Fail(item.Destinatario.Cpf != null && (!EmpresaBL.ValidaCPF(item.Destinatario.Cpf) || item.Destinatario.Cpf.Length != 11), CpfDestinatarioInvalido);
                    entity.Fail(item.Destinatario.Cnpj != null && (!EmpresaBL.ValidaCNPJ(item.Destinatario.Cnpj) || item.Destinatario.Cnpj.Length != 14), CnpjDestinatarioInvalido);
                    entity.Fail(string.IsNullOrEmpty(item.Destinatario.Nome), NomeDestinatarioRequerido);
                    entity.Fail(string.IsNullOrEmpty(item.Destinatario.Endereco.Logradouro), LogradouroDestinatarioRequerido);
                    entity.Fail(string.IsNullOrEmpty(item.Destinatario.Endereco.Numero), NumeroDestinatarioRequerido);
                    entity.Fail(string.IsNullOrEmpty(item.Destinatario.Endereco.Bairro), BairroDestinatarioRequerido);
                    entity.Fail(item.Destinatario.Endereco.CodigoMunicipio != null && string.IsNullOrEmpty(CidadeBL.All.Where(e => e.CodigoIbge == item.Destinatario.Endereco.CodigoMunicipio.ToString()).FirstOrDefault().CodigoIbge), EnderecoCodigoIBGEDestinatarioInvalido);
                    entity.Fail(string.IsNullOrEmpty(item.Destinatario.Endereco.Municipio), MunicipioDestinatarioRequerido);
                    if (item.Destinatario.InscricaoEstadual != null && item.Destinatario.IndInscricaoEstadual == IndInscricaoEstadual.ContribuinteICMS)
                    {
                        if (!EmpresaBL.ValidaIE(item.Destinatario.Endereco.UF, item.Destinatario.InscricaoEstadual, out msgError))
                        {
                            switch (msgError)
                            {
                                case "1":
                                    entity.Fail(true, IEDigitoVerificadorDestinatario);
                                    break;
                                case "2":
                                    entity.Fail(true, IEQuantidadeDeDigitosDestinatario);
                                    break;
                                case "3":
                                    entity.Fail(true, IEInvalidaDestinatario);
                                    break;
                                case "4":
                                    entity.Fail(true, UFInvalidaDestinatario);
                                    break;
                                case "5":
                                    entity.Fail(true, UFRequeridaDestinatario);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    entity.Fail(item.Destinatario.Endereco.Cep != null && !EmpresaBL.ValidaCEP(item.Destinatario.Endereco.Cep), CepInvalidoDestinatario);
                }

                #endregion

                var nItem = 1;

                foreach (var detalhe in item.Detalhes)
                {
                    #region Validações da classe Detalhe.Produto

                    if (detalhe.Produto == null)
                        entity.Fail(true, new Error("Os dados de produto são obrigatórios. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto"));
                    else
                    {
                        detalhe.NumeroItem = nItem;

                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.Codigo),
                            new Error("Código do produto é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.Codigo"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.GTIN),
                            new Error("Codigo de barras (GTIN/EAN) do produto é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.GTIN"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.Descricao),
                            new Error("Descrição do produto é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.Descricao"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.NCM),
                            new Error("NCM é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.NCM"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.CFOP.ToString()),
                            new Error("CFOP é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.CFOP"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.UnidadeMedida),
                            new Error("Unidade de medida é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.UnidadeMedida"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.Quantidade),
                            new Error("Quantidade é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.Quantidade"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.ValorUnitario.ToString()),
                            new Error("Valor unitário é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorUnitario"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.ValorBruto.ToString()),
                            new Error("Valor bruto é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorUnitario"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.GTIN_UnidadeMedidaTributada),
                            new Error("Codigo de barras (GTIN/EAN) da unidade de tributação é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.GTIN_UnidadeMedidaTributada"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.UnidadeMedidaTributada),
                            new Error("Unidade de tributação é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.UnidadeMedidaTributada"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.QuantidadeTributada),
                            new Error("Quantidade de tributação é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.QuantidadeTributada"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.ValorUnitarioTributado.ToString()),
                            new Error("Valor unitário de tributação é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorUnitarioTributado"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Produto.AgregaTotalNota.ToString()),
                            new Error("Valor unitário de tributação é um dado obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.AgregaTotalNota"));

                        entity.Fail(detalhe.Produto.Codigo != null && (detalhe.Produto.Codigo.Length < 1 || detalhe.Produto.Codigo.Length > 60),
                            new Error("Código inválido. (Tam. 1-60) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.Codigo"));

                        entity.Fail(
                            detalhe.Produto.GTIN != null &&
                            !(detalhe.Produto.GTIN.Length == 0 ||
                            detalhe.Produto.GTIN.Length == 8 ||
                            detalhe.Produto.GTIN.Length == 12 ||
                            detalhe.Produto.GTIN.Length == 13 ||
                            detalhe.Produto.GTIN.Length == 14)
                        , new Error("Codigo de barras (GTIN/EAN) inválido. (Tam. 0/8/12/13/14) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.GTIN"));

                        entity.Fail(detalhe.Produto.Descricao != null && (detalhe.Produto.Descricao.Length < 1 || detalhe.Produto.Descricao.Length > 120),
                            new Error("Descrição inválida. (Tam. 1-120) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.Descricao"));
                        entity.Fail(detalhe.Produto.NCM != null && (detalhe.Produto.NCM.Length < 2 || detalhe.Produto.NCM.Length > 8),
                            new Error("NCM inválido. (Tam. 2-8) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.NCM"));
                        entity.Fail(detalhe.Produto.CFOP.ToString().Length != 4 || string.IsNullOrEmpty(CfopBL.All.Where(e => e.Codigo == detalhe.Produto.CFOP).FirstOrDefault().Codigo.ToString()),
                            new Error("CFOP inválido. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.CFOP"));
                        entity.Fail(detalhe.Produto.UnidadeMedida != null && (detalhe.Produto.UnidadeMedida.Length < 1 || detalhe.Produto.UnidadeMedida.Length > 6),
                            new Error("Unidade de medida inválida. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.UnidadeMedida"));

                        if (detalhe.Produto.Quantidade != null)
                        {
                            string[] split = { "." };
                            var numero = detalhe.Produto.Quantidade.Split(split, StringSplitOptions.RemoveEmptyEntries);

                            for (int x = 0; x < numero.Length; x++)
                            {
                                if (x == 0)
                                    entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Quantidade inválida. (Tam. 15.4) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.Quantidade"));
                                else
                                    entity.Fail(numero[x] != null && numero[x].Length > 4, new Error("Quantidade de casas decimais inválida. (Tam. 15.4) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.Quantidade"));
                            }
                        }

                        if (!string.IsNullOrEmpty(detalhe.Produto.ValorUnitario.ToString()))
                        {
                            string[] split = { "." };
                            var numero = detalhe.Produto.ValorUnitario.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                            for (int x = 0; x < numero.Length; x++)
                            {
                                if (x == 0)
                                    entity.Fail(numero[x].Length < 1 || numero[x].Length > 21, new Error("Valor unitário inválido. (Tam. 21.10) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorUnitario"));
                                else
                                    entity.Fail(numero[x] != null && numero[x].Length > 10, new Error("Quantidade de casas decimais inválida. (Tam. 21.10) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorUnitario"));
                            }
                        }

                        if (!string.IsNullOrEmpty(detalhe.Produto.ValorBruto.ToString()))
                        {
                            string[] split = { "." };
                            var numero = detalhe.Produto.ValorBruto.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                            for (int x = 0; x < numero.Length; x++)
                            {
                                if (x == 0)
                                    entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Valor bruto inválido. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorBruto"));
                                else
                                    entity.Fail(numero[x] != null && numero[x].Length > 2, new Error("Quantidade de casas decimais inválida. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorBruto"));
                            }
                        }

                        if (detalhe.Produto.GTIN_UnidadeMedidaTributada != null)
                        {
                            entity.Fail(
                            !(detalhe.Produto.GTIN_UnidadeMedidaTributada.Length == 0 ||
                            detalhe.Produto.GTIN_UnidadeMedidaTributada.Length == 8 ||
                            detalhe.Produto.GTIN_UnidadeMedidaTributada.Length == 12 ||
                            detalhe.Produto.GTIN_UnidadeMedidaTributada.Length == 13 ||
                            detalhe.Produto.GTIN_UnidadeMedidaTributada.Length == 14)
                            , new Error("Codigo de barras (GTIN/EAN) da unidade de tributação inválido. (Tam. 0/8/12/13/14) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.GTIN_UnidadeMedidaTributada"));
                        }

                        entity.Fail(detalhe.Produto.UnidadeMedidaTributada != null && (detalhe.Produto.UnidadeMedidaTributada.Length < 1 || detalhe.Produto.UnidadeMedidaTributada.Length > 6),
                            new Error("Unidade de tributação inválida. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.UnidadeMedidaTributada"));

                        if (detalhe.Produto.QuantidadeTributada != null)
                        {
                            string[] split = { "." };
                            var numero = detalhe.Produto.QuantidadeTributada.Split(split, StringSplitOptions.RemoveEmptyEntries);

                            for (int x = 0; x < numero.Length; x++)
                            {
                                if (x == 0)
                                    entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Quantidade inválida. (Tam. 15.4) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.QuantidadeTributada"));
                                else
                                    entity.Fail(numero[x] != null && numero[x].Length > 4, new Error("Quantidade de casas decimais inválida. (Tam. 15.4) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.QuantidadeTributada"));
                            }
                        }

                        if (!string.IsNullOrEmpty(detalhe.Produto.ValorUnitarioTributado.ToString()))
                        {
                            string[] split = { "." };
                            var numero = detalhe.Produto.ValorUnitarioTributado.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                            for (int x = 0; x < numero.Length; x++)
                            {
                                if (x == 0)
                                    entity.Fail(numero[x].Length < 1 || numero[x].Length > 21, new Error("Valor unitário tributado inválido. (Tam. 21.10) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorUnitarioTributado"));
                                else
                                    entity.Fail(numero[x] != null && numero[x].Length > 10, new Error("Quantidade de casas decimais inválida. (Tam. 21.10) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorUnitarioTributado"));
                            }
                        }

                        if (detalhe.Produto.ValorFrete != null)
                        {
                            string[] split = { "." };
                            var numero = detalhe.Produto.ValorFrete.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                            for (int x = 0; x < numero.Length; x++)
                            {
                                if (x == 0)
                                    entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Valor de frete inválido. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorFrete"));
                                else
                                    entity.Fail(numero[x] != null && numero[x].Length > 2, new Error("Quantidade de casas decimais inválida. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorFrete"));
                            }
                        }

                        if (detalhe.Produto.ValorSeguro != null)
                        {
                            string[] split = { "." };
                            var numero = detalhe.Produto.ValorSeguro.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                            for (int x = 0; x < numero.Length; x++)
                            {
                                if (x == 0)
                                    entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Valor de seguro inválido. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorSeguro"));
                                else
                                    entity.Fail(numero[x] != null && numero[x].Length > 2, new Error("Quantidade de casas decimais inválida. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorSeguro"));
                            }
                        }

                        if (detalhe.Produto.ValorDesconto != null)
                        {
                            string[] split = { "." };
                            var numero = detalhe.Produto.ValorDesconto.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                            for (int x = 0; x < numero.Length; x++)
                            {
                                if (x == 0)
                                    entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Valor de desconto inválido. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorDesconto"));
                                else
                                    entity.Fail(numero[x] != null && numero[x].Length > 2, new Error("Quantidade de casas decimais inválida. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorDesconto"));
                            }
                        }

                        if (detalhe.Produto.ValorOutrasDespesas != null)
                        {
                            string[] split = { "." };
                            var numero = detalhe.Produto.ValorOutrasDespesas.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                            for (int x = 0; x < numero.Length; x++)
                            {
                                if (x == 0)
                                    entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, new Error("Valor de outras despesas inválido. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorOutrasDespesas"));
                                else
                                    entity.Fail(numero[x] != null && numero[x].Length > 2, new Error("Quantidade de casas decimais inválida. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Produto.ValorOutrasDespesas"));
                            }
                        }

                    }
                    #endregion

                    #region Validações da classe Detalhe.Imposto

                    if (detalhe.Imposto == null)
                        entity.Fail(true, new Error("Os dados de imposto são obrigatórios. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto"));
                    else
                    {
                        var totalAprox = (detalhe.Imposto.COFINS != null ? detalhe.Imposto.COFINS.ValorCOFINS : 0) +
                                         (detalhe.Imposto.ICMS.ValorICMS.HasValue ? detalhe.Imposto.ICMS.ValorICMS.Value : 0) +
                                         (detalhe.Imposto.II != null ? detalhe.Imposto.II.ValorII : 0) +
                                         (detalhe.Imposto.IPI != null ? detalhe.Imposto.IPI.ValorIPI : 0) +
                                         (detalhe.Imposto.PIS != null ? detalhe.Imposto.PIS.ValorPIS : 0) +
                                         (detalhe.Imposto.PISST != null ? detalhe.Imposto.PISST.ValorPISST : 0);

                        entity.Fail(!totalAprox.Equals(detalhe.Imposto.TotalAprox), new Error("Total aproximado de impostos inválido. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto"));

                        #region Validações da classe Imposto.ICMS

                        if (detalhe.Imposto.ICMS == null)
                            entity.Fail(true, new Error("Os dados de ICMS são obrigatórios. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS"));
                        else
                        {
                            entity.Fail(detalhe.Imposto.ICMS.OrigemMercadoria < 0 || (int)detalhe.Imposto.ICMS.OrigemMercadoria > 8, 
                                new Error("Origem da mercadoria inválida. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.OrigemMercadoria"));

                            var Modalidade = EnumHelper.GetDataEnumValues(typeof(ModalidadeDeterminacaoBCICMS));
                            var ModalidadeST = EnumHelper.GetDataEnumValues(typeof(ModalidadeDeterminacaoBCICMSST));

                            switch (((int)detalhe.Imposto.ICMS.CodigoSituacaoOperacao).ToString())
                            {
                                case "101": //Tributada pelo Simples Nacional com permissão de crédito
                                    entity.Fail(!detalhe.Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN.HasValue, 
                                        new Error("Alíquota aplicável de cálculo do crédito é obrigatória para CSOSN 101 e 201. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN"));
                                    entity.Fail(!detalhe.Imposto.ICMS.ValorCreditoICMS.HasValue, 
                                        new Error("Valor crédito do ICMS é obrigatório para CSOSN 101 e 201. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorCreditoICMS"));
                                    break;

                                case "102": //Tributada pelo Simples Nacional sem permissão de crédito
                                    break;

                                case "103": //Isenção do ICMS no Simples Nacional para faixa de receita bruta
                                    break;

                                case "201": //Tributada pelo Simples Nacional com permissão de crédito e com cobrança do ICMS por substituição tributária
                                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()), 
                                        new Error("Modalidade de determinação da base de cálculo do ICMS ST é obrigatória para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ModalidadeBCST"));
                                    entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()) && !ModalidadeST.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.ICMS.ModalidadeBCST)), 
                                        new Error("Modalidade de determinação da base de cálculo inválida. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ModalidadeBCST"));
                                    entity.Fail(!detalhe.Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN.HasValue, 
                                        new Error("Alíquota aplicável de cálculo do crédito é obrigatória para CSOSN 101 e 201. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN"));
                                    entity.Fail(!detalhe.Imposto.ICMS.ValorCreditoICMS.HasValue, 
                                        new Error("Valor crédito do ICMS é obrigatório para CSOSN 101 e 201. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorCreditoICMS"));
                                    entity.Fail(!detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST.HasValue, 
                                        new Error("Percentual da MVA do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.PercentualMargemValorAdicionadoST"));
                                    entity.Fail(!detalhe.Imposto.ICMS.ValorBCST.HasValue, 
                                        new Error("Valor da base de cálculo do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorBCST"));
                                    entity.Fail(!detalhe.Imposto.ICMS.AliquotaICMSST.HasValue, 
                                        new Error("Alíquota do ICMS ST é obrigatória para CSOSN 201, 202 e 203. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.AliquotaICMSST"));
                                    entity.Fail(!detalhe.Imposto.ICMS.ValorICMSST.HasValue, 
                                        new Error("Valor do ICMS ST é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorICMSST"));
                                    break;

                                case "202": //Tributada pelo Simples Nacional sem permissão de crédito e com cobrança do ICMS por substituição tributária
                                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()), 
                                        new Error("Modalidade de determinação da base de cálculo do ICMS ST é obrigatória para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ModalidadeBCST"));
                                    entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()) && !ModalidadeST.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.ICMS.ModalidadeBCST)), 
                                        new Error("Modalidade de determinação da base de cálculo inválida. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ModalidadeBCST"));
                                    entity.Fail(!detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST.HasValue, 
                                        new Error("Percentual da MVA do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.PercentualMargemValorAdicionadoST"));
                                    entity.Fail(!detalhe.Imposto.ICMS.ValorBCST.HasValue, 
                                        new Error("Valor da base de cálculo do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorBCST"));
                                    entity.Fail(!detalhe.Imposto.ICMS.AliquotaICMSST.HasValue, 
                                        new Error("Alíquota do ICMS ST é obrigatória para CSOSN 201, 202 e 203. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.AliquotaICMSST"));
                                    entity.Fail(!detalhe.Imposto.ICMS.ValorICMSST.HasValue, 
                                        new Error("Valor do ICMS ST é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorICMSST"));
                                    break;

                                case "203": //Isenção do ICMS no Simples Nacional para faixa de receita bruta e com cobrança do ICMS por substituição tributária
                                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()), 
                                        new Error("Modalidade de determinação da base de cálculo do ICMS ST é obrigatória para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ModalidadeBCST"));
                                    entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()) && !ModalidadeST.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.ICMS.ModalidadeBCST)), 
                                        new Error("Modalidade de determinação da base de cálculo inválida. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ModalidadeBCST"));
                                    entity.Fail(!detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST.HasValue, 
                                        new Error("Percentual da MVA do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.PercentualMargemValorAdicionadoST"));
                                    entity.Fail(!detalhe.Imposto.ICMS.ValorBCST.HasValue, 
                                        new Error("Valor da base de cálculo do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorBCST"));
                                    entity.Fail(!detalhe.Imposto.ICMS.AliquotaICMSST.HasValue, 
                                        new Error("Alíquota do ICMS ST é obrigatória para CSOSN 201, 202 e 203. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.AliquotaICMSST"));
                                    entity.Fail(!detalhe.Imposto.ICMS.ValorICMSST.HasValue, 
                                        new Error("Valor do ICMS ST é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorICMSST"));
                                    break;

                                case "300": //Imune
                                    break;

                                case "400": //Não tributada pelo Simples Nacional
                                    break;

                                case "500": //ICMS cobrado anteriormente por substituição tributária (substituído) ou por antecipação
                                    entity.Fail(!detalhe.Imposto.ICMS.ValorBCSTRetido.HasValue, 
                                        new Error("Valor da base de cálculo do ICMS substituído é obrigatório para CSOSN 500. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorBCSTRetido"));
                                    entity.Fail(!detalhe.Imposto.ICMS.ValorICMSSTRetido.HasValue, 
                                        new Error("Valor do ICMS substituído é obrigatório para CSOSN 500. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorICMSSTRetido"));
                                    break;

                                case "900": //Outros
                                            //Informação do CSOSN e valor do ICMS passível de crédito pelo destinatário
                                    entity.Fail(!detalhe.Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN.HasValue && detalhe.Imposto.ICMS.ValorCreditoICMS.HasValue, 
                                        new Error("Percentual de crédito é obrigatório para operações passíveis de crédito do ICMS. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN"));
                                    entity.Fail(detalhe.Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN.HasValue && !detalhe.Imposto.ICMS.ValorCreditoICMS.HasValue, 
                                        new Error("Valor de crédito é obrigatório para operações passíveis de crédito do ICMS. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorCreditoICMS"));

                                    //Informação do CSOSN e ICMS próprio
                                    var ICMSProprio = false;
                                    if (!string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBC.ToString()) ||
                                      (detalhe.Imposto.ICMS.PercentualReducaoBC.HasValue && detalhe.Imposto.ICMS.PercentualReducaoBC > 0) ||
                                      (detalhe.Imposto.ICMS.ValorBC.HasValue && detalhe.Imposto.ICMS.ValorBC > 0) ||
                                      (detalhe.Imposto.ICMS.AliquotaICMS.HasValue && detalhe.Imposto.ICMS.AliquotaICMS > 0) ||
                                      (detalhe.Imposto.ICMS.ValorICMS.HasValue && detalhe.Imposto.ICMS.ValorICMS > 0))
                                    {
                                        ICMSProprio = true;
                                        entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBC.ToString()), 
                                            new Error("Modalidade de determinação da base de cálculo é obrigatória para operações de ICMS próprio. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ModalidadeBC"));
                                        entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBC.ToString()) && !Modalidade.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.ICMS.ModalidadeBC)), 
                                            new Error("Modalidade de determinação da base de cálculo inválida. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ModalidadeBC"));
                                        entity.Fail(!detalhe.Imposto.ICMS.ValorBC.HasValue, 
                                            new Error("Base de cálculo requerida para operações de ICMS próprio. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorBC"));
                                        entity.Fail(!detalhe.Imposto.ICMS.AliquotaICMS.HasValue, 
                                            new Error("Alíquota requerida para operações de ICMS próprio. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorBC"));
                                        entity.Fail(!detalhe.Imposto.ICMS.ValorICMS.HasValue, 
                                            new Error("Valor do imposto requerido para operações de ICMS próprio. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorBC"));
                                    }

                                    //Informação do CSOSN, ICMS próprio e ICMS ST
                                    if (ICMSProprio &
                                      (!string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()) ||
                                      (detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST.HasValue && detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST > 0) ||
                                      (detalhe.Imposto.ICMS.PercentualReducaoBCST.HasValue && detalhe.Imposto.ICMS.PercentualReducaoBCST > 0) ||
                                      (detalhe.Imposto.ICMS.ValorBCST.HasValue && detalhe.Imposto.ICMS.ValorBCST > 0) ||
                                      (detalhe.Imposto.ICMS.AliquotaICMSST.HasValue && detalhe.Imposto.ICMS.AliquotaICMSST > 0) ||
                                      (detalhe.Imposto.ICMS.ValorICMSST.HasValue && detalhe.Imposto.ICMS.ValorICMSST > 0)
                                      ))
                                    {
                                        entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()), 
                                            new Error("Modalidade de determinação da base de cálculo do ICMS ST é obrigatória para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ModalidadeBCST"));
                                        entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()) && !ModalidadeST.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.ICMS.ModalidadeBCST)), 
                                            new Error("Modalidade de determinação da base de cálculo inválida. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ModalidadeBCST"));
                                        entity.Fail(!detalhe.Imposto.ICMS.ValorBCST.HasValue, 
                                            new Error("Valor da base de cálculo do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorBCST"));
                                        entity.Fail(!detalhe.Imposto.ICMS.AliquotaICMSST.HasValue, 
                                            new Error("Alíquota da Substituição Tributária é requerida. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorBC"));
                                        entity.Fail(!detalhe.Imposto.ICMS.ValorICMSST.HasValue, 
                                            new Error("Valor da Substituição Tributária é requerido. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorBC"));
                                    }
                                    break;

                                default:
                                    entity.Fail(true, new Error("CSOSN inválido. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.CodigoSituacaoOperacao"));
                                    break;
                            }
                        }

                        #endregion

                        #region Validação da classe Imposto.IPI

                        if (detalhe.Imposto.IPI != null)
                        {
                            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.CodigoEnquadramento), 
                                new Error("Código de enquadramento legal do IPI é obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.IPI.CodigoEnquadramento"));
                            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.CodigoST.ToString()), 
                                new Error("CST do IPI é obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.IPI.CodigoST"));
                            entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.IPI.CodigoST.ToString()) && (int)detalhe.Imposto.IPI.CodigoST < 50 && (int)item.Identificador.TipoDocumentoFiscal == 1, 
                                new Error("CST do IPI inválido para uma nota de saída. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.IPI.CodigoST"));
                            entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.IPI.CodigoST.ToString()) && (int)detalhe.Imposto.IPI.CodigoST >= 50 && (int)item.Identificador.TipoDocumentoFiscal == 0, 
                                new Error("CST do IPI inválido para uma nota de entrada. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.IPI.CodigoST"));
                            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.ValorBaseCalculo.ToString()), 
                                new Error("Base de cálculo do IPI é obrigatória. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.IPI.ValorBaseCalculo"));
                            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.PercentualIPI.ToString()), 
                                new Error("Alíquota do IPI é obrigatória. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.IPI.PercentualIPI"));
                            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.ValorIPI.ToString()), 
                                new Error("Valor do IPI é obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.IPI.ValorIPI"));
                            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.QtdTotalUnidadeTributavel.ToString()), 
                                new Error("Quantidade tributada do IPI é obrigatória. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.IPI.QtdTotalUnidadeTributavel"));
                            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.ValorUnidadeTributavel.ToString()), 
                                new Error("Valor por unidade tributável do IPI é obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.IPI.ValorUnidadeTributavel"));
                        }

                        #endregion

                        #region Validações da classe Imposto.PIS

                        if (detalhe.Imposto.PIS == null)
                            entity.Fail(true, new Error("Os dados de PIS são obrigatórios. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PIS"));
                        else
                        {
                            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PIS.CodigoSituacaoTributaria.ToString()), 
                                new Error("O CST do PIS é obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PIS.CodigoSituacaoTributaria"));
                            var CSTPIS = EnumHelper.GetDataEnumValues(typeof(CSTPISCOFINS));
                            entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.PIS.CodigoSituacaoTributaria.ToString()) && !CSTPIS.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.PIS.CodigoSituacaoTributaria)), 
                                new Error("Código CST inválido. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PIS.CodigoSituacaoTributaria"));

                            var OnlyCST = "04||05||06||07||08||09";

                            if (!OnlyCST.Contains(((int)detalhe.Imposto.PIS.CodigoSituacaoTributaria).ToString()))
                            {
                                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PIS.ValorBCDoPIS.ToString()), 
                                    new Error("A base de cálculo do PIS é obrigatória. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PIS.ValorBCDoPIS"));
                                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PIS.PercentualPIS.ToString()), 
                                    new Error("A alíquota do PIS é obrigatória. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PIS.PercentualPIS"));
                                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PIS.ValorPIS.ToString()), 
                                    new Error("O valor do PIS é obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PIS.ValorPIS"));

                                if (!string.IsNullOrEmpty(detalhe.Imposto.PIS.ValorBCDoPIS.ToString()))
                                {
                                    string[] split = { "." };
                                    var numero = detalhe.Imposto.PIS.ValorBCDoPIS.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                                    for (int x = 0; x < numero.Length; x++)
                                    {
                                        if (x == 0)
                                            entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, 
                                                new Error("O valor da base de cálculo do PIS é inválido. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PIS.ValorBCDoPIS"));
                                        else
                                            entity.Fail(numero[x] != null && numero[x].Length > 2, 
                                                new Error("O número de casas decimais da base de cálculo do PIS é inválido. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PIS.ValorBCDoPIS"));
                                    }
                                }
                                if (!string.IsNullOrEmpty(detalhe.Imposto.PIS.PercentualPIS.ToString()))
                                {
                                    string[] split = { "." };
                                    var numero = detalhe.Imposto.PIS.PercentualPIS.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                                    for (int x = 0; x < numero.Length; x++)
                                    {
                                        if (x == 0)
                                            entity.Fail(numero[x].Length < 1 || numero[x].Length > 5, 
                                                new Error("A alíquota do PIS é inválida. (Tam. 5.2-4) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PIS.PercentualPIS"));
                                        else
                                            entity.Fail(numero[x] != null && (numero[x].Length < 2 || numero[x].Length > 4), 
                                                new Error("O número de casas decimais da alíquota do PIS é inválido. (Tam. 5.2-4) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PIS.PercentualPIS"));
                                    }
                                }
                                if (!string.IsNullOrEmpty(detalhe.Imposto.PIS.ValorPIS.ToString()))
                                {
                                    string[] split = { "." };
                                    var numero = detalhe.Imposto.PIS.ValorPIS.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                                    for (int x = 0; x < numero.Length; x++)
                                    {
                                        if (x == 0)
                                            entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, 
                                                new Error("O valor do PIS é inválido. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PIS.ValorPIS"));
                                        else
                                            entity.Fail(numero[x] != null && numero[x].Length > 2, 
                                                new Error("O número de casas decimais do PIS é inválido. (Tam. 15.2) Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PIS.ValorPIS"));
                                    }
                                }
                            }

                            #region Validações da classe Imposto.PISST

                            if ((int)detalhe.Imposto.PIS.CodigoSituacaoTributaria == 5 || (int)detalhe.Imposto.PIS.CodigoSituacaoTributaria == 75)
                            {
                                entity.Fail(detalhe.Imposto.PISST == null, new Error("Os dados de PIS ST são obrigatórios para o CST 05. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PISST"));
                                if (detalhe.Imposto.PISST != null)
                                {
                                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PISST.ValorBC.ToString()), 
                                        new Error("Base do PIS ST é obrigatória. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PISST.ValorBC"));
                                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PISST.AliquotaPercentual.ToString()), 
                                        new Error("Alíquota do PIS ST é obrigatória. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PISST.AliquotaPercentual"));
                                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PISST.ValorPISST.ToString()), 
                                        new Error("Valor do PIS ST é obrigatório. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PISST.ValorPISST"));

                                    if (!string.IsNullOrEmpty(detalhe.Imposto.PISST.ValorBC.ToString()))
                                    {
                                        string[] split = { "." };
                                        var numero = detalhe.Imposto.PISST.ValorBC.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                                        for (int x = 0; x < numero.Length; x++)
                                        {
                                            if (x == 0)
                                                entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, 
                                                    new Error("Base do PIS ST inválida. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PISST.ValorBC"));
                                            else
                                                entity.Fail(numero[x] != null && numero[x].Length > 2, 
                                                    new Error("Casas decimais inválidas na base do PIS ST. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PISST.ValorBC"));
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(detalhe.Imposto.PISST.AliquotaPercentual.ToString()))
                                    {
                                        string[] split = { "." };
                                        var numero = detalhe.Imposto.PISST.AliquotaPercentual.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                                        for (int x = 0; x < numero.Length; x++)
                                        {
                                            if (x == 0)
                                                entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, 
                                                    new Error("Alíquota do PIS ST inválida. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PISST.AliquotaPercentual"));
                                            else
                                                entity.Fail(numero[x] != null && (numero[x].Length < 2 || numero[x].Length > 4), 
                                                    new Error("Casas decimais inválidas na alíquota do PIS ST. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PISST.AliquotaPercentual"));
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(detalhe.Imposto.PISST.ValorPISST.ToString()))
                                    {
                                        string[] split = { "." };
                                        var numero = detalhe.Imposto.PISST.ValorPISST.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                                        for (int x = 0; x < numero.Length; x++)
                                        {
                                            if (x == 0)
                                                entity.Fail(numero[x].Length < 1 || numero[x].Length > 15, 
                                                    new Error("Valor do PIS ST inválido. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PISST.ValorPISST"));
                                            else
                                                entity.Fail(numero[x] != null && numero[x].Length > 2, 
                                                    new Error("Casas decimais inválidas no valor do PIS ST. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.PISST.ValorPISST"));
                                        }
                                    }
                                }
                            }

                            #endregion
                        }

                        #endregion

                        #region Validações da classe Imposto.II


                        #endregion

                    }

                    #endregion

                    nItem++;
                }

                #region Validação da classe Totais

                if (item.Total == null)
                    entity.Fail(true, TotalNulo);
                else
                {
                    if (item.Total.ICMSTotal == null)
                        entity.Fail(true, ICMSTotalNulo);
                    else
                    {
                        #region SomatorioBC
                        double? somatorioBCTrue = item.Detalhes.Sum(e => e.Imposto.ICMS.ValorBC.HasValue ? e.Imposto.ICMS.ValorBC.Value : 0);
                        item.Total.ICMSTotal.SomatorioBC = Arredondar(item.Total.ICMSTotal.SomatorioBC, 2);
                        somatorioBCTrue = Arredondar(somatorioBCTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioBC.ToString()), SomatorioBCRequerido);
                        entity.Fail(!somatorioBCTrue.Equals(item.Total.ICMSTotal.SomatorioBC), SomatorioBCInvalido);
                        #endregion

                        #region SomatorioICMS
                        double? somatorioICMSTrue = item.Detalhes.Sum(e => e.Imposto.ICMS.ValorICMS.HasValue ? e.Imposto.ICMS.ValorICMS.Value : 0);
                        item.Total.ICMSTotal.SomatorioICMS = Arredondar(item.Total.ICMSTotal.SomatorioICMS, 2);
                        somatorioICMSTrue = Arredondar(somatorioICMSTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioICMS.ToString()), SomatorioICMSRequerido);
                        entity.Fail(!somatorioICMSTrue.Equals(item.Total.ICMSTotal.SomatorioICMS), SomatorioICMSInvalido);
                        #endregion

                        #region SomatorioBCST
                        double? somatorioBCSTTrue = item.Detalhes.Sum(e => e.Imposto.ICMS.ValorBCST.HasValue ? e.Imposto.ICMS.ValorBCST.Value : 0);
                        item.Total.ICMSTotal.SomatorioBCST = Arredondar(item.Total.ICMSTotal.SomatorioBCST, 2);
                        somatorioBCSTTrue = Arredondar(somatorioBCSTTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioBCST.ToString()), SomatorioBCSTRequerido);
                        entity.Fail(!somatorioBCSTTrue.Equals(item.Total.ICMSTotal.SomatorioBCST), SomatorioBCSTInvalido);

                        #endregion

                        #region SomatorioICMSST
                        double? somatorioICMSSTTrue = item.Detalhes.Sum(e => e.Imposto.ICMS.ValorICMSST.HasValue ? e.Imposto.ICMS.ValorICMSST.Value : 0);
                        item.Total.ICMSTotal.SomatorioICMSST = Arredondar(item.Total.ICMSTotal.SomatorioICMSST, 2);
                        somatorioICMSSTTrue = Arredondar(somatorioICMSSTTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioICMSST.ToString()), SomatorioICMSSTRequerido);
                        entity.Fail(!somatorioICMSSTTrue.Equals(item.Total.ICMSTotal.SomatorioICMSST), SomatorioICMSSTInvalido);
                        #endregion

                        #region SomatorioProdutos
                        double? somatorioProdutosTrue = item.Detalhes.Sum(e => e.Produto.ValorBruto);
                        item.Total.ICMSTotal.SomatorioProdutos = Arredondar(item.Total.ICMSTotal.SomatorioProdutos, 2);
                        somatorioProdutosTrue = Arredondar(somatorioProdutosTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioProdutos.ToString()), SomatorioProdutosRequerido);
                        entity.Fail(!(somatorioProdutosTrue.Value == item.Total.ICMSTotal.SomatorioProdutos),
                            new Error("Informe o somatório do valor bruto dos produtos. somatorioProdutosTrue.Value:" + somatorioProdutosTrue.Value + " item.Total.ICMSTotal.SomatorioProdutos:" + item.Total.ICMSTotal.SomatorioProdutos, "Item.Total.ICMSTotal.SomatorioProdutos"));
                        #endregion

                        #region ValorFrete
                        double? valorFreteTrue = Math.Round(item.Detalhes.Sum(e => e.Produto.ValorFrete.HasValue ? e.Produto.ValorFrete.Value : 0), 2);
                        item.Total.ICMSTotal.ValorFrete = Arredondar(item.Total.ICMSTotal.ValorFrete, 2);
                        valorFreteTrue = Arredondar(valorFreteTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.ValorFrete.ToString()), ValorFreteSomatorioRequerido);
                        entity.Fail(!valorFreteTrue.Equals(item.Total.ICMSTotal.ValorFrete), ValorFreteSomatorioInvalido);
                        #endregion

                        #region ValorSeguro
                        double? valorSeguroTrue = item.Detalhes.Sum(e => e.Produto.ValorSeguro.HasValue ? e.Produto.ValorSeguro.Value : 0);
                        item.Total.ICMSTotal.ValorSeguro = Arredondar(item.Total.ICMSTotal.ValorSeguro, 2);
                        valorSeguroTrue = Arredondar(valorSeguroTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.ValorSeguro.ToString()), ValorSeguroRequerido);
                        entity.Fail(!valorSeguroTrue.Equals(item.Total.ICMSTotal.ValorSeguro), ValorSeguroSumInvalido);
                        #endregion

                        #region SomatorioDesconto
                        double? somatorioDesconto = item.Detalhes.Sum(e => e.Produto.ValorDesconto.HasValue ? e.Produto.ValorDesconto.Value : 0);
                        item.Total.ICMSTotal.SomatorioDesconto = Arredondar(item.Total.ICMSTotal.SomatorioDesconto, 2);
                        somatorioDesconto = Arredondar(somatorioDesconto, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioDesconto.ToString()), SomatorioDescontoRequerido);
                        entity.Fail(!somatorioDesconto.Equals(item.Total.ICMSTotal.SomatorioDesconto), SomatorioDescontoInvalido);
                        #endregion

                        #region SomatorioII
                        double? somatorioIITrue = item.Detalhes.Sum(e => e.Imposto.II != null ? e.Imposto.II.ValorII : 0);
                        item.Total.ICMSTotal.SomatorioII = Arredondar(item.Total.ICMSTotal.SomatorioII, 2);
                        somatorioIITrue = Arredondar(somatorioIITrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioII.ToString()), SomatorioIIRequerido);
                        entity.Fail(!somatorioIITrue.Equals(item.Total.ICMSTotal.SomatorioII), SomatorioIIInvalido);
                        #endregion SomatorioII

                        #region SomatorioIPI
                        double somatorioIPITrue = item.Detalhes.Sum(e => e.Imposto.IPI != null ? e.Imposto.IPI.ValorIPI : 0);
                        item.Total.ICMSTotal.SomatorioIPI = Arredondar(item.Total.ICMSTotal.SomatorioIPI, 2);
                        somatorioIPITrue = Arredondar(somatorioIPITrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioIPI.ToString()), SomatorioIPIRequerido);
                        entity.Fail(!somatorioIPITrue.Equals(item.Total.ICMSTotal.SomatorioIPI), SomatorioIPIInvalido);
                        #endregion SomatorioIPI

                        #region SomatorioPIS
                        double somatorioPISTrue = item.Detalhes.Sum(e => e.Imposto.PIS != null ? e.Imposto.PIS.ValorPIS : 0);
                        item.Total.ICMSTotal.SomatorioPis = Arredondar(item.Total.ICMSTotal.SomatorioPis, 2);
                        somatorioPISTrue = Arredondar(somatorioPISTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioPis.ToString()), SomatorioPisRequerido);
                        entity.Fail(!somatorioPISTrue.Equals(item.Total.ICMSTotal.SomatorioPis), SomatorioPisInvalido);
                        #endregion SomatorioPIS

                        #region SomatorioCofins
                        double somatorioCofinsTrue = item.Detalhes.Sum(e => e.Imposto.COFINS != null ? e.Imposto.COFINS.ValorCOFINS : 0);
                        item.Total.ICMSTotal.SomatorioCofins = Arredondar(item.Total.ICMSTotal.SomatorioCofins, 2);
                        somatorioCofinsTrue = Arredondar(somatorioCofinsTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioCofins.ToString()), SomatorioCofinsRequerido);
                        entity.Fail(!somatorioCofinsTrue.Equals(item.Total.ICMSTotal.SomatorioCofins), SomatorioCofinsInvalido);
                        #endregion SomatorioCofins

                        #region SomatorioOutro
                        double somatorioOutroTrue = item.Detalhes.Sum(e => e.Produto.ValorOutrasDespesas.HasValue ? e.Produto.ValorOutrasDespesas.Value : 0);
                        item.Total.ICMSTotal.SomatorioOutro = Arredondar(item.Total.ICMSTotal.SomatorioOutro, 2);
                        somatorioOutroTrue = Arredondar(somatorioOutroTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioOutro.ToString()), SomatorioOutroRequerido);
                        entity.Fail(!somatorioOutroTrue.Equals(item.Total.ICMSTotal.SomatorioOutro), SomatorioOutroInvalido);
                        #endregion SomatorioOutro

                        #region ValorTotalNF
                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.ValorTotalNF.ToString()), ValorTotalNFRequerido);
                        #endregion ValorTotalNF

                    }
                }

                #endregion Validação da classe Totais
            }

            base.ValidaModel(entity);
        }

        public static Error NotaIdRequerido = new Error("O ID da nota é um dado obrigatório.", "Item.NotaId");
        public static Error VersaoRequerida = new Error("A versão da nota é um dado obrigatório.", "Item.Versao");

        #region ERRORs da classe Identificador

        public static Error IdentificadorNulo = new Error("Os dados de identificação são obrigatórios.", "Item.Identificador");
        public static Error CodigoUFInvalido = new Error("O código da UF é inválido.", "Item.Identificador.CodigoUF");
        public static Error CodigoNFRequerido = new Error("O código da Nota Fiscal é obrigatório.", "Item.Identificador.CodigoNF");
        public static Error NaturezaOperacaoRequerido = new Error("A descrição de Natureza da Operação é obrigatória.", "Item.Identificador.NaturezaOperacao");
        public static Error FormaPagamentoInvalida = new Error("Forma de pagamento inválida.", "Item.Identificador.FormaPagamento");
        public static Error SerieRequerida = new Error("Série é um dado obrigatório.", "Item.Identificador.Serie");
        public static Error SerieNormalInvalida = new Error("Série inválida para a modalidade 1 (Emissão Normal).", "Item.Identificador.Serie");
        public static Error SerieContingenciaInvalida = new Error("Série inválida para a modalidade 3 (Contingência SCAN).", "Item.Identificador.Serie");
        public static Error NumeroDocumentoRequerido = new Error("O número do documento é obrigatório.", "Item.Identificador.NumeroDocumentoFiscal");
        public static Error TipoNotaInvalido = new Error("O tipo da nota é inválido.", "Item.Identificador.TipoDocumentoFiscal");
        public static Error TipoDestinoInvalido = new Error("O tipo da nota é inválido.", "Item.Identificador.TipoDocumentoFiscal");
        public static Error CodigoIBGEInvalido = new Error("O código do município é inválido.", "Item.Identificador.CodigoMunicipio");
        public static Error TipoImpressaoDanfeInvalido = new Error("Tipo de impressão da DANFE inválido.", "Item.Identificador.ImpressaoDANFE");
        public static Error FormaEmissaoInvalida = new Error("Tipo de modalidade de emissão inválido.", "Item.Identificador.FormaEmissao");
        public static Error AmbienteInvalido = new Error("Ambiente inválido para transmissão de notas.", "Item.Identificador.Ambiente");
        public static Error FinalidadeInvalida = new Error("Ambiente inválido para transmissão de notas.", "Item.Identificador.FinalidadeEmissaoNFe");
        public static Error ConsumidorFinalInvalido = new Error("Informação de consumidor final inválida.", "Item.Identificador.ConsumidorFinal");

        #endregion

        #region ERRORs da classe Emitente

        public static Error EmitenteNulo = new Error("Os dados do emitente são obrigatórios.", "Item.Emitente");
        public static Error CpfouCnpjRequerido = new Error("Informe o CPF ou CNPJ do emitente.", "Item.Emitente.Cnpj");
        public static Error CpfInvalido = new Error("CPF do emitente inválido.", "Item.Emitente.Cpf");
        public static Error CnpjInvalido = new Error("CNPJ do emitente inválido.", "Item.Emitente.Cnpj");
        public static Error NomeRequerido = new Error("Nome do emitente é um dado obrigatório.", "Item.Emitente.Nome");
        public static Error LogradouroRequerido = new Error("Logradouro do emitente é um dado obrigatório.", "Item.Emitente.Endereco.Logradouro");
        public static Error NumeroRequerido = new Error("Número do emitente é um dado obrigatório.", "Item.Emitente.Endereco.Numero");
        public static Error BairroRequerido = new Error("Bairro do emitente é um dado obrigatório.", "Item.Emitente.Endereco.Bairro");
        public static Error EnderecoCodigoIBGEInvalido = new Error("Código de município do emitente inválido.", "Item.Emitente.Endereco.CodigoMunicipio");
        public static Error CodigoIBGEDivergente = new Error("Código de município do emitente difere do informado na identificação.", "Item.Emitente.Endereco.CodigoMunicipio");
        public static Error MunicipioRequerido = new Error("Município do emitente é um dado obrigatório.", "Item.Emitente.Endereco.Municipio");
        public static Error IERequerida = new Error("Inscrição Estadual do emitente é um dado obrigatório.", "Item.Emitente.InscricaoEstadual");
        public static Error IEDigitoVerificador = new Error("IE Emitente - Digito verificador inválido (para este estado).", "Item.Emitente.InscricaoEstadual");
        public static Error IEQuantidadeDeDigitos = new Error("IE Emitente - Quantidade de dígitos inválido (para este estado).", "Item.Emitente.InscricaoEstadual");
        public static Error IEInvalida = new Error("IE Emitente - Inscrição Estadual inválida (para este estado).", "Item.Emitente.InscricaoEstadual");
        public static Error UFInvalida = new Error("Sigla da UF do emitente inválida.", "item.Emitente.Endereco.UF");
        public static Error UFRequerida = new Error("UF do emitente é um dado obrigatório.", "item.Emitente.Endereco.UF");
        public static Error CepRequerido = new Error("CEP do emitente é um dado obrigatório.", "item.Emitente.Endereco.Cep");
        public static Error CepInvalido = new Error("CEP do emitente inválido.", "item.Emitente.Endereco.Cep");

        #endregion

        #region ERRORs da classe Destinatario

        public static Error DestinatarioNulo = new Error("Os dados do destinatário são obrigatórios.", "Item.Destinatario");
        public static Error CpfouCnpjDestinatarioRequerido = new Error("Informe o CPF ou CNPJ do destinatário.", "Item.Destinatario.Cnpj");
        public static Error CpfDestinatarioInvalido = new Error("CPF do destinatário inválido.", "Item.Destinatario.Cpf");
        public static Error CnpjDestinatarioInvalido = new Error("CNPJ do destinatário inválido.", "Item.Destinatario.Cnpj");
        public static Error NomeDestinatarioRequerido = new Error("Nome do destinatário é um dado obrigatório.", "Item.Destinatario.Nome");
        public static Error LogradouroDestinatarioRequerido = new Error("Logradouro do destinatário é um dado obrigatório.", "Item.Destinatario.Endereco.Logradouro");
        public static Error NumeroDestinatarioRequerido = new Error("Número do destinatário é um dado obrigatório.", "Item.Destinatario.Endereco.Numero");
        public static Error BairroDestinatarioRequerido = new Error("Bairro do destinatário é um dado obrigatório.", "Item.Destinatario.Endereco.Bairro");
        public static Error EnderecoCodigoIBGEDestinatarioInvalido = new Error("Código de município do destinatário inválido.", "Item.Destinatario.Endereco.CodigoMunicipio");
        public static Error MunicipioDestinatarioRequerido = new Error("Município do destinatário é um dado obrigatório.", "Item.Destinatario.Endereco.Municipio");
        public static Error IEDigitoVerificadorDestinatario = new Error("IE Destinatário - Digito verificador inválido (para este estado).", "Item.Destinatario.InscricaoEstadual");
        public static Error IEQuantidadeDeDigitosDestinatario = new Error("IE Destinatário - Quantidade de dígitos inválido (para este estado).", "Item.Destinatario.InscricaoEstadual");
        public static Error IEInvalidaDestinatario = new Error("IE Destinatário - Inscrição Estadual inválida (para este estado).", "Item.Destinatario.InscricaoEstadual");
        public static Error UFInvalidaDestinatario = new Error("Sigla da UF do destinatário inválida.", "item.Destinatario.Endereco.UF");
        public static Error UFRequeridaDestinatario = new Error("UF do destinatário é um dado obrigatório.", "item.Destinatario.Endereco.UF");
        public static Error CepRequeridoDestinatario = new Error("CEP do destinatário é um dado obrigatório.", "item.Destinatario.Endereco.Cep");
        public static Error CepInvalidoDestinatario = new Error("CEP do destinatário inválido.", "item.Destinatario.Endereco.Cep");

        #endregion
        
        #region ERRORs da classe Total

        public static Error TotalNulo = new Error("Os dados de totais são obrigatórios.", "Item.Total");
        public static Error ICMSTotalNulo = new Error("Os dados de ICMSTotal são obrigatórios", "Item.Total.ICMSTotal");

        public static Error SomatorioBCRequerido = new Error("Informe o somatório da BC do ICMS.", "Item.Total.ICMSTotal.SomatorioBC");
        public static Error SomatorioBCInvalido = new Error("O somatório da BC do ICMS não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioBC");

        public static Error SomatorioICMSRequerido = new Error("Informe o somatório da ICMS.", "Item.Total.ICMSTotal.SomatorioICMS");
        public static Error SomatorioICMSInvalido = new Error("O somatório da ICMS não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioICMS");

        public static Error SomatorioBCSTRequerido = new Error("Informe o somatório da BCST.", "Item.Total.ICMSTotal.SomatorioBCST");
        public static Error SomatorioBCSTInvalido = new Error("O somatório da BCST não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioBCST");

        public static Error SomatorioICMSSTRequerido = new Error("Informe o somatório da ICMSST.", "Item.Total.ICMSTotal.SomatorioICMSST");
        public static Error SomatorioICMSSTInvalido = new Error("O somatório da ICMSST não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioICMSST");

        public static Error SomatorioProdutosRequerido = new Error("Informe o somatório do valor bruto dos produtos.", "Item.Total.ICMSTotal.SomatorioProdutos");
        public static Error SomatorioProdutosInvalido = new Error("O somatório do valor bruto dos produtos não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioProdutos");

        public static Error ValorFreteSomatorioRequerido = new Error("Informe o somatório do valor de frete.", "Item.Total.ICMSTotal.ValorFrete");
        public static Error ValorFreteSomatorioInvalido = new Error("O somatório do valor de frete não confere com os valores informados.", "Item.Total.ICMSTotal.ValorFrete");

        public static Error ValorSeguroRequerido = new Error("Informe o somatório do valor do seguro.", "Item.Total.ICMSTotal.ValorSeguro");
        public static Error ValorSeguroSumInvalido = new Error("O somatório do valor do seguro não confere com os valores informados.", "Item.Total.ICMSTotal.ValorSeguro");

        public static Error SomatorioDescontoRequerido = new Error("Informe o somatório do valor de desconto.", "Item.Total.ICMSTotal.SomatorioDesconto");
        public static Error SomatorioDescontoInvalido = new Error("O somatório do valor de desconto não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioDesconto");

        public static Error SomatorioIIRequerido = new Error("Informe o somatório do valor de II.", "Item.Total.ICMSTotal.SomatorioII");
        public static Error SomatorioIIInvalido = new Error("O somatório do valor de II não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioII");

        public static Error SomatorioIPIRequerido = new Error("Informe o somatório do valor de IPI.", "Item.Total.ICMSTotal.SomatorioIPI");
        public static Error SomatorioIPIInvalido = new Error("O somatório do valor de IPI não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioIPI");

        public static Error SomatorioPisRequerido = new Error("Informe o somatório do valor de PIS.", "Item.Total.ICMSTotal.SomatorioPis");
        public static Error SomatorioPisInvalido = new Error("O somatório do valor de PIS não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioPis");

        public static Error SomatorioCofinsRequerido = new Error("Informe o somatório do valor de COFINS.", "Item.Total.ICMSTotal.SomatorioCofins");
        public static Error SomatorioCofinsInvalido = new Error("O somatório do valor de COFINS não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioCofins");

        public static Error SomatorioOutroRequerido = new Error("Informe o somatório de valor de Outros.", "Item.Total.ICMSTotal.SomatorioOutro");
        public static Error SomatorioOutroInvalido = new Error("O somatório do valor de Outros não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioOutro");

        public static Error ValorTotalNFRequerido = new Error("Informe o valor total da NFE.", "Item.Total.ICMSTotal.ValorTotalNF");

        #endregion ERRORs da classe Total

    }
}