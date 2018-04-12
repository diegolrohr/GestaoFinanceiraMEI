using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Compras.Entities.ViewModel
{
    [Serializable]
    public class UnidadeMedidaVM : DomainBaseVM
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("abreviacao")]
        public string Abreviacao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }
}