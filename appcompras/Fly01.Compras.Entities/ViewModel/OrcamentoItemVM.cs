using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Compras.Entities.ViewModel
{
    [Serializable]
    public class OrcamentoItemVM : OrdemCompraItemVM
    {
        [JsonProperty("orcamentoId")]
        public Guid OrcamentoId { get; set; }

        [JsonProperty("fornecedorId")]
        public Guid FornecedorId { get; set; }

        #region Navigations Properties

        [JsonProperty("fornecedor")]
        public virtual PessoaVM Fornecedor { get; set; }

        [JsonProperty("orcamento")]
        public virtual OrcamentoVM Orcamento { get; set; }
        #endregion
    }
}
