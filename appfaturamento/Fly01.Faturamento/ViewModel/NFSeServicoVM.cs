using System;
using Newtonsoft.Json;

namespace Fly01.Faturamento.ViewModel
{
    [Serializable]
    public class NFSeServicoVM : NotaFiscalItemVM
    {
        [JsonProperty("servicoId")]
        public Guid ServicoId { get; set; }

        [JsonProperty("valorOutrasRetencoes")]
        public double? ValorOutrasRetencoes { get; set; }

        [JsonProperty("descricaoOutrasRetencoes")]
        public string DescricaoOutrasRetencoes { get; set; }

        #region NavigationProperties

        [JsonProperty("servico")]
        public virtual ServicoVM Servico { get; set; }

        #endregion
    }
}