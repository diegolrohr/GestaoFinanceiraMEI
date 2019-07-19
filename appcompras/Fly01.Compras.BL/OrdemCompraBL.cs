using Fly01.Compras.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.Compras.BL
{
    public class OrdemCompraBL : PlataformaBaseBL<OrdemCompra>
    {
        public OrdemCompraBL(AppDataContext context) : base(context)
        {
        }

        public IQueryable<OrdemCompra> Everything => repository.All.Where(x => x.PlataformaId == PlataformaUrl);

        public override void Insert(OrdemCompra entity)
        {
            entity.Fail(true, new Error("Não é possível inserir, somente em orçamento ou pedido"));
        }

        public override void Update(OrdemCompra entity)
        {
            entity.Fail(true, new Error("Não é possível atualizar, somente em orçamento ou pedido"));
        }

        public override void Delete(OrdemCompra entity)
        {
            entity.Fail(true, new Error("Não é possível deletar, somente em orçamento ou pedido"));
        }
    }
}