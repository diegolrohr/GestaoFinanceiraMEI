using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.OrdemServico.ViewModel
{
    public class OrdemServicoManutencaoVM : DomainBaseVM
    {
        [JsonProperty("ordemServicoId")]
        public Guid OrdemServicoId { get; set; }

        [JsonProperty("produtoId")]
        public Guid ProdutoId { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        #region Navigation
        [JsonProperty("ordemServico")]
        public virtual OrdemServicoVM OrdemServico { get; set; }

        [JsonProperty("produto")]
        public virtual ProdutoVM Produto { get; set; }
        #endregion
    }
}
