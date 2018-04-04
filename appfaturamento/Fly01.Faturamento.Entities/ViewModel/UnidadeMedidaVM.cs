﻿using Fly01.Core.VM;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class UnidadeMedidaVM : DomainBaseVM
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("abreviacao")]
        public string Abreviacao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }
}