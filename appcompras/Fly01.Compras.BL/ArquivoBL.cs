using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using Fly01.Core.BL;
using Fly01.Core.Helpers;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Compras.DAL;

namespace Fly01.Compras.BL
{
    public class ArquivoBL : PlataformaBaseBL<Arquivo>
    {
        protected PessoaBL PessoaBL;
        protected IList<Pessoa> insertedPessoas = new List<Pessoa>();
        protected IList<Produto> insertedProdutos = new List<Produto>();
        private readonly string routingKeyPessoa = @"Pessoa";
        private readonly string routingKeyProduto = @"Produto";
        protected ProdutoBL ProdutoBL;
        protected GrupoProdutoBL GrupoProdutoBL;
        protected UnidadeMedidaBL UnidadeMedidaBL;

        public ArquivoBL(AppDataContext context, PessoaBL pessoaBL, ProdutoBL produtoBL, GrupoProdutoBL grupoProdutoBL, UnidadeMedidaBL unidadeMedidaBL)
            : base(context)
        {
            PessoaBL = pessoaBL;
            ProdutoBL = produtoBL;
            GrupoProdutoBL = grupoProdutoBL;
            UnidadeMedidaBL = unidadeMedidaBL;
        }

        public override void Insert(Arquivo entity)
        {
            base.Insert(entity);

            if (entity.Cadastro == "Produto")
                ImportaProduto(entity);
            else
                ImportaPessoa(entity);
        }

