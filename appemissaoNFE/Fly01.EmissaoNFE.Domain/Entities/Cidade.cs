﻿using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.EmissaoNFE.Domain
{
    public class Cidade : CidadeBase
    {
        public virtual Estado Estado { get; set; }
    }
}
