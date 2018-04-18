using Fly01.Compras.DAL;
using Fly01.Compras.Domain.Entities;
using Fly01.Core.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.BL
{
    public class EnquadramentoLegalIPIBL : DomainBaseBL<EnquadramentoLegalIPI>
    {
        public EnquadramentoLegalIPIBL(AppDataContext context) : base(context) { }
    }
}
