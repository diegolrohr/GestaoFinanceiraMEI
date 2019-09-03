using Fly01.Core.BL;
using System.Linq;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System;

namespace Fly01.Financeiro.BL
{
    public class ContaBancariaBL : EmpresaBaseBL<ContaBancaria>
    {
        private SaldoHistoricoBL saldoHistoricoBL;
        private BancoBL bancoBL;

        public ContaBancariaBL(AppDataContext context, SaldoHistoricoBL saldoHistoricoBL, BancoBL bancoBL) : base(context)
        {
            this.saldoHistoricoBL = saldoHistoricoBL;
            this.bancoBL = bancoBL;
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(ContaBancaria entity)
        {
            entity.Fail(All.Any(x => x.Id != entity.Id && x.NomeConta.ToUpper() == entity.NomeConta.ToUpper()), 
                new Error("Descrição da conta bancária já utilizada anteriormente.", "nomeConta", All.FirstOrDefault(x => x.Id != entity.Id && x.NomeConta.ToUpper() == entity.NomeConta.ToUpper())?.Id.ToString()));

            var banco = bancoBL.All.FirstOrDefault(x => x.Id == entity.BancoId);
            if(banco != null && banco.Codigo != "999")
            {
                var dadosAgenciaContaInvalid = string.IsNullOrWhiteSpace(entity.Agencia) ||
                    string.IsNullOrWhiteSpace(entity.Conta) ||
                    string.IsNullOrWhiteSpace(entity.DigitoConta);

                entity.Fail(dadosAgenciaContaInvalid, AgenciaContaObrigatoria);
            }

            base.ValidaModel(entity);
        }

        public override void Insert(ContaBancaria entity)
        {            
            base.Insert(entity);

            saldoHistoricoBL.InsereSaldoInicial(entity.Id, entity.ValorInicial == null ? 0 : entity.ValorInicial);
        }

        public static Error DescricaoDuplicada = new Error("Descrição da conta bancária já utilizada anteriormente.", "nomeConta");
        public static Error AgenciaContaObrigatoria = new Error("Dados de agência e conta são obrigatórios.", "agencia");
        public static Error BancoInvalido = new Error("Código do Banco inválido.", "bancoId");
    }
}