using Newtonsoft.Json;
using System;
using TipoItemEnum = Fly01.Core.Entities.Domains.Enum.TipoItem;


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

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }

        [JsonProperty("tipoItem")]
        public string TipoItem { get; set; }

        [JsonProperty("produtoServicoDescricao")]
        public string ProdutoServicoDescricao
        {
            get
            {
                return TipoItem == TipoItemEnum.Produto.ToString() ? Produto?.Descricao : Servico?.Descricao;
            }
            set { }
        }

        [JsonProperty("kit")]
        public virtual KitVM Kit { get; set; }

        [JsonProperty("produto")]
        public virtual ProdutoVM Produto { get; set; }

        [JsonProperty("servico")]
        public virtual ServicoVM Servico { get; set; }
    }
}