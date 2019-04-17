﻿using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Compras.DAL;
using Fly01.Core.Notifications;
using System.Linq;

namespace Fly01.Compras.BL
{
    public class NFeProdutoEntradaBL : PlataformaBaseBL<NFeProdutoEntrada>
    {
        public NFeProdutoEntradaBL(AppDataContext context) : base(context)
        {
        }

        public override void ValidaModel(NFeProdutoEntrada entity)
        {
            entity.Fail(entity.Valor <= 0, new Error("Valor deve ser superior a zero", "valor"));
            entity.Fail(entity.Quantidade <= 0, new Error("Quantidade deve ser superior a zero", "quantidade"));
            entity.Fail(entity.Desconto < 0, new Error("Desconto não pode ser negativo", "desconto"));
            entity.Fail(entity.Desconto >= (entity.Quantidade * entity.Valor), new Error("O Desconto não pode ser maior ou igual ao total", "desconto"));
            entity.Fail(entity.Total <= 0, new Error("O Total deve ser superior a zero", "total"));

            base.ValidaModel(entity);
        }
    }
}