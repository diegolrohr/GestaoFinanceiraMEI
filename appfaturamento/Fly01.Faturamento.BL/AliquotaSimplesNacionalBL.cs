using Fly01.Core.BL;
using System.Data.Entity;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.BL
{
    public class AliquotaSimplesNacionalBL : DomainBaseBL<AliquotaSimplesNacional>
    {
        public AliquotaSimplesNacionalBL(DbContext context) : base(context)
        {
        }
    }
}
