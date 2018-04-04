﻿using Fly01.Core.Api.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly01.Compras.Domain.Entities
{
    public class EnquadramentoLegalIPI : DomainBase
    {
        public string Codigo { get; set; }

        public string GrupoCST { get; set; }

        [MaxLength(600)]
        public string Descricao { get; set; }
    }
}
