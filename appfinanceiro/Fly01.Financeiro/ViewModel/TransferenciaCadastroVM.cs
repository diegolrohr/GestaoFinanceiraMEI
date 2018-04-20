using System;
using Newtonsoft.Json;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Financeiro.ViewModel
{
    [Serializable]
    public class TransferenciaCadastroVM : MovimentacaoVM
    {
        [JsonProperty("categoriaDestinoId")]
        public Guid? CategoriaDestinoId { get; set; }

        [JsonProperty("categoriaDestino")]
        public virtual CategoriaVM CategoriaDestino { get; set; }

        [JsonProperty("descricaoDestino")]
        public string DescricaoDestino { get; set; }
    }
}