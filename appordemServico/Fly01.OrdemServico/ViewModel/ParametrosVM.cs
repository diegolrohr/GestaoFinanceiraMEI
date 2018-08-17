using Fly01.Core.Helpers.Attribute;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.OrdemServico.ViewModel
{
    public class ParametrosVM : DomainBaseVM
    {
        [JsonProperty("diasPrazoEntrega")]
        public string DiasPrazoEntrega { get; set; }


    }
}
