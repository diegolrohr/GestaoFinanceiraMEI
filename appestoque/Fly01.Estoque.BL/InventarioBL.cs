using Fly01.Estoque.Domain.Entities;
using Fly01.Core.BL;
using System;
using System.Linq;
using Fly01.Core.Notifications;
using System.Data.Entity;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Estoque.BL
{
    public class InventarioBL : PlataformaBaseBL<Inventario>
    {
        private InventarioItemBL inventarioItemBl;
        private static Error descricaoUtilizadaEmOutroInventario = new Error("A descrição já foi utilizada em outro cadastro. Informe uma descrição diferente.", "descricao");
        private static Error inventarioFinalizado = new Error("Impossivel atualizar inventário finalizado.");
        private static Error inventarioSemItem = new Error("Impossível finalizar inventário sem produtos.");

        public InventarioBL(AppDataContextBase context, InventarioItemBL inventarioItemBl) : base(context)
        {
            this.inventarioItemBl = inventarioItemBl;
        }

        public override void Insert(Inventario entity)
        {
            entity.InventarioStatus = InventarioStatus.Aberto;
            base.Insert(entity);
        }

        public override void ValidaModel(Inventario entity)
        {
            var inventariosAtivosAbertos = All.Where(e => e.Ativo && e.Id != entity.Id);

            var registered = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);

            if(registered != null)
            {
                InventarioStatus oldStatus = registered.InventarioStatus;
                entity.Fail(oldStatus == InventarioStatus.Finalizado, inventarioFinalizado);
            }
            
            entity.Fail(inventariosAtivosAbertos.Any(e => e.Descricao == entity.Descricao), descricaoUtilizadaEmOutroInventario);

            entity.DataUltimaInteracao = DateTime.Now;

            base.ValidaModel(entity);
        }

        public override void Update(Inventario entity)
        {
            InventarioStatus oldStatus = All.AsNoTracking().FirstOrDefault(x => x.Id == entity.Id).InventarioStatus;

            if (oldStatus == InventarioStatus.Aberto && entity.InventarioStatus == InventarioStatus.Finalizado)
                Finaliza(entity);

            base.Update(entity);
        }

        public void ValidaFinalizacao(Inventario entity)
        {
            var exists = inventarioItemBl.All.Any(e => e.InventarioId == entity.Id && e.Ativo);

            entity.Fail(!exists, inventarioSemItem);
        }

        private void Finaliza(Inventario entity)
        {
            ValidaFinalizacao(entity);

            inventarioItemBl.Movimenta(entity);
        }

    }
}
