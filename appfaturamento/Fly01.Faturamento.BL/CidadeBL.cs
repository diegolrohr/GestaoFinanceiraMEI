using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.BL;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.BL
{
    public class CidadeBL : DomainBaseBL<Cidade>
    {
        private readonly DbContext _context;

        public CidadeBL(DbContext context) : base(context)
        {
            _context = context;
        }

        public Cidade FindByNome(string nome)
        {
            List<Cidade> cidades = _context.Set<Cidade>()
                .SqlQuery("SELECT * FROM Cidade WHERE UPPER(nome COLLATE SQL_Latin1_General_CP1_CI_AI) LIKE @p0", nome.ToUpper())
                .AsNoTracking()
                .ToListAsync().Result;

            if (cidades == null || !cidades.Any())
            {
                return null;
            }

            return cidades[0];
        }
    }
}
