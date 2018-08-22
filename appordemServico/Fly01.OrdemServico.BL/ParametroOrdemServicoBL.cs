using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.OrdemServico.DAL;
using System.Linq;

namespace Fly01.OrdemServico.BL
{
    public class ParametroOrdemServicoBL : PlataformaBaseBL<ParametroOrdemServico>
    {
        private ParametroOrdemServico _parametroPlataforma;

        public ParametroOrdemServicoBL(AppDataContext context) : base(context)
        {

        }

        public ParametroOrdemServico ParametroPlataforma
        {
            get
            {
                if (_parametroPlataforma == null)
                {
                    _parametroPlataforma = All.FirstOrDefault();
                    if (_parametroPlataforma == null)
                        _parametroPlataforma = new ParametroOrdemServico();
                }

                return _parametroPlataforma;
            }
        }
    }
}
