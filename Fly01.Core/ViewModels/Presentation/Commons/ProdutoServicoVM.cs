using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ProdutoServicoVM : DomainBaseVM
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("valorCusto")]
        public double ValorCusto { get; set; }

        [JsonProperty("valorVenda")]
        public double ValorVenda { get; set; }

        [JsonProperty("tipoItem")]
        public TipoItem TipoItem { get; set; }

        [JsonProperty("tipoItemDescricao")]
        public string TipoItemDescricao
        {
            get
            {
                return EnumHelper.GetDescription(typeof(TipoItem), TipoItem.ToString());
            }
            set { }
        }
    }
}