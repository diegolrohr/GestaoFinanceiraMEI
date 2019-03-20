using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        [JsonProperty("valorVenda")]
        public double ValorVenda { get; set; }

        [JsonProperty("valorCusto")]
        public double ValorCusto { get; set; }

        [JsonProperty("valorServico")]
        public double ValorServico { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }

        [JsonProperty("tipoItem")]
        public string TipoItem { get; set; }

        public string PropTipoItemDescricao { get; set; }

        [JsonProperty("tipoItemDescricao")]
        public string TipoItemDescricao
        {
            get
            {
                return EnumHelper.GetDescription(typeof(TipoItem), TipoItem);
            }
            set { PropTipoItemDescricao = value; }
        }

        [JsonProperty("produtoServicoDescricao")]
        public string ProdutoServicoDescricao
        {
            get
            {
                return TipoItem == TipoItemEnum.Produto.ToString() ? Produto?.Descricao : Servico?.Descricao;
            }
            set { }
        }

        [JsonProperty("produtoServicoCodigo")]
        public string ProdutoServicoCodigo
        {
            get
            {
                return TipoItem == TipoItemEnum.Produto.ToString() ? Produto?.CodigoProduto : Servico?.CodigoServico;
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