using Fly01.Core.BL;
using System.Data.Entity;
using Fly01.Core.Entities.Domains.Commons;
using System.Linq;
using Fly01.Core.Notifications;

namespace Fly01.EmissaoNFE.BL
{
    public class CidadeBL : DomainBaseBL<Cidade>
    {
        private readonly DbContext _context;

        public CidadeBL(DbContext context) : base(context)
        {
            _context = context;
        }
       
        public bool ValidaCodigoIBGE(string codigoIBGE)
        {
            return All.Any(x => x.CodigoIbge.ToUpper() == codigoIBGE.ToUpper());
        }

        public void ValidaCodigoIBGEException(string codigoIBGE)
        {
            var entity = new Cidade();
            entity.Fail(!ValidaCodigoIBGE(codigoIBGE), new Error("Código IBGE da cidade informado inválido ou inexistente", "codigoIbge"));

            base.ValidaModel(entity);
        }
    }
}
