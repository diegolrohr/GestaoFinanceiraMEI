using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.OrdemServico.DAL;
using System;
using System.Collections.Generic;
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
                    _parametroPlataforma = base.All.AsNoTracking().FirstOrDefault();
                    if (_parametroPlataforma == null)
                    {
                        _parametroPlataforma = new ParametroOrdemServico
                        {
                            PlataformaId = PlataformaUrl,
                            UsuarioInclusao = AppUser
                        };
                    }
                }

                return _parametroPlataforma;
            }
        }

        public override IQueryable<ParametroOrdemServico> All => (new List<ParametroOrdemServico>
                {
                    ParametroPlataforma
                }).AsQueryable().AsQueryable();

        public override void ValidaModel(ParametroOrdemServico entity)
        {
            entity.Fail(entity.DiasPrazoEntrega < 0, new Error("Dias para entrega devem ser igual ou maior que zero", "DiasPazoEntrega"));
            base.ValidaModel(entity);
        }

        public override void Insert(ParametroOrdemServico entity)
        {
            if (ParametroPlataforma.Id == Guid.Empty) base.Insert(entity);
            else
            {
                entity.Id = ParametroPlataforma.Id;
                entity.UsuarioInclusao = ParametroPlataforma.UsuarioInclusao;
                entity.PlataformaId = ParametroPlataforma.PlataformaId;
                entity.DataInclusao = ParametroPlataforma.DataInclusao;
                base.Update(entity);
                AttachForUpdate(entity);
            }
        }

        public override void Update(ParametroOrdemServico entity)
        {
            entity.Fail(true, new Error("Esta entidade não suporta o método PUT. Utilize POST"));
            base.ValidaModel(entity);
        }

        public override void Delete(Guid id) => Delete(ParametroPlataforma);

        public override void Delete(ParametroOrdemServico entityToDelete)
        {
            entityToDelete.Fail(true, new Error("Esta entidade não suporta deleção"));
            base.ValidaModel(entityToDelete);
        }
    }
}