        public void ImportaPessoa(Arquivo arquivo)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(arquivo.Conteudo))
                {
                    var content = Base64Helper.DecodificaBase64(arquivo.Conteudo).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                    var cols = Colunas(content[0]);
                    var cpf = new List<string>();
                    var cnpj = new List<string>();
                    if (!ValidaColunasImportadas(cols, PessoaBL.ColunasParaImportacao()))
                        throw new BusinessException("Colunas inválidas");

                    int count = 0;
                    for (var i = 1; i < content.Length; i++)
                    {
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(content[i]))
                            {
                                var pessoa = PopulaEntidade(new Pessoa(), cols, content[i]);
                                pessoa.CPFCNPJ = Regex.Replace(pessoa.CPFCNPJ ?? "", @"[^\d]", "").PadLeft(11, '0');

                                var cpjcnpjJaExiste = false;
                                if (pessoa.CPFCNPJ == "00000000000")
                                    pessoa.CPFCNPJ = string.Empty;

                                switch (pessoa.TipoDocumento)
                                {
                                    case "F":
                                        cpjcnpjJaExiste = cpf.Any(x => x == pessoa.CPFCNPJ && !string.IsNullOrEmpty(pessoa.CPFCNPJ));
                                        cpf.Add(pessoa.CPFCNPJ);
                                        break;
                                    case "J":
                                        cpjcnpjJaExiste = cnpj.Any(x => x == pessoa.CPFCNPJ && !string.IsNullOrEmpty(pessoa.CPFCNPJ));
                                        cnpj.Add(pessoa.CPFCNPJ);
                                        break;
                                }

                                if (cpjcnpjJaExiste)
                                    pessoa.Notification.Errors.Add(new Error(string.Format("CPF/CNPJ duplicado no arquivo : {0} - {1}", pessoa.Nome, pessoa.CPFCNPJ)));

                                PessoaBL.ValidaModelNoBase(pessoa);
                                if (!cpjcnpjJaExiste && PessoaBL.IsValid(pessoa))
                                {
                                    PessoaBL.Insert(pessoa);
                                    insertedPessoas.Add(pessoa);

                                    count = count + 1;
                                }
                                else
                                {
                                    var errors = pessoa.Notification.Errors.Select(e =>
                                    {
                                        return string.Format("Campo: {0}; Mensagem: {1}", e.DataField, e.Message);
                                    }).FirstOrDefault();
                                    arquivo.Retorno += "Linha " + (i + 1).ToString().PadLeft(5, '0') + ";" + errors + "\n";
                                }
                            }
                        }
                        catch
                        { }
                        finally
                        {
                            var totalProcessado = (double)i / content.Length * 100.0;
                            arquivo.TotalProcessado = (totalProcessado > 100 || i == (content.Length - 1))
                                ? 100.0
                                : totalProcessado;
                        }
                    }

                    arquivo.Retorno += count + " Registros inseridos com sucesso.";
                    Update(arquivo);
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message + ex.InnerException?.Message + ex.InnerException?.InnerException?.Message;
                throw new BusinessException(message + " " + ex.InnerException?.Message);
            }
        }

        public override void AfterSave(Arquivo entity)
        {
            if (entity.Cadastro == "Produto")
            {
                foreach (var item in insertedProdutos)
                    Producer<Produto>.Send(routingKeyProduto, AppUser, PlataformaUrl, item, RabbitConfig.EnHttpVerb.POST);
            }
            else
            {
                foreach (var item in insertedPessoas)
                    Producer<Pessoa>.Send(routingKeyPessoa, AppUser, PlataformaUrl, item, RabbitConfig.EnHttpVerb.POST);
            }
        }

        private Pessoa PopulaEntidade(Pessoa pessoa, List<string> cols, string row)
        {
            var values = new Regex(@",[^\d\w]")
                .Split(row)
                .Select(x => x.Replace("\"", ""))
                .ToList();

            if (cols.Count == values.Count)
            {
                for (var c = 0; c < cols.Count; c++)
                {
                    var propertyInfo = pessoa.GetType().GetProperty(cols[c]);
                    var type = propertyInfo?.PropertyType;
                    var value = string.IsNullOrWhiteSpace(values[c]) ? null : values[c];
                    if (value == null) continue;
                    propertyInfo?.SetValue(pessoa,
                        Convert.ChangeType(
                            type.Name == "Boolean" ? ConversorHelper.ToBool(value).ToString().ToLower() : value,
                            type), null);
                }
            }

            return pessoa;
        }

        public static List<string> Colunas(string linhaColunas)
        {
            var colunas = new Regex(@",[^\d\w]")
                .Split(linhaColunas)
                .Select(x => x.Replace("\"", ""))
                .ToList();

            return colunas;
        }

        public bool ValidaColunasImportadas(string linhaColunas, List<string> colunasEsperadas)
        {
            var colunasInformadas = new Regex(@",[^\d\w]")
                .Split(linhaColunas)
                .Select(x => x.Replace("\"", ""))
                .ToList();

            var retorno = ValidaColunasImportadas(colunasInformadas, colunasEsperadas);

            return retorno;
        }

        public bool ValidaColunasImportadas(List<string> colunasInformadas, List<string> colunasEsperadas)
        {
            var retorno = true;
            var colunasParaImportacao = colunasEsperadas;

            if (colunasInformadas.Count != colunasParaImportacao.ToList().Count)
                retorno = false;

            foreach (var coluna in colunasInformadas)
                if (!colunasParaImportacao.Any(x => x.Contains(coluna)))
                    retorno = false;

            return retorno;
        }
        public void ImportaProduto(Arquivo arquivo)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(arquivo.Conteudo))
                {
                    var content = Base64Helper.DecodificaBase64(arquivo.Conteudo).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                    var cols = Colunas(content[0]);
                    if (!ValidaColunasImportadas(cols, ProdutoBL.ColunasParaImportacao()))
                        throw new BusinessException("Colunas inválidas");

                    for (var i = 1; i < content.Length; i++)
                    {
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(content[i]))
                                PopularArquivoProduto(arquivo, content, cols, i);
                        }
                        catch
                        { }
                        finally
                        {
                            var totalProcessado = (double)i / content.Length * 100.0;
                            arquivo.TotalProcessado = (totalProcessado > 100 || i == (content.Length - 1))
                                ? 100.0
                                : totalProcessado;
                        }
                    }
                    Update(arquivo);
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message + ex.InnerException?.Message + ex.InnerException?.InnerException?.Message;
                throw new BusinessException(message + " " + ex.InnerException?.Message);
            }
        }

        private void PopularArquivoProduto(Arquivo arquivo, string[] content, List<string> cols, int i)
        {
            var produto = PopulaEntidadeProduto(new Produto(), cols, content[i]);

            InsertProduto(arquivo, i, produto);
        }

        private Produto PopulaEntidadeProduto(Produto produto, List<string> cols, string row)
        {
            var values = new Regex(@",[^\d\w]")
                .Split(row)
                .Select(x => x.Replace("\"", ""))
                .ToList();

            if (cols.Count == values.Count)
            {
                for (var c = 0; c < cols.Count; c++)
                {
                    var propertyInfo = produto.GetType().GetProperty(cols[c]);
                    var type = propertyInfo?.PropertyType;
                    var value = string.IsNullOrWhiteSpace(values[c]) ? null : values[c];
                    if (value == null) continue;
                    propertyInfo?.SetValue(produto,
                        Convert.ChangeType(
                            type.Name == "Boolean" ? ConversorHelper.ToBool(value).ToString().ToLower() : value,
                            type), null);
                }
            }
            return produto;
        }

        private void InsertProduto(Arquivo arquivo, int i, Produto produto)
        {
            ValidaModel(produto);

            if (IsValid(produto))
            {
                ProdutoBL.Insert(produto);
                insertedProdutos.Add(produto);
                arquivo.Retorno += "Linha " + (i + 1).ToString().PadLeft(5, '0') + ";" + produto.Descricao + " cadastrado com sucesso\n";
            }
            else
            {
                var errors = produto.Notification.Errors.Select(e =>
                {
                    return string.Format("Campo: {0}; Mensagem: {1}", e.DataField, e.Message);
                }).FirstOrDefault();
                arquivo.Retorno += "Linha " + (i + 1).ToString().PadLeft(5, '0') + ";" + errors + "\n";
            }
        }
        public void ValidaModel(Produto produto)
        {
            produto.ObjetoDeManutencao = ObjetoDeManutencao.Nao;
            produto.TipoProduto = TipoProduto.ProdutoFinal;
            produto.SaldoProduto = 0;

            if (produto.AbreviacaoUnidadeMedida != null)
            {
                var UnidadeMedida = UnidadeMedidaBL.All.Where(x => x.Abreviacao == produto.AbreviacaoUnidadeMedida).FirstOrDefault()?.Id;
                produto.UnidadeMedidaId = UnidadeMedida;
                produto.AbreviacaoUnidadeMedida = null;
            }

            produto.Fail(produto.UnidadeMedidaId == null, UnidadeMedidaInvalida);
            produto.Fail(string.IsNullOrEmpty(produto.Descricao), DescricaoEmBranco);
            produto.Fail(All.Where(x => x.Descricao == produto.Descricao).Any(x => x.Id != produto.Id), DescricaoDuplicada);
            if (!string.IsNullOrWhiteSpace(produto.CodigoProduto))
                produto.Fail(ProdutoBL.All.Where(x => x.CodigoProduto == produto.CodigoProduto).Any(x => x.Id != produto.Id), CodigoProdutoDuplicado);
        }

        public bool IsValid(Produto produto)
        {
            return !produto.Notification.HasErrors;
        }

        public static Error DescricaoEmBranco = new Error("Descrição não foi informada.", "descricao");
        public static Error DescricaoDuplicada = new Error("Descrição já utilizada anteriormente.", "descricao");
        public static Error UnidadeMedidaInvalida = new Error("Unidade de medida não foi encontrada ou informada.", "UnidadeDeMedida");
        public static Error CodigoProdutoDuplicado = new Error("Código do produto já utilizado anteriormente.", "CodigoProduto");



    }
}
