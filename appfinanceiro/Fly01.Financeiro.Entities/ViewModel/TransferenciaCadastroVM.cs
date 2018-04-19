using System;
using Newtonsoft.Json;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class TransferenciaCadastroVM : MovimentacaoVM
    {
        [JsonProperty("categoriaDestinoId")]
        public Guid? CategoriaDestinoId { get; set; }

        #region Navigations Properties
        [JsonProperty("categoriaDestino")]
        public virtual CategoriaVM CategoriaDestino { get; set; }

        [JsonProperty("descricaoDestino")]
        public string DescricaoDestino { get; set; }
        #endregion
    }
}