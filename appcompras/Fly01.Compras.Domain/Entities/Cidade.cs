﻿using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.Domain.Entities
{
    public class Cidade : CidadeBase
    {
        public virtual Estado Estado { get; set; }
    }
}
