using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System.Linq;
using System.Data.Entity;
using Fly01.Core.Entities.Domains.Enum;
using System;
using System.Collections.Generic;

namespace Fly01.Financeiro.BL
{
    public class ContaFinanceiraBaixaMultiplaBL : EmpresaBaseBL<ContaFinanceiraBaixaMultipla>
    {
        private ContaFinanceiraBaixaBL ContaFinanceiraBaixaBL;
        private ContaFinanceiraBL ContaFinanceiraBL;

        public ContaFinanceiraBaixaMultiplaBL(AppDataContext context, ContaFinanceiraBaixaBL contaFinanceiraBaixaBL, ContaFinanceiraBL contaFinanceiraBL) : base(context)
        {
            this.ContaFinanceiraBaixaBL = contaFinanceiraBaixaBL;
            this.ContaFinanceiraBL = contaFinanceiraBL;
        }

        public override void ValidaModel(ContaFinanceiraBaixaMultipla entity)
        {
            var contas = ContaFinanceiraBL.All.AsNoTracking().Where(x => entity.ContasFinanceirasIds.Contains(x.Id));

            entity.Fail(!contas.Any(), new Error("Informe ao menos uma conta financeira para realizar a baixa."));
            entity.Fail(contas.Any(x => x.TipoContaFinanceira != entity.TipoContaFinanceira), new Error("Adicione somente contas financeiras do mesmo tipo."));
            entity.Fail(contas.Count() > 50, new Error("Permitido até 50 contas financeiras por baixa múltipla."));
            entity.Fail(contas.Any(x => x.StatusContaBancaria != StatusContaBancaria.EmAberto && x.StatusContaBancaria != StatusContaBancaria.BaixadoParcialmente), new Error("Adicione somente contas financeiras de status (Em aberto) ou (Baixado parcialmente)."));

            base.ValidaModel(entity);
        }

        public override void Insert(ContaFinanceiraBaixaMultipla entity)
        {
            entity.EmpresaId = EmpresaId;
            entity.DataInclusao = DateTime.Now;
            entity.DataAlteracao = null;
            entity.DataExclusao = null;
            entity.UsuarioInclusao = AppUser;
            entity.UsuarioAlteracao = null;
            entity.UsuarioExclusao = null;
            entity.Ativo = true;

            ValidaModel(entity);

            if (entity.Id == default(Guid) || entity.Id == null)
                entity.Id = Guid.NewGuid();
        }
        public List<ContaFinanceiraBaixa> GeraBaixas(ContaFinanceiraBaixaMultipla entity)
        {
            var contas = ContaFinanceiraBL.All.AsNoTracking().Where(x => entity.ContasFinanceirasIds.Contains(x.Id)).ToList();

            return contas.Select(x => new ContaFinanceiraBaixa()
            {
                Data = entity.Data,
                ContaBancariaId = entity.ContaBancariaId,
                ContaFinanceiraId = x.Id,
                Observacao = x.Descricao + " " + entity.Observacao ?? "",
                Valor = x.Saldo
            }).ToList();
        }

        public override void Update(ContaFinanceiraBaixaMultipla entity)
        {
            throw new BusinessException("Não é possível alterar baixas múltiplas.");
        }

        public override void Delete(ContaFinanceiraBaixaMultipla entity)
        {
            throw new BusinessException("Não é possível deletar baixas múltiplas.");
        }
    }
}