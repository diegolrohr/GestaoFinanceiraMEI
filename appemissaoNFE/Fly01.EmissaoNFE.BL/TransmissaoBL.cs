using Fly01.Core.API;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Linq;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;

namespace Fly01.EmissaoNFE.BL
{
    public class TransmissaoBL : PlataformaBaseBL<TransmissaoVM>
    {
        protected CfopBL CfopBL;
        protected ChaveBL ChaveBL;
        protected CidadeBL CidadeBL;
        protected EmpresaBL EmpresaBL;
        protected EntidadeBL EntidadeBL;
        protected EstadoBL EstadoBL;
        protected NFeBL NFeBL;
        private static string msgError;

        public TransmissaoBL(AppDataContextBase context, CfopBL cfopBL, ChaveBL chaveBL, CidadeBL cidadeBL, EmpresaBL empresaBL, EntidadeBL entidadeBL, EstadoBL estadoBL, NFeBL nfeBL) : base(context)
        {
            CfopBL = cfopBL;
            ChaveBL = chaveBL;
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

        public NFeVM ConvertToNFe(ItemTransmissaoVM item)
        {
            var nota = new NFeVM();
            nota.InfoNFe = new InfoNFe();

            item.NotaId = ChaveBL.GeraChave(
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
            nota.InfoNFe.Identificador.CodigoNF = item.Identificador.CodigoNF.PadLeft(8, '0');
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
                entity.Fail(string.IsNullOrEmpty(item.Versao), new Error("A versão da nota é um dado obrigatório.", "Item.Versao"));

                #region Validações da classe Identificador

                if (item.Identificador == null)
                    entity.Fail(true, new Error("Os dados de identificação são obrigatórios.", "Item.Identificador"));
                else
                {
                    entity.Fail(!EstadoBL.All.Any(e => e.CodigoIbge == item.Identificador.CodigoUF.ToString()),
                        new Error("O código da UF é inválido.", "Item.Identificador.CodigoUF"));
                    entity.Fail(string.IsNullOrEmpty(item.Identificador.CodigoNF.ToString()),
                        new Error("O código da Nota Fiscal é obrigatório.", "Item.Identificador.CodigoNF"));
                    entity.Fail(string.IsNullOrEmpty(item.Identificador.NaturezaOperacao),
                        new Error("A descrição de Natureza da Operação é obrigatória.", "Item.Identificador.NaturezaOperacao"));
                    entity.Fail((item.Identificador.FormaPagamento < 0 || (int)item.Identificador.FormaPagamento > 2),
                        new Error("Forma de pagamento inválida.", "Item.Identificador.FormaPagamento"));
                    entity.Fail(string.IsNullOrEmpty(item.Identificador.Serie.ToString()),
                        new Error("Série é um dado obrigatório.", "Item.Identificador.Serie"));
                    entity.Fail(item.Identificador.Serie > 889 && item.Identificador.FormaEmissao == TipoModalidade.Normal,
                        new Error("Série inválida para a modalidade 1 (Emissão Normal).", "Item.Identificador.Serie"));
                    entity.Fail(item.Identificador.Serie < 900 && item.Identificador.FormaEmissao == TipoModalidade.SCAN,
                        new Error("Série inválida para a modalidade 3 (Contingência SCAN).", "Item.Identificador.Serie"));
                    entity.Fail(string.IsNullOrEmpty(item.Identificador.NumeroDocumentoFiscal.ToString()),
                        new Error("O número do documento é obrigatório.", "Item.Identificador.NumeroDocumentoFiscal"));
                    entity.Fail((item.Identificador.TipoDocumentoFiscal < 0 || (int)item.Identificador.TipoDocumentoFiscal > 1),
                        new Error("O tipo da nota é inválido.", "Item.Identificador.TipoDocumentoFiscal"));
                    entity.Fail(((int)item.Identificador.DestinoOperacao < 1 || (int)item.Identificador.DestinoOperacao > 3),
                        new Error("O tipo da nota é inválido.", "Item.Identificador.TipoDocumentoFiscal"));
                    entity.Fail(!CidadeBL.All.Any(e => e.CodigoIbge == item.Identificador.CodigoMunicipio),
                        new Error("O código do município é inválido.", "Item.Identificador.CodigoMunicipio"));
                    entity.Fail((item.Identificador.ImpressaoDANFE < 0 || (int)item.Identificador.ImpressaoDANFE > 5),
                        new Error("Tipo de impressão da DANFE inválido.", "Item.Identificador.ImpressaoDANFE"));
                    entity.Fail(((int)item.Identificador.FormaEmissao < 1 || (int)item.Identificador.FormaEmissao > 7) && (int)item.Identificador.FormaEmissao != 9,
                        new Error("Tipo de modalidade de emissão inválido.", "Item.Identificador.FormaEmissao"));
                    entity.Fail(((int)item.Identificador.Ambiente < 1 || (int)item.Identificador.Ambiente > 2),
                        new Error("Ambiente inválido para transmissão de notas.", "Item.Identificador.Ambiente"));
                    entity.Fail(((int)item.Identificador.FinalidadeEmissaoNFe < 1 || (int)item.Identificador.FinalidadeEmissaoNFe > 4),
                        new Error("Ambiente inválido para transmissão de notas.", "Item.Identificador.FinalidadeEmissaoNFe"));
                    entity.Fail(item.Identificador.ConsumidorFinal != 0 && item.Identificador.ConsumidorFinal != 1,
                        new Error("Informação de consumidor final inválida.", "Item.Identificador.ConsumidorFinal"));
                }

                #endregion

                #region Validações da classe Emitente

                if (item.Emitente == null)
                    entity.Fail(true, new Error("Os dados do emitente são obrigatórios.", "Item.Emitente"));
                else
                {
                    entity.Fail(item.Emitente.Cnpj == null && item.Emitente.Cpf == null,
                        new Error("Informe o CPF ou CNPJ do emitente.", "Item.Emitente.Cnpj"));
                    entity.Fail(item.Emitente.Cpf != null && (!EmpresaBL.ValidaCPF(item.Emitente.Cpf) || item.Emitente.Cnpj.Length != 11),
                        new Error("CPF do emitente inválido.", "Item.Emitente.Cpf"));
                    entity.Fail(item.Emitente.Cnpj != null && (!EmpresaBL.ValidaCNPJ(item.Emitente.Cnpj) || item.Emitente.Cnpj.Length != 14),
                        new Error("CNPJ do emitente inválido.", "Item.Emitente.Cnpj"));
                    entity.Fail(string.IsNullOrEmpty(item.Emitente.Nome),
                        new Error("Nome do emitente é um dado obrigatório.", "Item.Emitente.Nome"));
                    entity.Fail(string.IsNullOrEmpty(item.Emitente.Endereco.Logradouro),
                        new Error("Logradouro do emitente é um dado obrigatório.", "Item.Emitente.Endereco.Logradouro"));
                    entity.Fail(string.IsNullOrEmpty(item.Emitente.Endereco.Numero),
                        new Error("Número do emitente é um dado obrigatório.", "Item.Emitente.Endereco.Numero"));
                    entity.Fail(string.IsNullOrEmpty(item.Emitente.Endereco.Bairro),
                        new Error("Bairro do emitente é um dado obrigatório.", "Item.Emitente.Endereco.Bairro"));
                    entity.Fail(!CidadeBL.All.Any(e => e.CodigoIbge == item.Emitente.Endereco.CodigoMunicipio),
                        new Error("Código de município do emitente inválido.", "Item.Emitente.Endereco.CodigoMunicipio"));
                    entity.Fail(item.Emitente.Endereco.CodigoMunicipio != item.Identificador.CodigoMunicipio,
                        new Error("Código de município do emitente difere do informado na identificação.", "Item.Emitente.Endereco.CodigoMunicipio"));
                    entity.Fail(string.IsNullOrEmpty(item.Emitente.Endereco.Municipio),
                        new Error("Município do emitente é um dado obrigatório.", "Item.Emitente.Endereco.Municipio"));
                    if (item.Emitente.InscricaoEstadual != null)
                    {
                        if (!EmpresaBL.ValidaIE(item.Emitente.Endereco.UF, item.Emitente.InscricaoEstadual, out msgError))
                        {
                            switch (msgError)
                            {
                                case "1":
                                    entity.Fail(true, new Error("IE Emitente - Digito verificador inválido (para este estado).", "Item.Emitente.InscricaoEstadual"));
                                    break;
                                case "2":
                                    entity.Fail(true, new Error("IE Emitente - Quantidade de dígitos inválido (para este estado).", "Item.Emitente.InscricaoEstadual"));
                                    break;
                                case "3":
                                    entity.Fail(true, new Error("IE Emitente - Inscrição Estadual inválida (para este estado).", "Item.Emitente.InscricaoEstadual"));
                                    break;
                                case "4":
                                    entity.Fail(true, new Error("UF do emitente inválida.", "item.Emitente.Endereco.UF"));
                                    break;
                                case "5":
                                    entity.Fail(true, new Error("UF do emitente é um dado obrigatório.", "item.Emitente.Endereco.UF"));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                        entity.Fail(true, new Error("Inscrição Estadual do emitente é um dado obrigatório.", "Item.Emitente.InscricaoEstadual"));

                    entity.Fail(item.Emitente.Endereco.Cep == null,
                        new Error("CEP do emitente é um dado obrigatório.", "item.Emitente.Endereco.Cep"));
                    entity.Fail(item.Emitente.Endereco.Cep != null && !EmpresaBL.ValidaCEP(item.Emitente.Endereco.Cep),
                        new Error("CEP do emitente inválido.", "item.Emitente.Endereco.Cep"));
                }

                #endregion

                #region Validações da classe Destinatario

                if (item.Destinatario == null)
                    entity.Fail(true, new Error("Os dados do destinatário são obrigatórios.", "Item.Destinatario"));
                else
                {
                    entity.Fail(item.Destinatario.Cnpj == null && item.Destinatario.Cpf == null,
                        new Error("Informe o CPF ou CNPJ do destinatário.", "Item.Destinatario.Cnpj"));
                    entity.Fail(item.Destinatario.Cpf != null && (!EmpresaBL.ValidaCPF(item.Destinatario.Cpf) || item.Destinatario.Cpf.Length != 11),
                        new Error("CPF do destinatário inválido.", "Item.Destinatario.Cpf"));
                    entity.Fail(item.Destinatario.Cnpj != null && (!EmpresaBL.ValidaCNPJ(item.Destinatario.Cnpj) || item.Destinatario.Cnpj.Length != 14),
                        new Error("CNPJ do destinatário inválido.", "Item.Destinatario.Cnpj"));
                    entity.Fail(string.IsNullOrEmpty(item.Destinatario.Nome),
                        new Error("Nome do destinatário é um dado obrigatório.", "Item.Destinatario.Nome"));
                    entity.Fail(string.IsNullOrEmpty(item.Destinatario.Endereco.Logradouro),
                        new Error("Logradouro do destinatário é um dado obrigatório.", "Item.Destinatario.Endereco.Logradouro"));
                    entity.Fail(string.IsNullOrEmpty(item.Destinatario.Endereco.Numero),
                        new Error("Número do destinatário é um dado obrigatório.", "Item.Destinatario.Endereco.Numero"));
                    entity.Fail(string.IsNullOrEmpty(item.Destinatario.Endereco.Bairro),
                        new Error("Bairro do destinatário é um dado obrigatório.", "Item.Destinatario.Endereco.Bairro"));
                    entity.Fail(item.Destinatario.Endereco.CodigoMunicipio != null && !CidadeBL.All.Any(e => e.CodigoIbge == item.Destinatario.Endereco.CodigoMunicipio),
                        new Error("Código de município do destinatário inválido.", "Item.Destinatario.Endereco.CodigoMunicipio"));
                    entity.Fail(string.IsNullOrEmpty(item.Destinatario.Endereco.Municipio),
                        new Error("Município do destinatário é um dado obrigatório.", "Item.Destinatario.Endereco.Municipio"));
                    if (item.Destinatario.InscricaoEstadual != null && item.Destinatario.IndInscricaoEstadual == IndInscricaoEstadual.ContribuinteICMS)
                    {
                        if (!EmpresaBL.ValidaIE(item.Destinatario.Endereco.UF, item.Destinatario.InscricaoEstadual, out msgError))
                        {
                            switch (msgError)
                            {
                                case "1":
                                    entity.Fail(true, new Error("IE Destinatário - Digito verificador inválido (para este estado).", "Item.Destinatario.InscricaoEstadual"));
                                    break;
                                case "2":
                                    entity.Fail(true, new Error("IE Destinatário - Quantidade de dígitos inválido (para este estado).", "Item.Destinatario.InscricaoEstadual"));
                                    break;
                                case "3":
                                    entity.Fail(true, new Error("IE Destinatário - Inscrição Estadual inválida (para este estado).", "Item.Destinatario.InscricaoEstadual"));
                                    break;
                                case "4":
                                    entity.Fail(true, new Error("UF do destinatário inválida.", "item.Destinatario.Endereco.UF"));
                                    break;
                                case "5":
                                    entity.Fail(true, new Error("UF do destinatário é um dado obrigatório.", "item.Destinatario.Endereco.UF"));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    entity.Fail(item.Destinatario.Endereco.Cep != null && !EmpresaBL.ValidaCEP(item.Destinatario.Endereco.Cep),
                        new Error("CEP do destinatário inválido.", "item.Destinatario.Endereco.Cep"));
                }

                #endregion

                #region Validações da classe Transporte

                var modFrete = EnumHelper.GetDataEnumValues(typeof(ModalidadeFrete));
                entity.Fail(!modFrete.Any(x => x.Value == ((int)item.Transporte.ModalidadeFrete).ToString()),
                    new Error("Modalidade de frete inválida", "Item.Transporte.ModalidadeFrete"));

                if (item.Transporte.Transportadora != null)
                {
                    entity.Fail(string.IsNullOrEmpty(item.Transporte.Transportadora.RazaoSocial),
                        new Error("Razão Social da transportadora é um dado obrigatório", "Item.Transporte.Transportadora.RazaoSocial"));
                    entity.Fail(string.IsNullOrEmpty(item.Transporte.Transportadora.CNPJ) && string.IsNullOrEmpty(item.Transporte.Transportadora.CPF),
                        new Error("Informe CPF ou CNPJ da transportadora", "Item.Transporte.Transportadora.CNPJ"));
                    entity.Fail(!string.IsNullOrEmpty(item.Transporte.Transportadora.CNPJ) && !EmpresaBL.ValidaCNPJ(item.Transporte.Transportadora.CNPJ),
                        new Error("CNPJ da transportadora é inválido", "Item.Transporte.Transportadora.CNPJ"));
                    entity.Fail(!string.IsNullOrEmpty(item.Transporte.Transportadora.CPF) && !EmpresaBL.ValidaCPF(item.Transporte.Transportadora.CPF),
                        new Error("CPF da transportadora é inválido", "Item.Transporte.Transportadora.CPF"));
                    entity.Fail(string.IsNullOrEmpty(item.Transporte.Transportadora.Endereco),
                        new Error("Endereço da transportadora é obrigatório", "Item.Transporte.Transportadora.Endereco"));
                    entity.Fail(string.IsNullOrEmpty(item.Transporte.Transportadora.Municipio),
                        new Error("Município da transportadora é um dado obrigatório", "Item.Transporte.Transportadora.Municipio"));

                    if (item.Transporte.Transportadora.IE != null)
                    {
                        if (!EmpresaBL.ValidaIE(item.Transporte.Transportadora.UF, item.Transporte.Transportadora.IE, out msgError))
                        {
                            switch (msgError)
                            {
                                case "1":
                                    entity.Fail(true, new Error("IE Transportadora - Digito verificador inválido (para este estado)", "Item.Transporte.Transportadora.IE"));
                                    break;
                                case "2":
                                    entity.Fail(true, new Error("IE Transportadora - Quantidade de dígitos inválido (para este estado)", "Item.Transporte.Transportadora.IE"));
                                    break;
                                case "3":
                                    entity.Fail(true, new Error("IE Transportadora - Inscrição Estadual inválida (para este estado)", "Item.Transporte.Transportadora.IE"));
                                    break;
                                case "4":
                                    entity.Fail(true, new Error("UF da transportadora é inválida.", "Item.Transporte.Transportadora.UF"));
                                    break;
                                case "5":
                                    entity.Fail(true, new Error("UF da transportadora é um campo obrigatório.", "Item.Transporte.Transportadora.UF"));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        entity.Fail(true, new Error("IE Transportadora - Este dado é obrigatório"));
                    }
                }

                if (item.Transporte.Veiculo != null)
                {
                    entity.Fail(string.IsNullOrEmpty(item.Transporte.Veiculo.Placa) || item.Transporte.Veiculo.Placa.Length != 7,
                        new Error("Placa do veículo de transporte inválida", "Item.Transporte.Veiculo.Placa"));
                    entity.Fail(string.IsNullOrEmpty(item.Transporte.Veiculo.UF) || !EstadoBL.All.Any(x => x.Sigla == item.Transporte.Veiculo.UF),
                        new Error("Estado do veículo de transporte inválido", "Item.Transporte.Veiculo.UF"));
                    entity.Fail(string.IsNullOrEmpty(item.Transporte.Veiculo.RNTC),
                        new Error("Código RNTC é um dado obrigatório", "Item.Transporte.Veiculo.RNTC"));
                }

                if (item.Transporte.Volume != null)
                {
                    entity.Fail(item.Transporte.Volume.Quantidade <= 0,
                        new Error("Quantidade de volumes inválida", "Item.Transporte.Volume.Quantidade"));
                    entity.Fail(string.IsNullOrEmpty(item.Transporte.Volume.Especie),
                        new Error("Espécie de volume é um dado obrigatório", "Item.Transporte.Volume.Especie"));
                    entity.Fail(string.IsNullOrEmpty(item.Transporte.Volume.Marca),
                        new Error("Marca do volume é um dado obrigatório", "Item.Transporte.Volume.marca"));
                    entity.Fail(string.IsNullOrEmpty(item.Transporte.Volume.Numeracao),
                        new Error("Numeração do volume é um dado obrigatório", "Item.Transporte.Volume.Numeracao"));
                    entity.Fail(item.Transporte.Volume.PesoLiquido <= 0,
                        new Error("Peso líquido inválido", "Item.Transporte.Volume.PesoLiquido"));
                    entity.Fail(item.Transporte.Volume.PesoBruto <= 0,
                        new Error("Peso bruto inválido", "Item.Transporte.Volume.PesoBruto"));
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
                        var totalAprox = Math.Round((detalhe.Imposto.COFINS != null ? detalhe.Imposto.COFINS.ValorCOFINS : 0) +
                                         (detalhe.Imposto.ICMS.ValorICMS.HasValue ? detalhe.Imposto.ICMS.ValorICMS.Value : 0) +
                                         (detalhe.Imposto.II != null ? detalhe.Imposto.II.ValorII : 0) +
                                         (detalhe.Imposto.IPI != null ? detalhe.Imposto.IPI.ValorIPI : 0) +
                                         (detalhe.Imposto.PIS != null ? detalhe.Imposto.PIS.ValorPIS : 0) +
                                         (detalhe.Imposto.PISST != null ? detalhe.Imposto.PISST.ValorPISST : 0), 2);

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

                                    if (item.Versao == "4.0")
                                    {
                                        entity.Fail(!detalhe.Imposto.ICMS.BaseFCPST.HasValue,
                                            new Error("Valor da Base de Cálculo do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.vBCFCPST"));
                                        entity.Fail(!detalhe.Imposto.ICMS.AliquotaFCPST.HasValue,
                                            new Error("Percentual do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.pFCPST"));
                                        entity.Fail(!detalhe.Imposto.ICMS.ValorFCPST.HasValue,
                                            new Error("Valor do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.vFCPST"));
                                    }
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

                                    if (item.Versao == "4.0")
                                    {
                                        entity.Fail(!detalhe.Imposto.ICMS.BaseFCPST.HasValue,
                                        new Error("Valor da Base de Cálculo do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.vBCFCPST"));
                                        entity.Fail(!detalhe.Imposto.ICMS.AliquotaFCPST.HasValue,
                                            new Error("Percentual do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.pFCPST"));
                                        entity.Fail(!detalhe.Imposto.ICMS.ValorFCPST.HasValue,
                                            new Error("Valor do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.vFCPST"));
                                    }
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

                                    if (item.Versao == "4.0")
                                    {
                                        entity.Fail(!detalhe.Imposto.ICMS.BaseFCPST.HasValue,
                                        new Error("Valor da Base de Cálculo do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.vBCFCPST"));
                                        entity.Fail(!detalhe.Imposto.ICMS.AliquotaFCPST.HasValue,
                                            new Error("Percentual do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.pFCPST"));
                                        entity.Fail(!detalhe.Imposto.ICMS.ValorFCPST.HasValue,
                                            new Error("Valor do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.vFCPST"));
                                    }
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

                                    if (item.Versao == "4.0")
                                    {
                                        entity.Fail(!detalhe.Imposto.ICMS.BaseFCPSTRetido.HasValue,
                                            new Error("Valor da Base de Cálculo do FCP retido anteriormente por ST é obrigatório para CSOSN 500. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.vBCFCPSTRet"));
                                        entity.Fail(!detalhe.Imposto.ICMS.AliquotaFCPSTRetido.HasValue,
                                            new Error("Percentual do FCP retido anteriormente por Substituição Tributária é obrigatório para CSOSN 500. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.pFCPSTRet"));
                                        entity.Fail(!detalhe.Imposto.ICMS.ValorFCPSTRetido.HasValue,
                                            new Error("Valor do FCP retido por Substituição Tributária é obrigatório para CSOSN 500. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.vFCPSTRet"));
                                    }
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
                                      (detalhe.Imposto.ICMS.ValorICMS.HasValue && detalhe.Imposto.ICMS.ValorICMS > 0 ||
                                      !string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString())))
                                    {
                                        ICMSProprio = true;
                                        entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBC.ToString()),
                                            new Error("Modalidade de determinação da base de cálculo é obrigatória para operações de ICMS próprio. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ModalidadeBC"));
                                        entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()),
                                            new Error("Modalidade de determinação da base de cálculo do ICMS ST é obrigatória para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ModalidadeBCST"));
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
                                      (detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST.HasValue && detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST > 0) ||
                                      (detalhe.Imposto.ICMS.PercentualReducaoBCST.HasValue && detalhe.Imposto.ICMS.PercentualReducaoBCST > 0) ||
                                      (detalhe.Imposto.ICMS.ValorBCST.HasValue && detalhe.Imposto.ICMS.ValorBCST > 0) ||
                                      (detalhe.Imposto.ICMS.AliquotaICMSST.HasValue && detalhe.Imposto.ICMS.AliquotaICMSST > 0) ||
                                      (detalhe.Imposto.ICMS.ValorICMSST.HasValue && detalhe.Imposto.ICMS.ValorICMSST > 0)
                                      )
                                    {
                                        entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()) && !ModalidadeST.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.ICMS.ModalidadeBCST)),
                                            new Error("Modalidade de determinação da base de cálculo inválida. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ModalidadeBCST"));
                                        entity.Fail(!detalhe.Imposto.ICMS.ValorBCST.HasValue,
                                            new Error("Valor da base de cálculo do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorBCST"));
                                        entity.Fail(!detalhe.Imposto.ICMS.AliquotaICMSST.HasValue,
                                            new Error("Alíquota da Substituição Tributária é requerida. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorBC"));
                                        entity.Fail(!detalhe.Imposto.ICMS.ValorICMSST.HasValue,
                                            new Error("Valor da Substituição Tributária é requerido. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.ValorBC"));

                                        if (item.Versao == "4.0")
                                        {
                                            entity.Fail(!detalhe.Imposto.ICMS.BaseFCPST.HasValue,
                                            new Error("Valor da Base de Cálculo do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.vBCFCPST"));
                                            entity.Fail(!detalhe.Imposto.ICMS.AliquotaFCPST.HasValue,
                                                new Error("Percentual do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.pFCPST"));
                                            entity.Fail(!detalhe.Imposto.ICMS.ValorFCPST.HasValue,
                                                new Error("Valor do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItem, "Item.Detalhes[" + (nItem - 1) + "].Imposto.ICMS.vFCPST"));
                                        }
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

                    }

                    #endregion

                    nItem++;
                }

                #region Validação da classe Totais

                if (item.Total == null)
                    entity.Fail(true, new Error("Os dados de totais são obrigatórios.", "Item.Total"));
                else
                {
                    if (item.Total.ICMSTotal == null)
                        entity.Fail(true, new Error("Os dados de ICMSTotal são obrigatórios", "Item.Total.ICMSTotal"));
                    else
                    {
                        #region SomatorioBC
                        double? somatorioBCTrue = item.Detalhes.Sum(e => e.Imposto.ICMS.ValorBC.HasValue ? e.Imposto.ICMS.ValorBC.Value : 0);
                        item.Total.ICMSTotal.SomatorioBC = Arredondar(item.Total.ICMSTotal.SomatorioBC, 2);
                        somatorioBCTrue = Arredondar(somatorioBCTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioBC.ToString()),
                            new Error("Informe o somatório da BC do ICMS.", "Item.Total.ICMSTotal.SomatorioBC"));
                        entity.Fail(!somatorioBCTrue.Equals(item.Total.ICMSTotal.SomatorioBC),
                            new Error("O somatório da BC do ICMS não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioBC"));
                        #endregion

                        #region SomatorioICMS
                        double? somatorioICMSTrue = item.Detalhes.Sum(e => e.Imposto.ICMS.ValorICMS.HasValue ? e.Imposto.ICMS.ValorICMS.Value : 0);
                        item.Total.ICMSTotal.SomatorioICMS = Arredondar(item.Total.ICMSTotal.SomatorioICMS, 2);
                        somatorioICMSTrue = Arredondar(somatorioICMSTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioICMS.ToString()),
                            new Error("Informe o somatório da ICMS.", "Item.Total.ICMSTotal.SomatorioICMS"));
                        entity.Fail(!somatorioICMSTrue.Equals(item.Total.ICMSTotal.SomatorioICMS),
                            new Error("O somatório da ICMS não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioICMS"));
                        #endregion

                        #region SomatorioBCST
                        double? somatorioBCSTTrue = item.Detalhes.Sum(e => e.Imposto.ICMS.ValorBCST.HasValue ? e.Imposto.ICMS.ValorBCST.Value : 0);
                        item.Total.ICMSTotal.SomatorioBCST = Arredondar(item.Total.ICMSTotal.SomatorioBCST, 2);
                        somatorioBCSTTrue = Arredondar(somatorioBCSTTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioBCST.ToString()),
                            new Error("Informe o somatório da BCST.", "Item.Total.ICMSTotal.SomatorioBCST"));
                        entity.Fail(!somatorioBCSTTrue.Equals(item.Total.ICMSTotal.SomatorioBCST),
                            new Error("O somatório da BCST não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioBCST"));

                        #endregion

                        #region SomatorioICMSST
                        double? somatorioICMSSTTrue = item.Detalhes.Sum(e => e.Imposto.ICMS.ValorICMSST.HasValue ? e.Imposto.ICMS.ValorICMSST.Value : 0);
                        item.Total.ICMSTotal.SomatorioICMSST = Arredondar(item.Total.ICMSTotal.SomatorioICMSST, 2);
                        somatorioICMSSTTrue = Arredondar(somatorioICMSSTTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioICMSST.ToString()),
                            new Error("Informe o somatório da ICMSST.", "Item.Total.ICMSTotal.SomatorioICMSST"));
                        entity.Fail(!somatorioICMSSTTrue.Equals(item.Total.ICMSTotal.SomatorioICMSST),
                            new Error("O somatório da ICMSST não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioICMSST"));
                        #endregion

                        #region SomatorioProdutos
                        double? somatorioProdutosTrue = item.Detalhes.Sum(e => e.Produto.ValorBruto);
                        item.Total.ICMSTotal.SomatorioProdutos = Arredondar(item.Total.ICMSTotal.SomatorioProdutos, 2);
                        somatorioProdutosTrue = Arredondar(somatorioProdutosTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioProdutos.ToString()),
                            new Error("Informe o somatório do valor bruto dos produtos.", "Item.Total.ICMSTotal.SomatorioProdutos"));
                        entity.Fail(!(somatorioProdutosTrue.Value == item.Total.ICMSTotal.SomatorioProdutos),
                            new Error("O somatório do valor bruto dos produtos não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioProdutos"));
                        #endregion

