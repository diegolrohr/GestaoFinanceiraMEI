using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fly01.Core.Config
{
    public class StoneDataVM
    {
        public StoneDataVM()
        {
            Simulacoes = new List<ResponseAntecipacaoStoneVM>();
        }
        public string Token { get; set; }
        
        [JsonIgnore]
        public List<ResponseAntecipacaoStoneVM> Simulacoes { get; set; }
    }
}
