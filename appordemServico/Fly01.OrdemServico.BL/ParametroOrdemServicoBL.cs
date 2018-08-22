using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.OrdemServico.DAL;
using System.Data.Entity;
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
                    _parametroPlataforma = All.AsNoTracking().FirstOrDefault();
                    if (_parametroPlataforma == null)
                        _parametroPlataforma = new ParametroOrdemServico();
                }

                return _parametroPlataforma;
            }
        }

        public override void ValidaModel(ParametroOrdemServico entity)
        {
            entity.Fail(entity.DiasPadraoEntrega < 0, new Error("Dias para entrega devem ser igual ou maior que zero", "DiasPadraoEntrega"));
            base.ValidaModel(entity);
        }

        public override void Insert(ParametroOrdemServico entity)
        {
            entity.Fail(All.AsNoTracking().FirstOrDefault() != null, new Error("Só é permitido um registro de configuração por plataforma!"));
            base.Insert(entity);
        }
    }
}
