using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.OrdemServico.ViewModel
{
    public class OrdemServicoItemVM : DomainBaseVM
    {
        [JsonProperty("ordemServicoId")]
        public Guid OrdemServicoId { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("desconto")]
        public double Desconto { get; set; }

        [JsonIgnore]
        public double Total => Quantidade > 0 ? Math.Round(((Quantidade * Valor) - Desconto), 2, MidpointRounding.AwayFromZero) : Math.Round((Valor - Desconto), 2, MidpointRounding.AwayFromZero);

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        #region Navigation
        [JsonProperty("ordemServico")]
        public virtual OrdemServicoVM OrdemServico { get; set; }
        #endregion
    }
}
