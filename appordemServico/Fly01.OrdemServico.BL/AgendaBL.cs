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
            if (dataFinal == (dataInicial.AddDays(-1)))
            {
                List<AgendaVM> listaResult = new List<AgendaVM>();
                var response = OrdemServicoBL.AllIncluding(x => x.Cliente).Where(x => x.DataEntrega >= dataFinal && x.DataEntrega <= dataInicial).ToList();

                foreach (var item in response)
                {
                    TimeSpan horaFinal = item.HoraEntrega + TimeSpan.FromMinutes(item.Tempo.GetValueOrDefault());
                    TimeSpan horaEntrega = item.HoraEntrega;
                    TimeSpan duracao = (item.Tempo == null ? item.Duracao : (TimeSpan.FromMinutes(item.Tempo.GetValueOrDefault())));

                    var formatHoraInicial = string.Format("{0:00}:{1:00}", item.HoraEntrega.Hours, item.HoraEntrega.Minutes);
                    var formatHoraFinal = string.Format("{0:00}:{1:00}", horaFinal.Hours, horaFinal.Minutes);

                    listaResult.Add(new AgendaVM
                    {
                        ClassName = EnumHelper.GetCSS(typeof(StatusOrdemServico), item.Status.ToString()),
                        Title = item.Cliente?.Nome + " - " + formatHoraInicial + "h às "+ formatHoraFinal + "h",
                        Start = item.DataEntrega + horaEntrega,
                        End = item.DataEntrega + horaFinal,
                        Url = item.Status.Equals(StatusOrdemServico.Concluido) || item.Status.Equals(StatusOrdemServico.Cancelado) ? "" : $"OrdemServico/Edit/{item.Id.ToString()}"
                    });
                }
                return listaResult;
            }
            else
            {
                List<AgendaVM> listaResult = new List<AgendaVM>();

                var response = OrdemServicoBL.AllIncluding(x => x.Cliente).Where(x => x.DataEntrega >= dataFinal && x.DataEntrega <= dataInicial).ToList();

                foreach (var item in response)
                {
                    TimeSpan horaFinal = item.HoraEntrega + TimeSpan.FromMinutes(item.Tempo.GetValueOrDefault());
                    TimeSpan horaEntrega = (item.HoraEntrega - new TimeSpan(2, 0, 0));
                    TimeSpan duracao = (item.Tempo == null ? item.Duracao : (TimeSpan.FromMinutes(item.Tempo.GetValueOrDefault())) - new TimeSpan(2, 0, 0));
                    listaResult.Add(new AgendaVM
                    {
                        ClassName = EnumHelper.GetCSS(typeof(StatusOrdemServico), item.Status.ToString()),
                        Title = item.Cliente?.Nome,
                        Start = item.DataEntrega + horaEntrega,
                        End = item.DataEntrega + (horaFinal - new TimeSpan(2, 0, 0)),
                        Url = item.Status.Equals(StatusOrdemServico.Concluido) || item.Status.Equals(StatusOrdemServico.Cancelado) ? "" : $"OrdemServico/Edit/{item.Id.ToString()}"
                    });
                }

                return listaResult;
            }
        }
    }
}
