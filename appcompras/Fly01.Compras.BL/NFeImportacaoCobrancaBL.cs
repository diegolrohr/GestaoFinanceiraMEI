﻿using Fly01.Core.BL;
using Fly01.Compras.DAL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.BL
{
    public class NFeImportacaoCobrancaBL : PlataformaBaseBL<NFeImportacaoCobranca>
    {
        public NFeImportacaoCobrancaBL(AppDataContext context) : base(context)
        {
        }
    }
}