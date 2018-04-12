﻿using System;
using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;
using Fly01.Core.Attribute;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class MovimentacaoPorCategoriaVM : DomainBaseVM
    {
        [JsonProperty("categoriaId")]
        public Guid CategoriaId { get; set; }
        [JsonProperty("categoria")]
        public string Categoria { get; set; }
        [JsonProperty("categoriaPaiId")]
        public Guid? CategoriaPaiId { get; set; }
        [JsonProperty("previsto")]
        public double Previsto { get; set; }
        [JsonProperty("realizado")]
        public double? Realizado { get; set; }
        [JsonProperty("soma")]
        public double Soma { get; set; }
        [JsonProperty("tipoCarteira")]
        [APIEnum("TipoCarteira")]
        public string TipoCarteira { get; set; }
    }
}