using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Fly01.Faturamento.DAL;
using Fly01.Core.ValueObjects;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;
using System.Data.Entity;

namespace Fly01.Faturamento.BL
{
    public class PessoaBL : PlataformaBaseBL<Pessoa>
    {
        protected EstadoBL EstadoBL;
        protected CidadeBL CidadeBL;
        protected ArquivoBL ArquivoBL;

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
            entity.Fail(!entity.Transportadora && !entity.Cliente && !entity.Vendedor && !entity.Fornecedor, TipoCadastroInvalido);
            entity.Fail(entity.TipoDocumento != "J" && entity.TipoDocumento != "F", TipoDocumentoInvalido);
            ValidaFormatoDocumento(entity);
            ValidaFormatoCep(entity);
            entity.Fail(entity.Estado != null && !EstadoBL.All.Any(x => x.Sigla.Equals(entity.Estado.Sigla, StringComparison.CurrentCultureIgnoreCase)), SiglaEstadoInvalida);
            ValidaCidade(entity, entity.Estado);
            ValidaEmail(entity);
            ValidaInscricaoEstadual(entity);
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

        protected void ValidaInscricaoEstadual(Pessoa entity)
        {
            if (string.IsNullOrWhiteSpace(entity.EstadoId.ToString()) && (!string.IsNullOrWhiteSpace(entity.InscricaoEstadual)))
                throw new BusinessException("Preencha os campos Estado e Cidade para realizar a validação da Inscrição Estadual.");

            if (string.IsNullOrWhiteSpace(entity.CidadeId.ToString()) && (!string.IsNullOrWhiteSpace(entity.InscricaoEstadual)))
                throw new BusinessException("Preencha os campos Estado e Cidade para realizar a validação da Inscrição Estadual.");

            if (!string.IsNullOrWhiteSpace(entity.InscricaoEstadual))
            {
                var siglaUF = CidadeBL.AllIncluding(x => x.Estado).FirstOrDefault(x => x.Id == entity.CidadeId).Estado.Sigla;
                var msgErrorInscricaoEstadual = string.Empty;

                if (!InscricaoEstadualHelper.IsValid(siglaUF, entity.InscricaoEstadual, out msgErrorInscricaoEstadual))
                    throw new BusinessException("Inscrição Estadual inválida. (para este estado)");
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

        public void GetIdEstadoCidade(Pessoa entity)
        {
            if (!entity.CidadeId.HasValue && !entity.EstadoId.HasValue && !string.IsNullOrEmpty(entity.CidadeCodigoIbge))
            {
                var dadosCidade = CidadeBL.All.AsNoTracking().FirstOrDefault(x => x.CodigoIbge == entity.CidadeCodigoIbge);
                if (dadosCidade != null)
                {
                    entity.EstadoId = dadosCidade.EstadoId;
                    entity.CidadeId = dadosCidade.Id;
                }
            }
            else if (!entity.CidadeId.HasValue && !entity.EstadoId.HasValue && !string.IsNullOrEmpty(entity.EstadoCodigoIbge))
            {
                var dadosEstado = EstadoBL.All.AsNoTracking().FirstOrDefault(x => x.CodigoIbge == entity.EstadoCodigoIbge);
                if (dadosEstado != null)
                {
                    entity.EstadoId = dadosEstado.Id;
                }
            }
        }

        public override void Update(Pessoa entity)
        {
            GetIdEstadoCidade(entity);

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
            GetIdEstadoCidade(entity);

            ValidaModel(entity);
            if (!IsValid(entity))
            {
                var errors = entity.Notification.Errors.Cast<object>().Aggregate("", (current, item) => current + (item + "\n"));
                throw new BusinessException(errors);
            }

            base.Insert(entity);
        }

        public string Importa(Guid id)
        {
            var arquivo = ArquivoBL.All.FirstOrDefault(x => x.Id == id && x.Cadastro == "Pessoa");
            var conteudo = arquivo?.Conteudo;

            return conteudo;
        }

        public static List<string> ColunasParaImportacao()
        {
            return new List<string>
            {
                "Nome",
                "TipoDocumento",
                "CPFCNPJ",
                "CEP",
                "Endereco",
                "Numero",
                "Complemento",
                "Bairro",
                "Telefone",
                "Celular",
                "Contato",
                "Observacao",
                "Email",
                "NomeComercial",
                "Cliente",
                "Fornecedor",
                "Transportadora",
                "Vendedor"
            };
        }
    }
}