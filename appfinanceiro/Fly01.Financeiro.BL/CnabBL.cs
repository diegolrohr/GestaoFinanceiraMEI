using System;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using System.Linq;
using Fly01.Core.ViewModels;
using System.Text.RegularExpressions;
using System.Text;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.BL
{
    public class CnabBL : PlataformaBaseBL<Cnab>
    {
        protected ContaReceberBL contaReceberBL;
        protected ContaBancariaBL contaBancariaBL;

        private string codigoCedente;
        const double jurosDiaPadrao = 0.33;
        const double percentMultaPadrao = 2.00;

        public CnabBL(AppDataContextBase context, ContaReceberBL contaReceberBL, ContaBancariaBL contaBancariaBL) : base(context)
        {
            this.contaReceberBL = contaReceberBL;
            this.contaBancariaBL = contaBancariaBL;
        }

        public virtual IQueryable<Cnab> Everything => repository.All.Where(x => x.PlataformaId == PlataformaUrl);

        #region ApiMethods

        public Boleto2Net.BoletoBancario ImprimeBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            return GeraBoleto(GetDadosBoleto(contaReceberId, contaBancariaId));
        }

        public BoletoVM GetDadosBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            int nossoNumero = 0;
            var cnabReemprime = base.All.FirstOrDefault(x => x.ContaBancariaCedenteId == contaBancariaId && x.ContaReceberId == contaReceberId);

            if (cnabReemprime != null)
                nossoNumero = cnabReemprime.NossoNumero;
            else
            {
                var max = Everything.Any() ? Everything.Max(x => x.NossoNumero) : 0;
                max = (max == 1 && !Everything.Any(x => x.Ativo && x.NossoNumero == 1)) ? 0 : max;
                nossoNumero = ++max;
            };

            var contaReceber = contaReceberBL.Find(contaReceberId);
            var contaBancariaCedente = contaBancariaBL.AllIncluding(r => r.Banco).FirstOrDefault(x => x.Id == contaBancariaId);
            var cedente = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            var sacado = contaReceberBL.AllIncluding(r => r.Pessoa, r => r.Pessoa.Cidade, r => r.Pessoa.Cidade.Estado).Where(x => x.PessoaId == contaReceber.PessoaId).FirstOrDefault()?.Pessoa;

            codigoCedente = contaBancariaCedente.CodigoCedente;

            var juros = contaBancariaCedente.TaxaJuros ?? jurosDiaPadrao;
            var percentMulta = contaBancariaCedente.PercentualMulta ?? percentMultaPadrao;
            var valorParcial = contaReceber.ValorPrevisto - (contaReceber.ValorPago ?? 0);

            var valorMulta = (decimal)(valorParcial * (percentMulta / 100));
            var valorJuros = (decimal)(valorParcial * (juros / 100));
            var numerosGuidContaReceber = Regex.Replace(contaReceber.Id.ToString(), "[^0-9]", "");
            var randomNossoNumero = new Random().Next(0, 9999999);
            var dataCedente = GetDadosCedente(contaBancariaId);

            var boletoVM = new BoletoVM()
            {
                ValorPrevisto = (decimal)contaReceber.ValorPrevisto - (Convert.ToDecimal(contaReceber.ValorPago?? 0)),
                ValorDesconto = contaReceber.ValorDesconto.HasValue ? (decimal)contaReceber.ValorDesconto : 0,
                ValorMulta = valorMulta,
                ValorJuros = valorJuros,
                DataEmissao = DateTime.Now,
                DataDesconto = contaReceber.DataDesconto ?? null,
                DataVencimento = contaReceber.DataVencimento,
                NossoNumero = nossoNumero,
                NossoNumeroFormatado = FormataNossoNumero(dataCedente.CodigoCedente, dataCedente.ContaBancariaCedente.CodigoBanco, nossoNumero),
                EspecieMoeda = "R$",
                NumeroDocumento = $"BB{randomNossoNumero.ToString("D6")}",
                InstrucoesCaixa = GetInstrucoesAoCaixa(contaReceber, contaBancariaCedente),
                Cedente = dataCedente,
                Sacado = GetDadosSacado(contaReceber.PessoaId)
            };

            if (ValidaDadosBoleto(boletoVM))
                return boletoVM;
            else
                return null;
        }

        public Cnab GetCnab(Guid Id)
        {
            if (Id != Guid.Empty)
                return base.AllIncluding(b => b.ContaReceber, b => b.ContaReceber.Pessoa).Where(x => x.Id == Id).FirstOrDefault();

            return null;
        }

        #endregion

        private bool ValidaDadosBoleto(BoletoVM boleto)
        {
            if (string.IsNullOrEmpty(boleto.Cedente.EnderecoComplemento)) boleto.Cedente.EnderecoComplemento = "---";
            if (string.IsNullOrEmpty(boleto.Cedente.EnderecoNumero)) boleto.Cedente.EnderecoNumero = "0";
            if (string.IsNullOrEmpty(boleto.Sacado.EnderecoComplemento)) boleto.Sacado.EnderecoComplemento = "---";
            if (string.IsNullOrEmpty(boleto.Sacado.EnderecoNumero)) boleto.Sacado.EnderecoNumero = "0";

            if (string.IsNullOrEmpty(boleto.Cedente.CodigoCedente)) throw new Exception("É necessário informar o código do cedente");
            if (string.IsNullOrEmpty(boleto.Cedente.CodigoDV)) boleto.Cedente.CodigoDV = "0";

            return true;
        }

        private SacadoVM GetDadosSacado(Guid pessoaId)
        {
            var sacado = contaReceberBL.AllIncluding(r => r.Pessoa, r => r.Pessoa.Cidade, r => r.Pessoa.Cidade.Estado).Where(x => x.PessoaId == pessoaId).FirstOrDefault()?.Pessoa;

            return new SacadoVM
            {
                CNPJ = sacado.CPFCNPJ,
                Nome = sacado.Nome,
                Endereco = sacado.Endereco,
                EnderecoNumero = sacado.Numero,
                EnderecoComplemento = sacado.Complemento,
                EnderecoBairro = sacado.Bairro,
                EnderecoCidade = sacado.Cidade?.Nome,
                EnderecoUF = sacado.Cidade?.Estado?.Sigla,
                EnderecoCEP = sacado.CEP,
                Observacoes = ""
            };
        }

        public CedenteVM GetDadosCedente(Guid contaBancariaId)
        {
            var cedente = ApiEmpresaManager.GetEmpresa(PlataformaUrl);

            return new CedenteVM
            {
                CNPJ = cedente.CNPJ,
                RazaoSocial = cedente.RazaoSocial,
                Endereco = cedente.Endereco,
                EnderecoNumero = cedente.Numero,
                EnderecoComplemento = "",
                EnderecoBairro = cedente.Bairro,
                EnderecoCidade = cedente.Cidade.Nome,
                EnderecoUF = cedente.Cidade.Estado.Sigla,
                EnderecoCEP = cedente.CEP,
                Observacoes = "",
                CodigoCedente = codigoCedente,
                ContaBancariaCedente = GetContaBancariaCedente(contaBancariaId)
            };
        }

        private ContaBancariaCedenteVM GetContaBancariaCedente(Guid contaBancariaId)
        {
            var contaBancariaCedente = contaBancariaBL.AllIncluding(b => b.Banco).FirstOrDefault(x => x.Id == contaBancariaId);

            return new ContaBancariaCedenteVM
            {
                Agencia = contaBancariaCedente.Agencia,
                DigitoAgencia = contaBancariaCedente.DigitoAgencia,
                Conta = contaBancariaCedente.Conta,
                DigitoConta = contaBancariaCedente.DigitoConta,
                CodigoBanco = int.Parse(contaBancariaCedente.Banco.Codigo)
            };
        }

        private string GetInstrucoesAoCaixa(ContaReceber conta, ContaBancaria contaBancaria)
        {
            var juros = contaBancaria.TaxaJuros ?? jurosDiaPadrao;
            var percentMulta = contaBancaria.PercentualMulta ?? percentMultaPadrao;

            var valorParcial = conta.ValorPrevisto - (conta.ValorPago ?? 0);
            var valorMulta = (decimal)(valorParcial * (percentMulta / 100));
            var valorJuros = (decimal)(valorParcial * (juros / 100));
            var msgCaixa = new StringBuilder();

            if (conta.ValorDesconto.HasValue && conta.DataDesconto.HasValue) msgCaixa.AppendLine($"Conceder desconto de {conta.ValorDesconto.Value.ToString("R$ ##,##0.00")} até {conta.DataDesconto.Value.ToString("dd/MM/yyyy")}. ");
            if (percentMultaPadrao > 0) msgCaixa.AppendLine($"Cobrar multa de {valorMulta.ToString("R$ ##,##0.00")} após o vencimento. ");
            if (jurosDiaPadrao > 0) msgCaixa.AppendLine($"Cobrar juros de {valorJuros.ToString("R$ ##,##0.00")} por dia de atraso. ");

            return msgCaixa.ToString();
        }

        public override void Insert(Cnab entity)
        {
            if (!All.Any(x => x.ContaReceberId == entity.ContaReceberId))
                base.Insert(entity);
        }

        public Boleto2Net.BoletoBancario GeraBoleto(BoletoVM boleto)
        {
            var mensagemBoleto = "";
            var proxy = new Boleto2Net.Boleto2NetProxy();
            var cedente = boleto.Cedente;
            var contaCedente = cedente.ContaBancariaCedente;
            var sacado = boleto.Sacado;
            var carteira = BoletoBL.GetTipoCarteira(boleto.Cedente.ContaBancariaCedente.CodigoBanco);

            if (!proxy.SetupCobranca(cedente.CNPJ, cedente.RazaoSocial, cedente.Endereco, cedente.EnderecoNumero, cedente.EnderecoComplemento, cedente.EnderecoBairro,
                cedente.EnderecoCidade, cedente.EnderecoUF, cedente.EnderecoCEP, cedente.Observacoes, contaCedente.CodigoBanco, contaCedente.Agencia, contaCedente.DigitoAgencia,
                "1", contaCedente.Conta, contaCedente.DigitoConta, cedente.CodigoCedente, cedente.CodigoDV, "", carteira.CarteiraPadrao, carteira.VariacaoCarteira,
                (int)Boleto2Net.TipoCarteira.CarteiraCobrancaSimples, (int)Boleto2Net.TipoFormaCadastramento.ComRegistro, (int)Boleto2Net.TipoImpressaoBoleto.Empresa, (int)Boleto2Net.TipoDocumento.Tradicional, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.NovoBoleto(ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirSacado(cedente.CNPJ, sacado.Nome, sacado.Endereco, sacado.EnderecoNumero, sacado.EnderecoComplemento, sacado.EnderecoBairro, sacado.EnderecoCidade,
                sacado.EnderecoUF, sacado.EnderecoCEP, sacado.Observacoes, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirBoleto(Boleto2Net.TipoEspecieDocumento.DM.ToString(), boleto.NumeroDocumento, FormataNossoNumero(cedente.CodigoCedente, cedente.ContaBancariaCedente.CodigoBanco, boleto.NossoNumero), boleto.DataEmissao,
                DateTime.Now, boleto.DataVencimento, boleto.ValorPrevisto, boleto.NumeroDocumento, "N", ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirMulta(boleto.DataVencimento, boleto.ValorMulta, 2, ref mensagemBoleto)) throw new Exception(mensagemBoleto);
            if (!proxy.DefinirJuros(boleto.DataVencimento.AddDays(1), boleto.ValorJuros, 3, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (boleto.DataDesconto.HasValue)
                if (!proxy.DefinirDesconto(boleto.DataDesconto.Value, boleto.ValorDesconto.Value, ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            if (!proxy.DefinirInstrucoes(boleto.InstrucoesCaixa, "", "", "", "", "", "", "", ref mensagemBoleto)) throw new Exception(mensagemBoleto);

            proxy.FecharBoleto(ref mensagemBoleto);

            var result = new Boleto2Net.BoletoBancario
            {
                Boleto = proxy.boleto,
                OcultarInstrucoes = false,
                MostrarComprovanteEntrega = false,
                MostrarEnderecoCedente = true
            };

            //Devolve o nosso número sem formatação.
            result.Boleto.NossoNumero = boleto.NossoNumero.ToString();

            return result;
        }

        protected string FormataNossoNumero(string codigoCedente, int codigoBanco, int nossoNumero)
        {
            var tipo = (TipoCodigoBanco)Enum.ToObject(typeof(TipoCodigoBanco), codigoBanco);
            switch (tipo)
            {
                case TipoCodigoBanco.BancoBrasil:
                    return $"{codigoCedente}{nossoNumero.ToString().PadLeft(10, '0')}";
                case TipoCodigoBanco.Itau:
                    return nossoNumero.ToString().PadLeft(8, '0');
                case TipoCodigoBanco.Banrisul:
                    return nossoNumero.ToString().PadLeft(8, '0');
                case TipoCodigoBanco.Bradesco:
                    return nossoNumero.ToString().PadLeft(11, '0');
                case TipoCodigoBanco.Caixa:
                    return $"{14}{nossoNumero.ToString().PadLeft(15, '0')}";
                case TipoCodigoBanco.Santander:
                    return nossoNumero.ToString().PadLeft(12, '0');
            }
            return nossoNumero.ToString();
        }

        public void SalvaBoleto(Boleto2Net.BoletoBancario boleto, Guid contaReceberId, Guid contaBancariaId, bool reimprimeBoleto)
        {
            var cnabToEdit = Everything.Where(x => x.ContaReceberId.Value.Equals(contaReceberId));

            var cnab = new Cnab()
            {
                Status = StatusCnab.BoletoGerado,
                DataEmissao = boleto.Boleto.DataEmissao,
                DataVencimento = boleto.Boleto.DataVencimento,
                NossoNumero = Convert.ToInt32(boleto.Boleto.NossoNumero),
                NossoNumeroFormatado = FormataNossoNumero(boleto.Boleto.Banco.Cedente.Codigo, boleto.Boleto.Banco.Codigo, Convert.ToInt32(boleto.Boleto.NossoNumero)),
                DataDesconto = boleto.Boleto.DataDesconto,
                ValorDesconto = (double)boleto.Boleto.ValorDesconto,
                ContaBancariaCedenteId = contaBancariaId,
                ContaReceberId = contaReceberId,
                ValorBoleto = (double)boleto.Boleto.ValorTitulo
            };

            if (!reimprimeBoleto)
            {
                if (cnabToEdit.Count() <= 0)
                    base.Insert(cnab);
            }
            else
            {
                if (cnabToEdit != null)
                {
                    if (contaBancariaId != cnabToEdit.FirstOrDefault().ContaBancariaCedenteId)
                    {
                        cnab.Id = cnabToEdit.FirstOrDefault().Id;
                        base.Update(cnab);
                    }
                }
            }
        }
    }
}