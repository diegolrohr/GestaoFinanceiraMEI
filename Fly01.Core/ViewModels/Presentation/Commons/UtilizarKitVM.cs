﻿using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class UtilizarKitVM : DomainBaseVM
    {
        [JsonProperty("kitId")]
        public Guid KitId  { get; set; }

        [JsonProperty("grupoTributarioProdutoId")]
        public Guid GrupoTributarioProdutoId { get; set; }

        [JsonProperty("grupoTributarioServicoId")]
        public Guid GrupoTributarioServicoId { get; set; }

        [JsonProperty("adicionarProdutos")]
        public bool AdicionarProdutos { get; set; }

        [JsonProperty("adicionarServicos")]
        public bool AdicionarServicos { get; set; }

        [JsonProperty("kit")]
        public virtual KitVM Kit { get; set; }

        [JsonProperty("grupoTributarioProduto")]
        public virtual GrupoTributarioVM GrupoTributarioProduto { get; set; }

        [JsonProperty("grupoTributarioServico")]
        public virtual GrupoTributarioVM GrupoTributarioServico { get; set; }
    }
}