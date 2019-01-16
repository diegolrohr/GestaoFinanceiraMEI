using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.OrdemServico.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Fly01.OrdemServico.BL
{
    public class AgendaBL : DomainBaseBL<Agenda>
    {
        protected OrdemServicoBL OrdemServicoBL;
        public AgendaBL(AppDataContext context, OrdemServicoBL ordemServicoBL) : base(context)
        {
            OrdemServicoBL = ordemServicoBL;
        }

        public List<AgendaVM> GetCalendar(DateTime dataFinal, DateTime dataInicial)
        {
            List<AgendaVM> listaResult = new List<AgendaVM>();

            var response = OrdemServicoBL.AllIncluding(x => x.Cliente).Where(x => x.DataEntrega >= dataFinal && x.DataEntrega <= dataInicial).ToList();

            foreach (var item in response)
            {
                listaResult.Add(new AgendaVM
                {
                    Status = item.Status.ToString(),
                    Cliente = item.Cliente?.Nome,
                    Entrega = item.DataEntrega,
                    OSId = item.Id.ToString()
                });
            }

            return listaResult;
        }
    }
}
