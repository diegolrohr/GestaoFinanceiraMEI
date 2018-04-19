using Fly01.Core.BL;
using System.Linq;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;

namespace Fly01.Financeiro.BL
{
    public class ContaBancariaBL : PlataformaBaseBL<ContaBancaria>
    {
        private SaldoHistoricoBL saldoHistoricoBL;

        public ContaBancariaBL(AppDataContext context, SaldoHistoricoBL saldoHistoricoBL) : base(context)
        {
            this.saldoHistoricoBL = saldoHistoricoBL;
        }

        public override void ValidaModel(ContaBancaria entity)
        {
            entity.Fail(All.Any(x => x.Id != entity.Id && x.NomeConta.ToUpper() == entity.NomeConta.ToUpper()), DescricaoDuplicada);

            base.ValidaModel(entity);
        }

        public override void Insert(ContaBancaria entity)
        {            
            base.Insert(entity);

            saldoHistoricoBL.InsereSaldoInicial(entity.Id);
        }

        public static Error DescricaoDuplicada = new Error("Descrição já utilizada anteriormente.", "nomeConta");
    }
}
