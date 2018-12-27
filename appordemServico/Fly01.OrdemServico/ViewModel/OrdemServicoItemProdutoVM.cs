using Fly01.Core.Helpers.Attribute;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.OrdemServico.ViewModel
{
    public class OrdemServicoItemProdutoVM : OrdemServicoItemVM
    {
        [JsonProperty("produtoId")]
        public Guid ProdutoId { get; set; }

        #region Navigation
        [JsonProperty("produto")]
        public virtual ProdutoVM Produto { get; set; }
        #endregion
    }
}
