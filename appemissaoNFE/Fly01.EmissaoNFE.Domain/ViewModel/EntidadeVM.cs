using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.Core.Domain;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class EntidadeVM : PlataformaBase
    {
        public string Producao { get; set; }
        public string Homologacao { get; set; }
        public TipoAmbiente EntidadeAmbiente { get; set; }
    }
}
