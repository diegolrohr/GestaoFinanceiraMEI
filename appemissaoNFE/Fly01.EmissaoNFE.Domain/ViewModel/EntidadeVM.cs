using Fly01.Core.Entities.Domains;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class EntidadeVM : PlataformaBase
    {
        public string Producao { get; set; }
        public string Homologacao { get; set; }
        public TipoAmbiente EntidadeAmbiente { get; set; }
        public TipoAmbiente EntidadeAmbienteNFS { get; set; }
    }
}
