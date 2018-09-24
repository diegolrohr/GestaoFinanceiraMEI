using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Faturamento.DAL;
using Fly01.Core.Notifications;
using System.Linq;
using System;
using System.Data.Entity;
using System.Collections.Generic;

namespace Fly01.Faturamento.BL
{
    public class NotaFiscalItemTributacaoBL : PlataformaBaseBL<NotaFiscalItemTributacao>
    {
        private NotaFiscalItemBL NotaFiscalItemBL { get; set; }

        public NotaFiscalItemTributacaoBL(AppDataContext context, NotaFiscalItemBL notaFiscalItemBL) : base(context)
        {
            NotaFiscalItemBL = notaFiscalItemBL;
        }

        public override void ValidaModel(NotaFiscalItemTributacao entity)
        {
            var jaExiste = All.Any(x => x.NotaFiscalItemId == entity.NotaFiscalItemId && x.Id != entity.Id);
            entity.Fail(jaExiste, new Error("Já existe um registro de tributação para este item da nota fiscal"));

            base.ValidaModel(entity);
        }

        public List<NotaFiscalItemTributacao> GetConfiguracoesGrupoTributarioFinalizacaoPedido(Guid notaFiscalId)
        {
            //agrupar distinct GrupoTributario
            var gruposTributarios = (from notaFiscalItem in NotaFiscalItemBL.All.AsNoTracking().Where(x => x.NotaFiscalId == notaFiscalId)
                                    group notaFiscalItem by notaFiscalItem.GrupoTributarioId into groupResult
                                    select new
                                    {
                                        GrupoTributarioId = groupResult.Key
                                    });

            var result = new List<NotaFiscalItemTributacao>();

            //vários notafiscalItem que podem usar o mesmo grupo tributario
            //não interessa de qual deles, pois quero só mostrar os checks do grupo tributario e não os detalhes dos impostos 
            foreach (var grupoTrib in gruposTributarios)
            {
                var notaFiscalItemId = NotaFiscalItemBL.All.AsNoTracking().Where(x => x.NotaFiscalId == notaFiscalId && x.GrupoTributarioId == grupoTrib.GrupoTributarioId).FirstOrDefault().Id;
                result.Add(All.AsNoTracking().Where(x => x.NotaFiscalItemId == notaFiscalItemId).FirstOrDefault());
            }

            return result;

        }
    }
}