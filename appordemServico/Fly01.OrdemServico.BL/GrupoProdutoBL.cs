﻿using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System.Data.Entity;
using System.Linq;

namespace Fly01.OrdemServico.BL
{
    public class GrupoProdutoBL : PlataformaBaseBL<GrupoProduto>
    {
        public GrupoProdutoBL(AppDataContextBase context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(GrupoProduto entity)
        {
            entity.Fail(All.AsNoTracking().Any(x => x.Descricao.ToUpper() == entity.Descricao.ToUpper() && x.Id != entity.Id), DescricaoEmBranco);

            base.ValidaModel(entity);
        }

        public static Error DescricaoEmBranco = new Error("Descrição do grupo de produto já utilizada anteriormente.", "descricao");
    }
}
