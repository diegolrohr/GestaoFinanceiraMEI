﻿using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Entities.Domains.Enum;
using Newtonsoft.Json;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class KitItem : PlataformaBase
    {
        public Guid KitId { get; set; }

        public Guid? ProdutoId { get; set; }

        public Guid? ServicoId { get; set; }

        public double Quantidade { get; set; }

        public TipoItem TipoItem{ get; set; }

        public virtual Kit Kit { get; set; }

        public virtual Produto Produto { get; set; }

        public virtual Servico Servico { get; set; }

    }
}