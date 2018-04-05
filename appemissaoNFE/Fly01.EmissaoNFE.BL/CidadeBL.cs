using Fly01.EmissaoNFE.Domain;
using Fly01.Core.BL;
using System.Data.Entity;

namespace Fly01.EmissaoNFE.BL
{
    public class CidadeBL : DomainBaseBL<Cidade>
    {
        private readonly DbContext _context;

        public CidadeBL(DbContext context) : base(context)
        {
            _context = context;
        }
    }
}
