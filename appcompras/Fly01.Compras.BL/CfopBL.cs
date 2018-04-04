﻿using Fly01.Compras.DAL;
using Fly01.Compras.Domain.Entities;
using Fly01.Core.Api.BL;

namespace Fly01.Compras.BL
{
    public class CfopBL : DomainBaseBL<Cfop>
    {
        public CfopBL(AppDataContext context) : base(context) { }
    }
}