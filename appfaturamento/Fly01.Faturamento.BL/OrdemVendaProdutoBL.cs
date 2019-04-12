﻿using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System;

namespace Fly01.Faturamento.BL
{
    public class OrdemVendaProdutoBL : PlataformaBaseBL<OrdemVendaProduto>
    {
        public OrdemVendaProdutoBL(AppDataContextBase context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(OrdemVendaProduto entity)
        {
            entity.Fail(entity.Valor < 0, new Error("Valor não pode ser negativo", "valor"));
            entity.Fail(entity.Quantidade < 0, new Error("Quantidade não pode ser negativo", "quantidade"));
            entity.Fail(entity.Desconto < 0, new Error("Desconto não pode ser negativo", "desconto"));
            entity.Fail(entity.Desconto > (entity.Quantidade * entity.Valor), new Error("O Desconto não pode ser maior ao total", "desconto"));
            entity.Fail(entity.Total < 0, new Error("O Total não pode ser negativo", "total"));
            entity.Fail(Math.Round(entity.ValorCreditoICMS,2) > Math.Round((entity.Quantidade * entity.Valor),2), new Error("O valor do crédito ICMS não pode ser superior ao total bruto", "valorCreditoICMS"));

            base.ValidaModel(entity);
        }
    }
}