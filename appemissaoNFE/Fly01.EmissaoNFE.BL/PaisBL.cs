using Fly01.Core.BL;
using System.Data.Entity;
using Fly01.Core.Entities.Domains.Commons;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.EmissaoNFE.BL
{
    public class PaisBL : DomainBaseBL<Pais>
    {
        private readonly DbContext _context;

        public PaisBL(DbContext context) : base(context)
        {
            _context = context;
        }

        public Pais FindByNome(string nome)
        {
            List<Pais> paises = _context.Set<Pais>()
                .SqlQuery("SELECT * FROM Pais WHERE UPPER(nome COLLATE SQL_Latin1_General_CP1_CI_AI) LIKE @p0", nome.ToUpper())
                .AsNoTracking()
                .ToListAsync().Result;

            if (paises == null || !paises.Any())
            {
                return null;
            }

            return paises[0];
        }
    }
}
