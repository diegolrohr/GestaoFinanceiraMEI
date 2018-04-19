﻿using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Financeiro.API.Models.DAL;

namespace Fly01.Financeiro.BL
{
    public class FormaPagamentoBL : PlataformaBaseBL<FormaPagamento>
    {
        public FormaPagamentoBL(AppDataContext context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }
        
        public override void ValidaModel(FormaPagamento entity)
        {
            entity.Fail(All.Any(x => x.Id != entity.Id && (x.Descricao.ToUpper() == entity.Descricao.ToUpper() && x.TipoFormaPagamento == entity.TipoFormaPagamento)), new Error("Descrição já utilizada anteriormente.", "descricao"));

            base.ValidaModel(entity);
        }
    }
}