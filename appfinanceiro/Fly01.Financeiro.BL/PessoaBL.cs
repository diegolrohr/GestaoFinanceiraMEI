using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.ValueObjects;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using System.Data.Entity;

namespace Fly01.Financeiro.BL
{
    public class PessoaBL : EmpresaBaseBL<Pessoa>
    {
        protected EstadoBL EstadoBL;
        protected CidadeBL CidadeBL;

        public PessoaBL(AppDataContext context, EstadoBL uFBL, CidadeBL cidadeBL) : base(context)
        {
            EstadoBL = uFBL;
            CidadeBL = cidadeBL;
            MustConsumeMessageServiceBus = true;
        }

        #region Notification

        public override void ValidaModel(Pessoa entity)
        {
            ValidaModelNoBase(entity);

            base.ValidaModel(entity);
        }

        public void ValidaModelNoBase(Pessoa entity)
        {
            ValidaDefaultCPFCNPJTipoDocumento(entity);
            entity.Fail(string.IsNullOrWhiteSpace(entity.Nome), NomeInvalido);
            entity.Fail(!entity.Cliente && !entity.Fornecedor, TipoCadastroInvalido);
            entity.Fail(entity.TipoDocumento != "J" && entity.TipoDocumento != "F", TipoDocumentoInvalido);
            ValidaFormatoDocumento(entity);
            ValidaFormatoCep(entity);
            entity.Fail(entity.Estado != null && !EstadoBL.All.Any(x => x.Sigla.Equals(entity.Estado.Sigla, StringComparison.CurrentCultureIgnoreCase)), SiglaEstadoInvalida);
            ValidaCidade(entity, entity.Estado);
            ValidaEmail(entity);
            ValidaCPFCNPJ(entity);
        }

        protected void ValidaCPFCNPJ(Pessoa entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.CPFCNPJ.ToString()))
            {
                var pessoa = All.AsNoTracking().FirstOrDefault(x => x.CPFCNPJ.Trim().ToUpper() == entity.CPFCNPJ.Trim().ToUpper() && x.Id != entity.Id);
                if (pessoa == null)
                {
                    pessoa = ContextAddedEntriesSelfType().FirstOrDefault(x => x.CPFCNPJ.Trim().ToUpper() == entity.CPFCNPJ.Trim().ToUpper() && x.Id != entity.Id);
                }
                entity.Fail(pessoa != null, new Error("O CPF/CNPJ informado já foi utilizado em outro cadastro.", "cpfcnpj", pessoa?.Id.ToString()));
            }
        }

        protected void ValidaDefaultCPFCNPJTipoDocumento(Pessoa entity)
        {
            entity.CPFCNPJ = !string.IsNullOrEmpty(entity.CPFCNPJ) ? entity.CPFCNPJ : string.Empty;
            entity.TipoDocumento = !string.IsNullOrEmpty(entity.TipoDocumento) ? entity.TipoDocumento : "F";
        } 

        protected void ValidaFormatoDocumento(Pessoa entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.CPFCNPJ.ToString()))
            {
                switch (entity.TipoDocumento)
                {
                    case "F":
                        entity.Fail(!CPF.ValidaNumero(entity.CPFCNPJ), FormatoDocumentoInvalido);
                        break;
                    case "J":
                        entity.Fail(!CNPJ.ValidaNumero(entity.CPFCNPJ), FormatoDocumentoInvalido);
                        break;
                    default:
                        entity.Fail(true, FormatoDocumentoInvalido);
                        break;
                }
            }

        }

        protected void ValidaFormatoCep(Pessoa entity)
        {
            if (!string.IsNullOrEmpty(entity.CEP))
            {
                entity.CEP = entity.CEP.Replace("-", "");

                entity.Fail(entity.CEP.Length != 8, FormatoCepInvalido);
            }
        }

        protected void ValidaEmail(Pessoa entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Email)) return;

            const string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            entity.Fail(!Regex.IsMatch(entity.Email ?? "", pattern), EmailInvalido);
        }

        protected void ValidaCidade(Pessoa entity, Estado estado)
        {
            entity.Fail(
                entity.Cidade != null
                && !CidadeBL.All.Any(x => x.EstadoId == entity.EstadoId && x.Nome.Equals(entity.Cidade.Nome, StringComparison.CurrentCultureIgnoreCase)), NomeCidadeInvalido);
        }

        public static Error TipoCadastroInvalido = new Error("Informe se ao menos a pessoa é um Cliente e/ou Fornecedor.");
        public static Error TipoDocumentoInvalido = new Error("Tipo documento inválido, somente J ou F.");
        public static Error FormatoDocumentoInvalido = new Error("Formato de documento inválido.", "cpfcnpj");
        public static Error FormatoCepInvalido = new Error("O CEP informado está incorreto. Solução: Informe todos os 8 números.", "cep");
        public static Error SiglaEstadoInvalida = new Error("Sigla do estado inválida.", "estadoNome");
        public static Error NomeCidadeInvalido = new Error("O nome da cidade está incorreto ou a cidade não pertence ao estado selecionado.", "cidadeNome");
        public static Error EmailInvalido = new Error("Informe um e-mail válido.", "email");
        public static Error NomeInvalido = new Error("Informe um nome válido.", "nome");
        public static Error CidadeInvalida = new Error("Código da cidade inválida.", "cidadeId");

        public bool IsValid(Pessoa entity)
        {
            return !entity.Notification.HasErrors;
        }

        #endregion

        public override void Update(Pessoa entity)
        {
            ValidaModel(entity);
            if (!IsValid(entity))
            {
                var errors = entity.Notification.Errors.Cast<object>().Aggregate("", (current, item) => current + (item + "\n"));
                throw new BusinessException(errors);
            }

            base.Update(entity);
        }

        public override void Insert(Pessoa entity)
        {
            ValidaModel(entity);
            if (!IsValid(entity))
            {
                var errors = entity.Notification.Errors.Cast<object>().Aggregate("", (current, item) => current + (item + "\n"));
                throw new BusinessException(errors);
            }

            base.Insert(entity);
        }

        public void Persist()
        {

        }

        public Guid BuscaPessoaNome(string nomePessoa, bool cliente, bool fornecedor)
        {
            var pessoaPadrao = All.AsNoTracking().FirstOrDefault(x => x.Nome == nomePessoa && x.Cliente == cliente && x.Fornecedor == fornecedor && x.Ativo == true);

            //Se Pessoa nao existe, insere
            if (pessoaPadrao == null)
            {
                var novaPessoa = new Pessoa
                {
                    Id = Guid.NewGuid(),
                    Nome = nomePessoa,
                    TipoDocumento = "F",
                    Cliente = cliente,
                    Fornecedor = fornecedor,
                    CPFCNPJ = string.Empty
                };
                base.Insert(novaPessoa);
                return novaPessoa.Id;
            }
            return pessoaPadrao.Id;
        }
    }
}