                        #region ValorFrete
                        double? valorFreteTrue = Math.Round(item.Detalhes.Sum(e => e.Produto.ValorFrete.HasValue ? e.Produto.ValorFrete.Value : 0), 2);
                        item.Total.ICMSTotal.ValorFrete = Arredondar(item.Total.ICMSTotal.ValorFrete, 2);
                        valorFreteTrue = Arredondar(valorFreteTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.ValorFrete.ToString()),
                            new Error("Informe o somatório do valor de frete.", "Item.Total.ICMSTotal.ValorFrete"));
                        entity.Fail(!valorFreteTrue.Equals(item.Total.ICMSTotal.ValorFrete),
                            new Error("O somatório do valor de frete não confere com os valores informados.", "Item.Total.ICMSTotal.ValorFrete"));
                        #endregion

                        #region ValorSeguro
                        double? valorSeguroTrue = item.Detalhes.Sum(e => e.Produto.ValorSeguro.HasValue ? e.Produto.ValorSeguro.Value : 0);
                        item.Total.ICMSTotal.ValorSeguro = Arredondar(item.Total.ICMSTotal.ValorSeguro, 2);
                        valorSeguroTrue = Arredondar(valorSeguroTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.ValorSeguro.ToString()),
                            new Error("Informe o somatório do valor do seguro.", "Item.Total.ICMSTotal.ValorSeguro"));
                        entity.Fail(!valorSeguroTrue.Equals(item.Total.ICMSTotal.ValorSeguro),
                            new Error("O somatório do valor do seguro não confere com os valores informados.", "Item.Total.ICMSTotal.ValorSeguro"));
                        #endregion

                        #region SomatorioDesconto
                        double? somatorioDesconto = item.Detalhes.Sum(e => e.Produto.ValorDesconto.HasValue ? e.Produto.ValorDesconto.Value : 0);
                        item.Total.ICMSTotal.SomatorioDesconto = Arredondar(item.Total.ICMSTotal.SomatorioDesconto, 2);
                        somatorioDesconto = Arredondar(somatorioDesconto, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioDesconto.ToString()),
                            new Error("Informe o somatório do valor de desconto.", "Item.Total.ICMSTotal.SomatorioDesconto"));
                        entity.Fail(!somatorioDesconto.Equals(item.Total.ICMSTotal.SomatorioDesconto),
                            new Error("O somatório do valor de desconto não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioDesconto"));
                        #endregion

                        #region SomatorioII
                        double? somatorioIITrue = item.Detalhes.Sum(e => e.Imposto.II != null ? e.Imposto.II.ValorII : 0);
                        item.Total.ICMSTotal.SomatorioII = Arredondar(item.Total.ICMSTotal.SomatorioII, 2);
                        somatorioIITrue = Arredondar(somatorioIITrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioII.ToString()),
                            new Error("Informe o somatório do valor de II.", "Item.Total.ICMSTotal.SomatorioII"));
                        entity.Fail(!somatorioIITrue.Equals(item.Total.ICMSTotal.SomatorioII),
                            new Error("O somatório do valor de II não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioII"));
                        #endregion SomatorioII

                        #region SomatorioIPI
                        double somatorioIPITrue = item.Detalhes.Sum(e => e.Imposto.IPI != null ? e.Imposto.IPI.ValorIPI : 0);
                        item.Total.ICMSTotal.SomatorioIPI = Arredondar(item.Total.ICMSTotal.SomatorioIPI, 2);
                        somatorioIPITrue = Arredondar(somatorioIPITrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioIPI.ToString()),
                            new Error("Informe o somatório do valor de IPI.", "Item.Total.ICMSTotal.SomatorioIPI"));
                        entity.Fail(!somatorioIPITrue.Equals(item.Total.ICMSTotal.SomatorioIPI),
                            new Error("O somatório do valor de IPI não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioIPI"));
                        #endregion SomatorioIPI

                        #region SomatorioPIS
                        double somatorioPISTrue = item.Detalhes.Sum(e => e.Imposto.PIS != null ? e.Imposto.PIS.ValorPIS : 0);
                        item.Total.ICMSTotal.SomatorioPis = Arredondar(item.Total.ICMSTotal.SomatorioPis, 2);
                        somatorioPISTrue = Arredondar(somatorioPISTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioPis.ToString()),
                            new Error("Informe o somatório do valor de PIS.", "Item.Total.ICMSTotal.SomatorioPis"));
                        entity.Fail(!somatorioPISTrue.Equals(item.Total.ICMSTotal.SomatorioPis),
                            new Error("O somatório do valor de PIS não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioPis"));
                        #endregion SomatorioPIS

                        #region SomatorioCofins
                        double somatorioCofinsTrue = item.Detalhes.Sum(e => e.Imposto.COFINS != null ? e.Imposto.COFINS.ValorCOFINS : 0);
                        item.Total.ICMSTotal.SomatorioCofins = Arredondar(item.Total.ICMSTotal.SomatorioCofins, 2);
                        somatorioCofinsTrue = Arredondar(somatorioCofinsTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioCofins.ToString()),
                            new Error("Informe o somatório do valor de COFINS.", "Item.Total.ICMSTotal.SomatorioCofins"));
                        entity.Fail(!somatorioCofinsTrue.Equals(item.Total.ICMSTotal.SomatorioCofins),
                            new Error("O somatório do valor de COFINS não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioCofins"));
                        #endregion SomatorioCofins

                        #region SomatorioOutro
                        double somatorioOutroTrue = item.Detalhes.Sum(e => e.Produto.ValorOutrasDespesas.HasValue ? e.Produto.ValorOutrasDespesas.Value : 0);
                        item.Total.ICMSTotal.SomatorioOutro = Arredondar(item.Total.ICMSTotal.SomatorioOutro, 2);
                        somatorioOutroTrue = Arredondar(somatorioOutroTrue, 2);

                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.SomatorioOutro.ToString()),
                            new Error("Informe o somatório de valor de Outros.", "Item.Total.ICMSTotal.SomatorioOutro"));
                        entity.Fail(!somatorioOutroTrue.Equals(item.Total.ICMSTotal.SomatorioOutro),
                            new Error("O somatório do valor de Outros não confere com os valores informados.", "Item.Total.ICMSTotal.SomatorioOutro"));
                        #endregion SomatorioOutro

                        #region ValorTotalNF
                        entity.Fail(string.IsNullOrEmpty(item.Total.ICMSTotal.ValorTotalNF.ToString()),
                            new Error("Informe o valor total da NFE.", "Item.Total.ICMSTotal.ValorTotalNF"));
                        #endregion ValorTotalNF

                    }
                }

                #endregion Validação da classe Totais
            }

            base.ValidaModel(entity);
        }
    }
}