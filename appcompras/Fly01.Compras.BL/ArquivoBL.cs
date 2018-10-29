using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Fly01.Compras.DAL;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using Fly01.Core.BL;
using Fly01.Core.Helpers;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.BL
{
    public class ArquivoBL : PlataformaBaseBL<Arquivo>
    {
        protected PessoaBL PessoaBL;
        protected IList<Pessoa> insertedPessoas = new List<Pessoa>();
        private readonly string routingKeyPessoa = @"Pessoa";

        public ArquivoBL(AppDataContext context, PessoaBL pessoaBL)
            : base(context)
        {
            PessoaBL = pessoaBL;
        }

        public override void Insert(Arquivo entity)
        {
            base.Insert(entity);
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

                    for (var i = 1; i < content.Length; i++)
                    {
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(content[i]))
                                PopularArquivo(arquivo, content, cols, cpf, cnpj, i);   
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

        private void PopularArquivo(Arquivo arquivo, string[] content, List<string> cols, List<string> cpf, List<string> cnpj, int i)
        {
            var pessoa = PopulaEntidade(new Pessoa(), cols, content[i]);
            pessoa.CPFCNPJ = Regex.Replace(pessoa.CPFCNPJ ?? "", @"[^\d]", "").PadLeft(11, '0');

            var cpjcnpjJaExiste = false;
            switch (pessoa.TipoDocumento)
            {
                case "F":
                    cpjcnpjJaExiste = cpf.Any(x => x == pessoa.CPFCNPJ);
                    cpf.Add(pessoa.CPFCNPJ);
                    break;
                case "J":
                    cpjcnpjJaExiste = cnpj.Any(x => x == pessoa.CPFCNPJ);
                    cnpj.Add(pessoa.CPFCNPJ);
                    break;
            }

            if (cpjcnpjJaExiste)
                pessoa.Notification.Errors.Add(new Error(string.Format("CPF/CNPJ duplicado no arquivo : {0} - {1}", pessoa.Nome, pessoa.CPFCNPJ)));

            PessoaBL.ValidaModelNoBase(pessoa);
            InsertPessoa(arquivo, i, pessoa, cpjcnpjJaExiste);
        }

        private void InsertPessoa(Arquivo arquivo, int i, Pessoa pessoa, bool cpjcnpjJaExiste)
        {
            if (!cpjcnpjJaExiste && PessoaBL.IsValid(pessoa))
            {
                PessoaBL.Insert(pessoa);
                insertedPessoas.Add(pessoa);
                arquivo.Retorno += "Linha " + (i + 1).ToString().PadLeft(5, '0') + ";" + pessoa.Nome + ", CPF/CNPJ " + pessoa.CPFCNPJ + " cadastrado com sucesso\n";
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

        public override void AfterSave(Arquivo entity)
        {
            foreach (var item in insertedPessoas)
                Producer<Pessoa>.Send(routingKeyPessoa, AppUser, PlataformaUrl, item, RabbitConfig.EnHttpVerb.POST);
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
    }
}