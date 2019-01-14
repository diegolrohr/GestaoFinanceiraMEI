using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System;
using System.Linq;

namespace Fly01.Faturamento.BL
{
    public class OrdemVendaServicoBL : PlataformaBaseBL<OrdemVendaServico>
    {
        public OrdemVendaServicoBL(AppDataContextBase context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(OrdemVendaServico entity)
        {
            entity.Fail(entity.Valor < 0, new Error("Valor deve ser superior a zero", "valor"));
            entity.Fail(entity.Quantidade <= 0, new Error("Quantidade deve ser superior a zero", "quantidade"));
            entity.Fail(entity.Desconto < 0, new Error("Desconto não pode ser negativo", "desconto"));
            entity.Fail(entity.Desconto > (entity.Quantidade * entity.Valor), new Error("O Desconto não pode ser maior ao total", "desconto"));
            entity.Fail(entity.Total < 0, new Error("O Total não pode ser negativo", "total"));

            var jaExiste = All.Any(x => x.OrdemVendaId == entity.OrdemVendaId && x.ServicoId == entity.ServicoId && x.Id != entity.Id);
            entity.Fail(jaExiste, new Error("Este serviço já está adicionado"));

            var previusProritario = All.Where(x => x.OrdemVendaId == entity.OrdemVendaId && x.IsServicoPrioritario && x.Id != entity.Id).FirstOrDefault();
            if (entity.IsServicoPrioritario && previusProritario != null)
            {
                previusProritario.IsServicoPrioritario = false;
                UpdateIsPrioritario(previusProritario);
            }

            if (!entity.IsServicoPrioritario && previusProritario == null)
                entity.IsServicoPrioritario = true;
            
            base.ValidaModel(entity);
        }

        public void UpdateIsPrioritario(OrdemVendaServico entity)
        {
            entity.PlataformaId = PlataformaUrl;
            entity.DataAlteracao = DateTime.Now;
            entity.DataExclusao = null;
            entity.UsuarioAlteracao = AppUser;
            entity.UsuarioExclusao = null;
            if (!entity.Ativo)
            {
                entity.UsuarioExclusao = AppUser;
                entity.DataExclusao = DateTime.Now;
            }
        }
    }
}