using Newtonsoft.Json;
using System;


namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class KitItemVM : DomainBaseVM
    {
        [JsonProperty("kitId")]
        public Guid KitId { get; set; }

        [JsonProperty("produtoId")]
        public Guid? ProdutoId { get; set; }

        [JsonProperty("servicoId")]
        public Guid? ServicoId { get; set; }

        [JsonProperty("kit")]
        public virtual KitVM Kit { get; set; }

        [JsonProperty("produto")]
        public virtual ProdutoVM Produto { get; set; }

        [JsonProperty("servico")]
        public virtual ServicoVM Servico { get; set; }
    }
}