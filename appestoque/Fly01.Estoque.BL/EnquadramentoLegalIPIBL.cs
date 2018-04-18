﻿using Fly01.Estoque.DAL;
using Fly01.Estoque.Domain.Entities;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.BL
{
    public class EnquadramentoLegalIPIBL : DomainBaseBL<EnquadramentoLegalIPI>
    {
        public EnquadramentoLegalIPIBL(AppDataContext context) : base(context) { }
    }
}
