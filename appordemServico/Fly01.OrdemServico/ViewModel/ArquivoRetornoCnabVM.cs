using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Financeiro.ViewModel
{
    public class ArquivoRetornoCnabVM : DomainBaseVM
    {
        [JsonProperty("valueArquivo")]
        public string ValueArquivo { get; set; }

        [JsonProperty("contaBancariaId")]
        public Guid ContaBancariaId { get; set; }
        
    }
}