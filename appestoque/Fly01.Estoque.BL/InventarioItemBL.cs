using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System.Linq;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.BL
{
    public class InventarioItemBL : PlataformaBaseBL<InventarioItem>
    {
        private static Error produtoInseridoEmInventarioAberto = new Error("Produto ja inserido em inventário Aberto. Favor escolher outro produto.");
        private static Error novoSaldoNegativo = new Error("O Novo Saldo não pode ser menor que 0.");
        private MovimentoBL movimentoBL;


        public InventarioItemBL(AppDataContextBase context, MovimentoBL movimentoBL) : base(context)
        {
            this.movimentoBL = movimentoBL;
        }

        public override void ValidaModel(InventarioItem entity)
        {
            var inventarioItensAbertosAtivos = All.Where(e => e.Ativo && e.Id != entity.Id && e.Inventario.Ativo && e.Inventario.InventarioStatus == InventarioStatus.Aberto);

            entity.Fail(inventarioItensAbertosAtivos.Any(e => e.ProdutoId == entity.ProdutoId), produtoInseridoEmInventarioAberto);

            entity.Fail(entity.SaldoInventariado < 0, novoSaldoNegativo);
            
            base.ValidaModel(entity);
        }

        public void Movimenta(Inventario entity)
        {
            All.Where(e => e.InventarioId == entity.Id && e.Ativo).ToList().ForEach(i =>
            {
                movimentoBL.Movimenta(i);
            });
        }
    }
}
