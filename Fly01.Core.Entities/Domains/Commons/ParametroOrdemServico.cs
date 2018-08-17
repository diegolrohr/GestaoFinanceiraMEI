﻿using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ParametroOrdemServico : PlataformaBase
    {
        public int DiasPadraoEntrega { get; set; }
        public Guid? ResponsavelPadraoId { get; set; }

        #region Navigation
        public virtual Pessoa ResponsavelPadrao { get; set; }
        #endregion
    }
}
