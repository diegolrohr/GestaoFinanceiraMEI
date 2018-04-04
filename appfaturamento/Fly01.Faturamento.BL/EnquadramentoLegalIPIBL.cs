using Fly01.Faturamento.DAL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core.Api.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly01.Faturamento.BL
{
    public class EnquadramentoLegalIPIBL : DomainBaseBL<EnquadramentoLegalIPI>
    {
        public EnquadramentoLegalIPIBL(AppDataContext context) : base(context) { }
    }
}
